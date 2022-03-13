using RustRcon.Types.Commands.Base;

namespace RustRcon.Types.Commands.Server
{
    public class StopServer : BaseCommand
    {
        /// <summary>
        /// Stop game server
        /// </summary>
        public StopServer() : base("server.stop")
        {

        }

        public override void Dispose()
        {
            
        }
    }
}
