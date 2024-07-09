using RustRcon.Types.Commands.Base;

namespace RustRcon.Types.Commands.Server

{
    public class KickPlayer : BaseCommand
    {
        /// <summary>
        /// Kick player from server
        /// </summary>
        /// <param name="steamId">Player SteamID</param>
        /// <param name="reason">Reason of kick</param>
        public static KickPlayer Create(string steamId, string reason = "")
        {
            var command = CreatePackage<KickPlayer>();
            command.Content = $"kick {steamId} {reason}";

            return command;
        }
    }
}
