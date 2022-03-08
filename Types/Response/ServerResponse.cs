using Newtonsoft.Json;
using RustRcon.Types.Commands.Base;

namespace RustRcon.Types.Response
{
    public class ServerResponse : BasePackage
    {
        [JsonProperty("Type")]
        public string Type { get; }

        [JsonProperty("Stacktrace")]
        public string Stacktrace { get; }

        public ServerResponse(string type, string stackstrace, string content, int id) : base(id, content)
        {
            this.Type = type;
            this.Stacktrace = stackstrace;
        }
    }
}
