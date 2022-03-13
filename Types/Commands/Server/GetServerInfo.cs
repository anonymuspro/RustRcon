using Newtonsoft.Json;
using RustRcon.Types.Commands.Base;
using RustRcon.Types.Response;
using System;

namespace RustRcon.Types.Commands.Server
{
    public class GetServerInfo : BaseCommand
    {
        private Action<ServerInfo> _callback;

        /// <summary>
        /// Return server info
        /// </summary>
        /// <param name="callback">Called when a response is received, contains an instance of the ServerInfo class</param>
        public GetServerInfo(Action<ServerInfo> callback = null) : base("serverinfo")
        {
            _callback = callback;
        }

        public override void Complete(ServerResponse response)
        {
            base.Complete(response);

            try
            {
                var serverInfo = JsonConvert.DeserializeObject<ServerInfo>(response.Content);

                _callback?.Invoke(serverInfo);
            }
            catch
            {
                _callback?.Invoke(new ServerInfo());
            }
        }

        public override void Dispose()
        {
            _callback = null;
        }
    }
}
