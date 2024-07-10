#region

using RustRcon.Types.Commands.Base;
using RustRcon.Types.Response.Server;

#endregion

namespace RustRcon.Types.Commands.Server.Entity
{
    public class BanPlayer : BaseCommand<ServerResponse>
    {
        /// <summary>
        ///     Blocks a player on the server
        /// </summary>
        /// <param name="steamId">User steam ID</param>
        /// <param name="reason">Reason of ban</param>
        public static BanPlayer Create(string steamId, string reason = "")
        {
            var command = CreatePackage<BanPlayer>();
            command.Content = $"ban {steamId} {reason}";

            return command;
        }

        public override void Complete(ServerResponse response)
        {
            base.Complete(response);
            Result = response;
        }
    }
}