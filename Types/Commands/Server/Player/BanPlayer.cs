using RustRcon.Types.Commands.Base;

namespace RustRcon.Types.Commands.Server.Player
{
    public class BanPlayer : BaseCommand
    {
        /// <summary>
        /// Blocks a player on the server
        /// </summary>
        /// <param name="steamId">User steam ID</param>
        /// <param name="reason">Reason of ban</param>   
        public static BanPlayer Create(string steamId, string reason = "")
        {
            var command = CreatePackage<BanPlayer>();
            command.Content = $"ban {steamId} {reason}";

            return command;
        }
    }
}