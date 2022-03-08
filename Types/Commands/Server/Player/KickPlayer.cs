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
        public KickPlayer(string steamId, string reason = "") : base($"kick {steamId} {reason}")
        {
        }
    }
}
