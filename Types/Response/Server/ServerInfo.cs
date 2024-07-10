#region

using RustRcon.Pooling;

#endregion

namespace RustRcon.Types.Response.Server
{
    public class ServerInfo : BasePoolable
    {
        public string Hostname { get; set; }
        public int MaxPlayers { get; set; }
        public int Players { get; set; }
        public int Queued { get; set; }
        public int Joining { get; set; }
        public int EntityCount { get; set; }
        public string GameTime { get; set; }
        public int Uptime { get; set; }
        public string Map { get; set; }
        public double Framerate { get; set; }
        public int Memory { get; set; }
        public int Collections { get; set; }
        public int NetworkIn { get; set; }
        public int NetworkOut { get; set; }
        public bool Restarting { get; set; }
        public string SaveCreatedTime { get; set; }

        protected override void EnterPool()
        {
            base.EnterPool();

            Hostname = string.Empty;
            MaxPlayers = 0;
            Players = 0;
            Queued = 0;
            Joining = 0;
            EntityCount = 0;
            GameTime = string.Empty;
            Uptime = 0;
            Map = string.Empty;
            Framerate = 0;
            Memory = 0;
            Collections = 0;
            NetworkIn = 0;
            NetworkOut = 0;
            Restarting = false;
            SaveCreatedTime = string.Empty;
        }
    }
}