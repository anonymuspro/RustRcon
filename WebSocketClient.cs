#region

using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RustRcon.Types;
using RustRcon.Utility;

#endregion

namespace Services.Client
{
    public class WebSocketClient : IDisposable, ILogger
    {
        private readonly Uri _uri;
        private readonly ClientWebSocket _webSocket;
        private readonly SemaphoreSlim _sendSemaphore;
        private readonly SemaphoreSlim _receiveSemaphore;
        private readonly ILogger _logger;
        private readonly ReactiveProperty<WebSocketState> _state;

        private readonly Encoding _textEncoding;
        private readonly JsonSerializer _jsonSerializer;

        protected CancellationTokenSource? ConnectionCts { get; private set; }
        public readonly ReadOnlyReactiveProperty<WebSocketState> State;

        public WebSocketClient(Uri uri, ILogger? logger)
        {
            _uri = uri;
            _logger = logger ?? this;
            _webSocket = new ClientWebSocket();
            _sendSemaphore = new SemaphoreSlim(1);
            _receiveSemaphore = new SemaphoreSlim(1);
            _state = new ReactiveProperty<WebSocketState>();

            _textEncoding = new UTF8Encoding(false);
            _jsonSerializer = JsonSerializer.CreateDefault();

            State = new ReadOnlyReactiveProperty<WebSocketState>(_state);
        }

        public virtual async Task ConnectAsync(CancellationToken cancellation = default)
        {
            if (_webSocket.State == WebSocketState.Connecting || _webSocket.State == WebSocketState.Open)
                throw new InvalidOperationException("Client already connected.");

            ConnectionCts = new CancellationTokenSource();
            Task handleTask = new Task(() => HandleStateChanges(ConnectionCts.Token), ConnectionCts.Token);
            handleTask.Start();

            await _webSocket.ConnectAsync(_uri, cancellation);
        }

        public async Task DisconnectAsync(string reason, CancellationToken cancellation = default)
        {
            ConnectionCts?.Cancel();
            _state.Value = WebSocketState.Closed;

            if (!CanSendWebSocketMessage())
                return;

            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, reason, cancellation);
        }

        protected async Task SendAsync<T>(T command, CancellationToken cancellation)
        {
            if (_webSocket.State != WebSocketState.Open)
                throw new InvalidOperationException("Client is not connected.");

            await _sendSemaphore.WaitAsync(cancellation);

            if (!CanSendWebSocketMessage())
                return;

            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ConnectionCts!.Token, cancellation);

            try
            {
                await using MemoryStream stream = new MemoryStream();
                await using (StreamWriter streamWriter = new StreamWriter(stream, _textEncoding, 1024, true))

                using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter))
                {
                    _jsonSerializer.Serialize(jsonWriter, command);
                    await jsonWriter.FlushAsync(linkedCts.Token);
                }

                ArraySegment<byte> buffer = new ArraySegment<byte>(stream.GetBuffer(), 0, (int)stream.Length);
                await _webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, linkedCts.Token);
            }
            catch (WebSocketException ex) when (ex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
            {
                await DisconnectAsync(ex.ErrorCode.ToString(), CancellationToken.None);
                _logger.Log($"Error closing web socket: {ex.Message}");
            }
            finally
            {
                _sendSemaphore.Release();
            }
        }

        protected async Task<T?> ReadAsync<T>(CancellationToken cancellation = default) where T : class
        {
            await _receiveSemaphore.WaitAsync(cancellation);

            if (!CanSendWebSocketMessage())
                return null;

            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ConnectionCts!.Token, cancellation);

            try
            {
                await using MemoryStream stream = new MemoryStream();

                byte[] receiveBuffer = new byte[1024];
                ArraySegment<byte> buffer = new ArraySegment<byte>(receiveBuffer);

                WebSocketReceiveResult result;
                do
                {
                    linkedCts.Token.ThrowIfCancellationRequested();
                    result = await _webSocket.ReceiveAsync(buffer, linkedCts.Token);

                    switch (result.MessageType)
                    {
                        case WebSocketMessageType.Close:
                        {
                            _logger.Log($"Close message received: ${result.CloseStatusDescription}");
                            await DisconnectAsync(result.CloseStatusDescription, CancellationToken.None);
                            break;
                        }
                        case WebSocketMessageType.Text:
                        {
                            await stream.WriteAsync(buffer.Array, buffer.Offset, result.Count, linkedCts.Token);
                            break;
                        }
                    }

                    linkedCts.Token.ThrowIfCancellationRequested();
                } while (!linkedCts.Token.IsCancellationRequested && !result.EndOfMessage);

                stream.Seek(0, SeekOrigin.Begin);

                using StreamReader streamReader = new StreamReader(stream);
                using JsonTextReader reader = new JsonTextReader(streamReader);

                return _jsonSerializer.Deserialize<T>(reader);
            }
            catch (WebSocketException ex) when (ex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
            {
                await DisconnectAsync(ex.ErrorCode.ToString(), CancellationToken.None);
                _logger.Log($"Error closing web socket: {ex.Message}");
            }
            finally
            {
                _receiveSemaphore.Release();
            }

            return default;
        }

        private void HandleStateChanges(CancellationToken cancellation)
        {
            while (!cancellation.IsCancellationRequested)
            {
                WebSocketState state = _webSocket.State;

                if (state == _state.Value)
                {
                    Thread.Sleep(500);
                    continue;
                }

                _state.Value = _webSocket.State;
                _logger.Log($"Connection state changed: {_state.Value}");
            }
        }

        private bool CanSendWebSocketMessage()
        {
            return _webSocket.State != WebSocketState.Aborted && _webSocket.State != WebSocketState.Closed &&
                   _webSocket.State != WebSocketState.CloseSent;
        }

        public void Dispose()
        {
            _webSocket?.Dispose();
            _sendSemaphore?.Dispose();
            _receiveSemaphore?.Dispose();
        }

        public void Log(string message)
        {
            Console.WriteLine($"[WebSocketClient] {message}");
        }
    }
}