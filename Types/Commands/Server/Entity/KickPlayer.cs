#region

using RustRcon.Types.Commands.Base;
using RustRcon.Types.Response.Server;

#endregion

namespace RustRcon.Types.Commands.Server.Entity
{
    public class KickPlayer : BaseCommand<ServerResponse>
    {
        /// <summary>
        ///     Kick player from server
        /// </summary>
        /// <param name="steamId">Player SteamID</param>
        /// <param name="reason">Reason of kick</param>
        public static KickPlayer Create(string steamId, string reason = "")
        {
            var command = CreatePackage<KickPlayer>();
            command.Content = $"kick {steamId} {reason}";

            return command;
        }

        public override void Complete(ServerResponse response)
        {
            base.Complete(response);
            Result = response;
        }
    }
}