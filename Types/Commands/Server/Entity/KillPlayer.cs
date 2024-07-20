#region

using RustRcon.Types.Commands.Base;
using RustRcon.Types.Response.Server;

#endregion

namespace RustRcon.Types.Commands.Server.Entity
{
    public class KillPlayer : BaseCommand
    {
        /// <summary>
        ///     Kill player character
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