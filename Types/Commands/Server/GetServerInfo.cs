#region

using Newtonsoft.Json;
using RustRcon.Pooling;
using RustRcon.Types.Commands.Base;
using RustRcon.Types.Response.Server;

#endregion

namespace RustRcon.Types.Commands.Server
{
    public class GetServerInfo : BaseCommand<ServerInfo>
    {
        /// <summary>
        ///     Return server info
        /// </summary>
        /// <returns>ServerInfo</returns>
        public static GetServerInfo Create()
        {
            var command = CreatePackage<GetServerInfo>();
            command.Content = "serverinfo";

            return command;
        }

        public override void Complete(ServerResponse response)
        {
            base.Complete(response);

            try
            {
                Result = RustRconPool.Get<ServerInfo>();
                JsonConvert.PopulateObject(response.Content, Result);
            }
            catch
            {
                // ignored
            }
        }
    }
}