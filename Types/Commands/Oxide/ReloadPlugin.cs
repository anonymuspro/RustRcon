using RustRcon.Types.Commands.Base;

namespace RustRcon.Types.Commands.Oxide
{
    public class ReloadPlugin : BaseCommand
    {
        public static ReloadPlugin Create(string pluginName)
        {
            var command = CreatePackage<ReloadPlugin>();
            command.Content = $"o.load {pluginName}";

            return command;
        }
    }
}
