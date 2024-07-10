#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RustRcon.Pooling;
using RustRcon.Types.Commands.Base;
using RustRcon.Types.Response.Server;
using RustRcon.Types.Server;

#endregion

namespace RustRcon.Types.Commands.Server.Entity
{
    public class GetBanList : BaseCommand<PoolableList<BannedPlayer>>
    {
        /// <summary>
        ///     Get players ban list command
        /// </summary>
        public static GetBanList Create()
        {
            var command = CreatePackage<GetBanList>();
            command.Content = "banlistex";

            return command;
        }

        public override void Complete(ServerResponse response)
        {
            base.Complete(response);

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

                        Result.Add(bannedPlayer);
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}