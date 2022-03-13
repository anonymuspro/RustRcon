using RustRcon.Types.Response;
using System;

namespace RustRcon.Types.Commands.Base
{
    public abstract class BaseCommand : BasePackage, IDisposable
    {
        /// <summary>
        /// Indicates whether the response has arrived
        /// </summary>
        public bool Completed { get; private set; }

        /// <summary>
        /// Required for the server to accept the command
        /// </summary>
        public string Name { get { return "WebRcon";  } }

        /// <summary>
        /// Gives an answer to the command and completes its call
        /// </summary>
        /// <param name="response">Server response</param>
        public virtual void Complete(ServerResponse response)
        {
            if (Completed) return;

            Completed = true;
        }

        public abstract void Dispose();


        /// <summary>
        /// The base class of the server command.
        /// </summary>
        public BaseCommand(string message) : base(message)
        {

        }
    }
}
