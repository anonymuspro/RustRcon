#region

using RustRcon.Types.Commands.Base;

#endregion

namespace RustRcon.Types.Commands.Server
{
    public class StopServer : BaseCommand
    {
        /// <summary>
        ///     Stop game server
        /// </summary>
        public static StopServer Create()
        {
            var command = CreatePackage<StopServer>();
            command.Content = "server.stop";

            return command;
        }
    }
}