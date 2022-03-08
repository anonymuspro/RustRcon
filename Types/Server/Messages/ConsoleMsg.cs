namespace RustRcon.Types.Server.Messages
{
    public class ConsoleMsg
    {
        public ConsoleMsg(string message, MessageType type)
        {
            Message = message;
            Type = type;
        }

        public string Message { get; private set; }

        public MessageType Type { get; private set; }
    
        public enum MessageType
        {
            Generic,
            Log,
            Error,
            Warning
        }
    }

}
