#region

using RustRcon.Types.Commands.Base;
using RustRcon.Types.Response.Server;

#endregion

namespace RustRcon.Types.Commands.Oxide
{
    public class UnloadPlugin : BaseCommand<ServerResponse>
    {
        public static UnloadPlugin Create(string pluginName)
        {
            var command = CreatePackage<UnloadPlugin>();
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