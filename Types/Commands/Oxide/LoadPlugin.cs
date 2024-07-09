using RustRcon.Types.Commands.Base;

namespace RustRcon.Types.Commands.Oxide
{
    public class LoadPlugin : BaseCommand
    {
        public static LoadPlugin Create(string pluginName)
        {
            var command = CreatePackage<LoadPlugin>();
            command.Content = $"o.load {pluginName}";

            return command;
        }
    }
}