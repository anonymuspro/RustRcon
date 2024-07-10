#region

using RustRcon.Types.Commands.Base;
using RustRcon.Types.Response.Server;

#endregion

namespace RustRcon.Types.Commands.Oxide
{
    public class ReloadPlugin : BaseCommand<ServerResponse>
    {
        public static ReloadPlugin Create(string pluginName)
        {
            var command = CreatePackage<ReloadPlugin>();
            command.Content = $"o.load {pluginName}";

            return command;
        }

        public override void Complete(ServerResponse response)
        {
            base.Complete(response);
            Result = response;
        }
    }
}