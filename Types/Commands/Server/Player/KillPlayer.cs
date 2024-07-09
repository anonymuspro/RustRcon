using RustRcon.Types.Commands.Base;

namespace RustRcon.Types.Commands.Server
{
    public class KillPlayer : BaseCommand
    {
        /// <summary>
        /// Kill player character
        /// </summary>
        /// <param name="steamId">Player SteamID</param>
        public static KickPlayer Create(string steamId)
        {
            var command = CreatePackage<KickPlayer>();
            command.Content = $"killplayer {steamId}";

            return command;
        }
    }
}
