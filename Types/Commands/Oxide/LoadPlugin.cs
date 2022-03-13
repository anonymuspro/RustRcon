using RustRcon.Types.Commands.Base;


namespace RustRcon.Types.Commands.Oxide
{
    public class LoadPlugin : BaseCommand
    {
        public LoadPlugin(string pluginName) : base($"o.unload {pluginName}")
        {
        }

        public override void Dispose()
        {
            
        }
    }
}
