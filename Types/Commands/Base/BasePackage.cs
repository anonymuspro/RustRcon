#region

using System;
using Newtonsoft.Json;
using RustRcon.Pooling;

#endregion

namespace RustRcon.Types.Commands.Base
{
    public abstract class BasePackage : BasePoolable
    {
        private static Int32 _idCounter = 2;

        /// <summary>
        ///     It is necessary to determine the type of package and its call
        /// </summary>
        [JsonProperty("Identifier")]
        public Int32 Id { get; protected set; }

        /// <summary>
        ///     Package Contents
        /// </summary>
        [JsonProperty("Message")]
        public string Content { get; set; }

        protected static T CreatePackage<T>() where T : BasePackage, new()
        {
            return RustRconPool.Get<T>();
        }

        protected override void EnterPool()
        {
            base.EnterPool();

            Id = 0;
            Content = string.Empty;
        }

        protected override void LeavePool()
        {
            base.LeavePool();

            Id = ++_idCounter;
        }
    }
}