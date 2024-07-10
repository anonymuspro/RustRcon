#region

using RustRcon.Pooling;
using RustRcon.Types.Response.Server;

#endregion

namespace RustRcon.Types.Commands.Base
{
    public abstract class BaseCommand<T> : BasePackage where T : BasePoolable, new()
    {
        public T? Result { get; protected set; }

        public ServerResponse? ServerResponse { get; protected set; }

        /// <summary>
        ///     Indicates whether the response has arrived
        /// </summary>
        public bool Completed { get; private set; }

        /// <summary>
        ///     Required for the server to accept the command
        /// </summary>
        public string Name => "WebRcon";

        /// <summary>
        ///     Gives an answer to the command and completes its call
        /// </summary>
        /// <param name="response">Server response</param>
        public virtual void Complete(ServerResponse response)
        {
            if (Completed) return;

            ServerResponse = response;
            Completed = true;
        }

        protected override void EnterPool()
        {
            base.EnterPool();

            ServerResponse?.Dispose();
            Result?.Dispose();
            Completed = false;
        }
    }
}