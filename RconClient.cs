using Newtonsoft.Json;
using RustRcon.Types.Commands.Base;
using RustRcon.Types.Response;
using RustRcon.Types.Server.Messages;
using System;
using System.Collections.Generic;
using WebSocketSharp;

namespace RustRcon
{
    public class RconClient : IDisposable
    {
        private string _host;
        private int _port;
        private string _password;

        private WebSocket _client;

        /// <summary>
        /// Called when a new console message is received from the server.
        /// </summary>
        public event Action<ConsoleMsg> OnConsoleMessage;

        /// <summary>
        /// Called when a new chat message is received from the server.
        /// </summary>
        public event Action<ChatMsg> OnChatMessage;

        /// <summary>
        /// Called when the connection status changes.
        /// </summary>
        public event Action<bool> OnConnectionChanged;

        private List<BaseCommand> _commands;

        /// <summary>
        /// Initialise new remote connection to a specified server.
        /// </summary>
        /// <param name="host">IP, hostname or FQDN of the host</param>
        /// <param name="port">Query port, usualy game port + 1</param>
        /// <param name="password">RCON passphrase</param>
        public RconClient(string host, int port, string password)
        {
            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(password) || port < 1 || port > short.MaxValue)
                throw new ArgumentException("Not all arguments have been supplied.");

            _host = host;
            _port = port;
            _password = password;

            _client = new WebSocket($"ws://{host}:{port}/{password}");
            _client.OnClose += OnClose;
            _client.OnError += OnError;
            _client.OnMessage += OnMessage;
            _client.OnOpen += OnOpen;

            _commands = new List<BaseCommand>();
        }

        public void Connect()
        {
            try
            {
                _client.Connect();
            }
            catch (Exception ex)
            { }
        }

        public void SendCommand(BaseCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            string json = JsonConvert.SerializeObject(command);

            _client.Send(json);

            _commands.Add(command);
        }

        private void OnMessage(object sender, MessageEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data)) return;

            var response = JsonConvert.DeserializeObject<ServerResponse>(e.Data);

            if (response == null) return;

            var command = _commands.Find(x => x.ID == response.ID);

            if (command != null)
            {
                if (command.Completed) return;

                command.Complete(response);
                command.Dispose();
                _commands.Remove(command);
                return;
            }

            try
            {
                if (response.Content.StartsWith("[rcon]"))
                    return;

                switch (response.ID)
                {
                    case 0:
                        {
                            OnConsoleMessage?.Invoke(new ConsoleMsg(response.Content, response.Type.ToEnum<ConsoleMsg.MessageType>()));
                            break;
                        }
                    default:
                        if (response.Type == "Chat")
                        {
                            var msg = JsonConvert.DeserializeObject<ChatMsg>(response.Content);
                            OnChatMessage?.Invoke(msg);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OnOpen(object sender, EventArgs e)
        {
            OnConnectionChanged?.Invoke(true);
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            try
            {
                _client.Close();
                OnConnectionChanged?.Invoke(false);
            }
            catch
            {

            }
        }

        private void OnClose(object sender, CloseEventArgs e)
        {
            OnConnectionChanged?.Invoke(false);
        }

        public void Dispose()
        {
            if (IsConnected)
            {
                _client.Close();
            }

            OnChatMessage = null;
            OnConsoleMessage = null;
            OnConnectionChanged = null;

            _commands = null;
        }

        public bool IsConnected
        {
            get
            {
                return _client.IsAlive;
            }
        }
    }
}
