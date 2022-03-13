using RustRcon.Types.Commands.Base;
using RustRcon.Types.Response;
using System;

namespace RustRcon.Types.Commands.Server
{
    public class ConsoleCommand : BaseCommand
    {
        private event Action<ServerResponse> _callback;

        /// <summary>
        /// Run command on the server console
        /// </summary>
        /// <param name="message">Console command</param>
        /// <param name="callback">A response containing a message from the server</param>
        public ConsoleCommand(string message, Action<ServerResponse> callback = null) : base(message)
        {
            this._callback = callback;
        }

        public override void Complete(ServerResponse response)
        {
            base.Complete(response);

            _callback?.Invoke(response);
        }

        public override void Dispose()
        {
            _callback = null;
        }
    }
}
