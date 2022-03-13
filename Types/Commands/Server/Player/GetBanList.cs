using RustRcon.Types.Commands.Base;
using RustRcon.Types.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RustRcon.Types.Commands.Server
{
    public class BannedPlayer
    {
        public string SteamID { get; set; }
        public string DisplayName { get; set; }
        
        public string Reason { get; set; }
    }


    public class GetBanList : BaseCommand
    {
        private Action<List<BannedPlayer>> _callback;


        /// <summary>
        /// Get players ban list command
        /// </summary>
        /// <param name="callback">A response containing a list of locks is called after receiving a response from the server</param>
        public GetBanList(Action<List<BannedPlayer>> callback) : base("banlistex")
        {
            _callback = callback;
        }

        public override void Complete(ServerResponse response)
        {
            base.Complete(response);

            var bannedPlayes = new List<BannedPlayer>();

            try
            {
                if(string.IsNullOrEmpty(response.Content) == false)
                {
                    List<string> rows = response.Content.Split('\n').ToList();

                    foreach(var row in rows)
                    {
                        Match match = Regex.Match(row, "[0-9]+\\s([0-9]+)\\s\"(.*?)\"\\s\"(.*?)\"\\s(.*)");

                        if(match.Success)
                        {
                            var bannedPlayer = new BannedPlayer()
                            {
                                SteamID = match.Groups[1].Value,
                                DisplayName = match.Groups[2].Value,
                                Reason = match.Groups[3].Value
                            };

                            bannedPlayes.Add(bannedPlayer);
                        }
                    }
                }
            }
            catch
            {

            }

            _callback?.Invoke(bannedPlayes);
        }

        public override void Dispose()
        {
            _callback = null;
        }
    }
}
