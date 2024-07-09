namespace RustRcon.Types.Server
{
    public class BannedPlayer
    {
        public string SteamID { get; set; }
        public string DisplayName { get; set; }
        
        public string Reason { get; set; }
    }
}