#region

using System;
using Newtonsoft.Json;
using RustRcon.Pooling;
using RustRcon.Types.Commands.Base;
using RustRcon.Types.Response.Server;
using RustRcon.Types.Server;

#endregion

namespace RustRcon.Types.Commands.Server.Entity
{
    public class GetPlayers : BaseCommand
    {
        public PoolableList<Player>? Result { get; private set; }

        /// <summary>
        ///     Get players command
        /// </summary>
        public static GetPlayers Create()
        {
            var command = CreatePackage<GetPlayers>();
            command.Content = "playerlist";

            return command;
        }

        public override void Complete(ServerResponse response)
        {
            base.Complete(response);

            try
            {
                Result = RustRconPool.Get<PoolableList<Player>>();
                JsonConvert.PopulateObject(response.Content, Result);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        protected override void EnterPool()
        {
            base.EnterPool();
            Result?.Dispose();
        }
    }
}