#region

using RustRcon.Types.Commands.Base;
using RustRcon.Types.Response.Server;

#endregion

namespace RustRcon.Types.Commands.Server
{
    public class ConsoleCommand : BaseCommand<ServerResponse>
    {
        /// <summary>
        ///     Run command on the server console
        /// </summary>
        /// <param name="message">Console command</param>
        /// <returns></returns>
        public static ConsoleCommand Create(string message)
        {
            var command = CreatePackage<ConsoleCommand>();
            command.Content = message;

            return command;
        }

        public override void Complete(ServerResponse response)
        {
            base.Complete(response);
            Result = response;
        }
    }
}