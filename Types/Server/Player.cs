namespace RustRcon.Types.Server
{
    public class Player
    {
        public string SteamID { get; set; }
        public string OwnerSteamID { get; set; }
        public string DisplayName { get; set; }
        public int Ping { get; set; }
        public string Address { get; set; }
        public int ConnectedSeconds { get; set; }
        public double VoiationLevel { get; set; }
        public double CurrentLevel { get; set; }
        public double UnspentXp { get; set; }
        public double Health { get; set; }
    }
}