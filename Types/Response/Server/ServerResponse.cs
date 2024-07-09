using Newtonsoft.Json;
using RustRcon.Pooling;
using RustRcon.Types.Commands.Base;

namespace RustRcon.Types.Response.Server
{
    public class ServerResponse : BasePackage
    {
        [JsonProperty("Type")]
        public string Type { get; private set; }

        [JsonProperty("Stacktrace")]
        public string Stacktrace { get; private set; }

        public static ServerResponse Create(string type, string stackstrace, string content, int id)
        {
            var package = RustRconPool.Get<ServerResponse>();

            package.Id = id;
            package.Content = content;
            package.Type = type;
            package.Stacktrace = stackstrace;

            return package;
        }
    }
}