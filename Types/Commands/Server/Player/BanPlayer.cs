using RustRcon.Types.Commands.Base;

namespace RustRcon.Types.Commands.Server

{
    public class BanPlayer : BaseCommand
    {
        /// <summary>
        /// Blocks a player on the server
        /// </summary>
        /// <param name="steamId">User steam ID</param>
        /// <param name="reason">Reason of ban</param>
        public BanPlayer(string steamId, string reason = "") : base($"ban {steamId} {reason}")
        {
        }
    }
}
