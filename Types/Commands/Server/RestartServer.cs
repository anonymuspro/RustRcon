using RustRcon.Types.Commands.Base;

namespace RustRcon.Types.Commands.Server
{
    public class RestartServer : BaseCommand
    {
        /// <summary>
        /// Restart server 
        /// </summary>
        /// <param name="seconds">time in seconds to restart</param>
        public RestartServer(int seconds = 1) : base($"restart {seconds}")
        {

        }
    }
}
