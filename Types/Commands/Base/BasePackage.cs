using Newtonsoft.Json;
using System;

namespace RustRcon.Types.Commands.Base
{
    public abstract class BasePackage
    {
        /// <summary>
        /// It is necessary to determine the type of package and its call
        /// </summary>
        [JsonProperty("Identifier")]
        public Int32 ID { get; private set; }

        /// <summary>
        /// Package Contents
        /// </summary>

        [JsonProperty("Message")]
        public string Content { get; }

        private static Int32 id_counter = 2;

        /// <summary>
        /// A package constructor with the ability to set a custom ID, usually used for packages from the server
        /// </summary>
        /// <param name="id">Package ID</param>
        /// <param name="content">Package content</param>
        public BasePackage(int id, string content)
        {
            this.ID = id;
            this.Content = content;
        }

        /// <summary>
        /// Constructor for a package without the ability to set a custom ID, usually used for packages from the client
        /// </summary>
        /// <param name="content"></param>
        public BasePackage(string content)
        {
            this.ID = ++id_counter;
            this.Content = content;
        }
    }
}
