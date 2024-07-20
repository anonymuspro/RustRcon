#region

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RustRcon.Pooling;
using RustRcon.Types;
using RustRcon.Types.Commands.Base;
using RustRcon.Types.Response.Server;
using RustRcon.Types.Server.Messages;
using Services.Client;

#endregion

namespace RustRcon
{
    public class RconClient : WebSocketClient
    {
        private readonly Dictionary<Int32, TaskCompletionSource<ServerResponse>> _commandHandlers;

        /// <summary>
        ///     Called when a new console message is received from the server.
        /// </summary>
        public event Action<ConsoleMsg>? OnConsoleMessage;

        /// <summary>
        ///     Called when a new chat message is received from the server.
        /// </summary>
        public event Action<ChatMsg>? OnChatMessage;


        /// <summary>
        ///     Initialise new remote connection to a specified server.
        /// </summary>
        /// <param name="host">IP, hostname or FQDN of the host</param>
        /// <param name="port">Query port, usualy game port + 1</param>
        /// <param name="password">RCON passphrase</param>
        /// <param name="logger">WebSockets logger</param>
        public RconClient(string host, int port, string password, ILogger? logger = null) : base(
            new Uri($"ws://{host}:{port}/{password}"), logger)
        {
            _commandHandlers = new Dictionary<int, TaskCompletionSource<ServerResponse>>();
        }

        public override async Task ConnectAsync(CancellationToken cancellation = default)
        {
            await base.ConnectAsync(cancellation);

            _ = Task.Run(() => HandleMessages(ConnectionCts!.Token), ConnectionCts!.Token);
        }

        public async Task SendCommandAsync(BaseCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            await SendAsync(command, cancellationToken);
            var readTcs = new TaskCompletionSource<ServerResponse>();

            cancellationToken.Register(() =>
            {
                readTcs.TrySetCanceled();
                _commandHandlers.Remove(command.Id);
            });

            _commandHandlers.Add(command.Id, readTcs);
            var serverResponse = await readTcs.Task;
            command.Complete(serverResponse);
        }

        private async void HandleMessages(CancellationToken cancellation)
        {
            while (!cancellation.IsCancellationRequested)
            {
                ServerResponse? serverResponse = await ReadAsync<ServerResponse>(cancellation);

                if (serverResponse == null)
                    continue;

                if (!_commandHandlers.Remove(serverResponse.Id, out TaskCompletionSource<ServerResponse>? tcs))
                {
                    ConsoleMsg? consoleMsg = null;
                    ChatMsg? chatMsg = null;

                    try
                    {
                        if (serverResponse.Content.StartsWith("[rcon]"))
                            continue;

                        switch (serverResponse.Id)
                        {
                            case 0:
                            {
                                consoleMsg = RustRconPool.Get<ConsoleMsg>();
                                consoleMsg.Message = serverResponse.Content;
                                consoleMsg.Type = serverResponse.Type.ToEnum<ConsoleMsg.MessageType>();

                                OnConsoleMessage?.Invoke(consoleMsg);
                                break;
                            }
                            default:
                            {
                                if (serverResponse.Type != "Chat")
                                    continue;

                                chatMsg = RustRconPool.Get<ChatMsg>();
                                JsonConvert.PopulateObject(serverResponse.Content, chatMsg);

                                OnChatMessage?.Invoke(chatMsg);
                                break;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        consoleMsg?.Dispose();
                        chatMsg?.Dispose();
                    }

                    continue;
                }

                tcs.SetResult(serverResponse);
            }

            foreach (var (_, tcs) in _commandHandlers)
            {
                tcs.TrySetCanceled();
            }
        }
    }
}