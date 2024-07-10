#region

using RustRcon.Types.Commands.Base;
using RustRcon.Types.Response.Server;

#endregion

namespace RustRcon.Types.Commands.Server
{
    public class StopServer : BaseCommand<ServerResponse>
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

        public override void Complete(ServerResponse response)
        {
            base.Complete(response);
            Result = response;
        }
    }
}