using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeXt.Twitch.API.v5
{
    public class TwitchApi
    {
        private JsonSerializer serializer = JsonSerializer.CreateDefault();

        public TwitchApi(string clientId)
        {
            ClientId = clientId;
            Channels = new ChannelApi(this);
        }

        public string ClientId { get; }

        public IChannelApi Channels { get; }

        /// <summary>
        /// Deserializes a json string into an object of the specified type using this API instances serializes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        internal T Deserialize<T>(string json)
        {
            using (var jReader = new JsonTextReader(new StringReader(json)))
            {
                return serializer.Deserialize<T>(jReader);
            }
        }

        internal async Task<string> MakeGetRequest(string url, string parameters = null)
        {
            throw new NotImplementedException();
        }
    }

}
