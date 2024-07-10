#region

using RustRcon.Pooling;

#endregion

namespace RustRcon.Types.Server.Messages
{
    public class ConsoleMsg : BasePoolable
    {
        public string Message { get; set; }

        public MessageType Type { get; set; }

        public enum MessageType
        {
            Generic,
            Log,
            Error,
            Warning
        }

        protected override void EnterPool()
        {
            base.EnterPool();

            Message = string.Empty;
            Type = MessageType.Generic;
        }
    }
}