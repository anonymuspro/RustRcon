using Newtonsoft.Json;
using RustRcon.Types.Commands.Base;
using RustRcon.Types.Response;
using RustRcon.Types.Server;
using System;
using System.Collections.Generic;
using RustRcon.Pooling;

namespace RustRcon.Types.Commands.Server
{
    public class GetPlayers : BaseCommand
    {
        private Action<List<Player>> _callback;

        /// <summary>
        /// Get players command
        /// </summary>
        /// <param name="callback">A response containing a list of players is called after receiving a response from the server</param>
        public static GetPlayers Create(Action<List<Player>> callback)
        {
            var command = CreatePackage<GetPlayers>();
            command._callback = callback;
            command.Content = "playerlist";

            return command;
        }

        public override void Complete(ServerResponse response)
        {
            base.Complete(response);

            List<Player> players = RustRconPool.GetList<Player>();

            try
            {
                players = JsonConvert.DeserializeObject<List<Player>>(response.Content);
            }
            catch (Exception)
            {
                // ignored
            }

            _callback?.Invoke(players);
            RustRconPool.FreeList(players);
        }

        protected override void EnterPool()
        {
            base.EnterPool();
            
            _callback = null;
        }
    }
}