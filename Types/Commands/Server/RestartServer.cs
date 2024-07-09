using RustRcon.Types.Commands.Base;

namespace RustRcon.Types.Commands.Server
{
    public class RestartServer : BaseCommand
    {
        /// <summary>
        /// Restart server 
        /// </summary>
        /// <param name="seconds">time in seconds to restart</param>
        public static RestartServer Create(int seconds = 1)
        {
            var command = CreatePackage<RestartServer>();
            command.Content = $"restart {seconds}";

            return command;
        }
    }
}