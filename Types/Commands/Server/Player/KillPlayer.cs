using RustRcon.Types.Commands.Base;

namespace RustRcon.Types.Commands.Server
{
    public class KillPlayer : BaseCommand
    {
        /// <summary>
        /// Kill player character
        /// </summary>
        /// <param name="steamID">Player SteamID</param>
        public KillPlayer(string steamID) : base($"killplayer {steamID}")
        {
        }
    }
}
