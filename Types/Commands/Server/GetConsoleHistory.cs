#region

using System;
using Newtonsoft.Json;
using RustRcon.Pooling;
using RustRcon.Types.Commands.Base;
using RustRcon.Types.Response.Server;
using RustRcon.Types.Server.Messages;

#endregion

namespace RustRcon.Types.Commands.Server
{
    public class GetConsoleHistory : BaseCommand
    {
        public PoolableList<ConsoleMsg>? Result { get; private set; }

        /// <summary>
        ///     Return server console messages history
        /// </summary>
        /// <param name="messagesCount">Maximum number of messages</param>
        public static GetConsoleHistory Create(int messagesCount = 1024)
        {
            var command = CreatePackage<GetConsoleHistory>();
            command.Content = $"console.tail {messagesCount}";

            return command;
        }

        public override void Complete(ServerResponse response)
        {
            base.Complete(response);

            try
            {
                Result = RustRconPool.Get<PoolableList<ConsoleMsg>>();
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