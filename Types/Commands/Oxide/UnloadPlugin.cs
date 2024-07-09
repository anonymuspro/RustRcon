using RustRcon.Types.Commands.Base;

namespace RustRcon.Types.Commands.Oxide
{
    public class UnloadPlugin : BaseCommand
    {
        public static UnloadPlugin Create(string pluginName)
        {
            var command = CreatePackage<UnloadPlugin>();
            command.Content = $"o.load {pluginName}";

            return command;
        }
    }
}
