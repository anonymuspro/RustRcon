using RustRcon.Types.Commands.Base;
using RustRcon.Types.Response;
using System;

namespace RustRcon.Types.Commands.Server
{
    public class ConsoleCommand : BaseCommand
    {
        private Action<ServerResponse> _callback;

        /// Run command on the server console
        /// </summary>
        /// <param name="message">Console command</param>
        /// <param name="callback">A response containing a message from the server</param>
        public static ConsoleCommand Create(Action<ServerResponse> callback, string message)
        {
            var command = CreatePackage<ConsoleCommand>();
            command._callback = callback;
            command.Content = message;

            return command;
        }

        public override void Complete(ServerResponse response)
        {
            base.Complete(response);

            _callback?.Invoke(response);
        }

        protected override void EnterPool()
        {
            base.EnterPool();

            _callback = null;
        }
    }
}