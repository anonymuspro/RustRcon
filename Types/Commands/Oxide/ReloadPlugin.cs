#region

using RustRcon.Types.Commands.Base;

#endregion

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