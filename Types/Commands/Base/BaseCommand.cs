using RustRcon.Pooling;
using RustRcon.Types.Response;
using RustRcon.Types.Response.Server;

namespace RustRcon.Types.Commands.Base
{
    public abstract class BaseCommand : BasePackage
    {
        /// <summary>
        /// Indicates whether the response has arrived
        /// </summary>
        public bool Completed { get; private set; }

        /// <summary>
        /// Required for the server to accept the command
        /// </summary>
        public string Name => "WebRcon";

        /// <summary>
        /// Gives an answer to the command and completes its call
        /// </summary>
        /// <param name="response">Server response</param>
        public virtual void Complete(ServerResponse response)
        {
            if (Completed) return;

            Completed = true;
        }

        protected override void EnterPool()
        {
            base.EnterPool();

            Completed = false;
        }
    }
}