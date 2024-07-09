using RustRcon.Types.Commands.Base;
using RustRcon.Types.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RustRcon.Pooling;

namespace RustRcon.Types.Commands.Server
{
    public class GetBanList : BaseCommand
    {
        private Action<List<BannedPlayer>> _callback;

        /// <summary>
        /// Get players ban list command
        /// </summary>
        /// <param name="callback">A response containing a list of locks is called after receiving a response from the server</param>
        public static GetBanList Create(Action<List<BannedPlayer>> callback)
        {
            var command = CreatePackage<GetBanList>();
            command._callback = callback;
            command.Content = "banlistex";

            return command;
        }

        public override void Complete(ServerResponse response)
        {
            base.Complete(response);

            List<BannedPlayer> bannedPlayers = RustRconPool.GetList<BannedPlayer>();

            try
            {
                if (string.IsNullOrEmpty(response.Content) == false)
                {
                    List<string> rows = response.Content.Split('\n').ToList();

                    foreach (var row in rows)
                    {
                        Match match = Regex.Match(row, "[0-9]+\\s([0-9]+)\\s\"(.*?)\"\\s\"(.*?)\"\\s(.*)");

                        if (!match.Success)
                            continue;

                        var bannedPlayer = new BannedPlayer()
                        {
                            SteamID = match.Groups[1].Value,
                            DisplayName = match.Groups[2].Value,
                            Reason = match.Groups[3].Value
                        };

                        bannedPlayers.Add(bannedPlayer);
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            _callback?.Invoke(bannedPlayers);
            RustRconPool.FreeList(bannedPlayers);
        }

        protected override void EnterPool()
        {
            base.EnterPool();

            _callback = null;
        }
    }
}