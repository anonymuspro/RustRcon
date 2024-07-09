using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RustRcon.Pooling;
using RustRcon.Types.Commands.Base;
using RustRcon.Types.Response.Server;

namespace RustRcon.Types.Commands.Server.Player
{
    public class GetPlayers : BaseCommand
    {
        private Action<List<Types.Server.Player>> _callback;

        /// <summary>
        /// Get players command
        /// </summary>
        /// <param name="callback">A response containing a list of players is called after receiving a response from the server</param>
        public static GetPlayers Create(Action<List<Types.Server.Player>> callback)
        {
            var command = CreatePackage<GetPlayers>();
            command._callback = callback;
            command.Content = "playerlist";

            return command;
        }

        public override void Complete(ServerResponse response)
        {
            base.Complete(response);

            List<Types.Server.Player> players = RustRconPool.GetList<Types.Server.Player>();

            try
            {
                players = JsonConvert.DeserializeObject<List<Types.Server.Player>>(response.Content);
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