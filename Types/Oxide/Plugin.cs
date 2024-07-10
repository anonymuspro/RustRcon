namespace RustRcon.Types.Oxide
{
    public class Plugin
    {
        public Plugin(string name, bool loaded, string version, string author)
        {
            Name = name;
            Loaded = loaded;
            Version = version;
            Author = author;
        }

        public string Name { get; private set; }
        public bool Loaded { get; private set; }
        public string Version { get; private set; }
        public string Author { get; private set; }
    }
}