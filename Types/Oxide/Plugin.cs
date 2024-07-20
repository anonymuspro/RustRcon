namespace RustRcon.Types.Oxide
{
    public struct Plugin
    {
        public Plugin(string filename, string name, string version, string author, string totalHookTime, string totalHookMemory)
        {
            Filename = filename;
            Name = name;
            Version = version;
            Author = author;
            TotalHookTime = totalHookTime;
            TotalHookMemory = totalHookMemory;
        }

        public string Filename { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public string TotalHookTime { get; set; }
        public string TotalHookMemory { get; set; }
    }
}