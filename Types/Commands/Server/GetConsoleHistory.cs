using Newtonsoft.Json;
using RustRcon.Types.Commands.Base;
using RustRcon.Types.Response;
using RustRcon.Types.Server.Messages;
using System;
using System.Collections.Generic;
using RustRcon.Pooling;

namespace RustRcon.Types.Commands.Server
{
    public class GetConsoleHistory : BaseCommand
    {
        private Action<List<ConsoleMsg>> _callback;

        /// <summary>
        /// Return server console messages history
        /// </summary>
        /// <param name="callback">Called when a response is received</param>
        /// <param name="messagesCount">Maximum number of messages</param>
        public static GetConsoleHistory Create(Action<List<ConsoleMsg>> callback, int messagesCount = 1024)
        {
            var command = CreatePackage<GetConsoleHistory>();
            command._callback = callback;
            command.Content = $"console.tail {messagesCount}";

            return command;
        }

        public override void Complete(ServerResponse response)
        {
            base.Complete(response);

            List<ConsoleMsg> messages = RustRconPool.GetList<ConsoleMsg>();

            try
            {
                messages = JsonConvert.DeserializeObject<List<ConsoleMsg>>(response.Content);
            }
            catch (Exception)
            {
                // ignored
            }

            _callback?.Invoke(messages);
            RustRconPool.FreeList(messages);
        }

        protected override void EnterPool()
        {
            base.EnterPool();

            _callback = null;
        }
    }
}