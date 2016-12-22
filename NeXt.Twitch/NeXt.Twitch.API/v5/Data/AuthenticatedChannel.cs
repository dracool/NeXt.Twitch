using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeXt.Twitch.API.v5.Data
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AuthenticatedChannel : Channel
    {
        [JsonProperty("email")]
        public string Email { get; private set; }

        [JsonProperty("stream_key")]
        public string StreamKey { get; private set; }
    }
}
