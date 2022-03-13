using Newtonsoft.Json;
using RustRcon.Types.Commands.Base;
using RustRcon.Types.Response;
using RustRcon.Types.Server.Messages;
using System;
using System.Collections.Generic;

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
        public GetConsoleHistory(Action<List<ConsoleMsg>> callback = null, int messagesCount = 1024) : base($"console.tail {messagesCount}")
        {
            _callback = callback;
        }

        public override void Complete(ServerResponse response)
        {
            base.Complete(response);

            List<ConsoleMsg> messages = new List<ConsoleMsg>();

            try
            { 
                messages = JsonConvert.DeserializeObject<List<ConsoleMsg>>(response.Content); 
            }
            catch (Exception ex)
            {
                
            }

            _callback?.Invoke(messages);
        }

        public override void Dispose()
        {
            _callback = null;
        }
    }
}
