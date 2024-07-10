#region

using RustRcon.Pooling;

#endregion

namespace RustRcon.Types.Server.Messages
{
    public class ChatMsg : BasePoolable
    {
        public int Channel { get; set; }
        public string Message { get; set; }
        public long UserId { get; set; }
        public string Username { get; set; }
        public string Color { get; set; }
        public long Time { get; set; }

        protected override void EnterPool()
        {
            Channel = 0;
            Message = string.Empty;
            UserId = 0;
            Username = string.Empty;
            Color = string.Empty;
            Time = 0;
        }
    }
}