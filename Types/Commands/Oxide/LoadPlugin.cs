#region

using RustRcon.Types.Commands.Base;
using RustRcon.Types.Response.Server;

#endregion

namespace RustRcon.Types.Commands.Oxide
{
    public class LoadPlugin : BaseCommand<ServerResponse>
    {
        public static LoadPlugin Create(string pluginName)
        {
            var command = CreatePackage<LoadPlugin>();
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