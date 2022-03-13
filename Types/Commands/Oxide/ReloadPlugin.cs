using RustRcon.Types.Commands.Base;

namespace RustRcon.Types.Commands.Oxide
{
    public class ReloadPlugin : BaseCommand
    {
        public ReloadPlugin(string pluginName) : base($"o.reload {pluginName}")
        {
        }

        public override void Dispose()
        {
            
        }
    }
}
