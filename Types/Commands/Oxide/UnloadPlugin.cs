using RustRcon.Types.Commands.Base;
using System;


namespace RustRcon.Types.Commands.Oxide
{
    public class UnloadPlugin : BaseCommand
    {
        public UnloadPlugin(string pluginName) : base($"o.unload {pluginName}")
        {
        }

        public override void Dispose()
        {
            
        }
    }
}
