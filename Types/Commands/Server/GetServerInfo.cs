using Newtonsoft.Json;
using RustRcon.Types.Commands.Base;
using RustRcon.Types.Response;
using System;

namespace RustRcon.Types.Commands.Server
{
    public class GetServerInfo : BaseCommand
    {
        private Action<ServerInfo> _callback;   
        
        /// Return server info
        /// </summary>
        /// <param name="callback">Called when a response is received, contains an instance of the ServerInfo class</param>
        public static GetServerInfo Create(Action<ServerInfo> callback)
        {
            var command =  CreatePackage<GetServerInfo>();
            command._callback = callback;
            command.Content = "serverinfo";

            return command;
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

        protected override void EnterPool()
        {
            base.EnterPool();

            _callback = null;
        }
    }
}
