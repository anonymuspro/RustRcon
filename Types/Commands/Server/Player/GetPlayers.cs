using Newtonsoft.Json;
using RustRcon.Types.Commands.Base;
using RustRcon.Types.Response;
using RustRcon.Types.Server;
using System;
using System.Collections.Generic;

namespace RustRcon.Types.Commands.Server
{
    public class GetPlayers : BaseCommand
    {
        private Action<List<Player>> _callback;

        /// <summary>
        /// Get players command
        /// </summary>
        /// <param name="callback">A response containing a list of players is called after receiving a response from the server</param>
        public GetPlayers(Action<List<Player>> callback) : base("playerlist")
        {
            _callback = callback;
        }

        public override void Complete(ServerResponse response)
        {
            base.Complete(response);

            List<Player> players = new List<Player>();

            try
            {
                players = JsonConvert.DeserializeObject<List<Player>>(response.Content);
            }
            catch
            {

            }

            _callback?.Invoke(players);

        }
    }
}
