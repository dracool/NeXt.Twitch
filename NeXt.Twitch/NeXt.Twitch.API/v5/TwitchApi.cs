using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NeXt.Twitch.API.v5
{
    public class ResponseTask : Task<HttpWebResponse>
    {
        private HttpWebRequest Request { get; }
        private JsonSerializer Serializer { get; }
        public ResponseTask(HttpWebRequest request, JsonSerializer serializer) : base(() => (HttpWebResponse)request.GetResponse())
        {
            Request = request;
        }

        public async Task<string> AsText()
        {
            using (var resp = await this)
            using (var stream = resp.GetResponseStream())
            {
                return await new StreamReader(stream, Encoding.Default, true).ReadToEndAsync();
            }
        }

        public async Task<T> As<T>()
        {
            using (var resp = await this)
            using (var reader = new StreamReader(resp.GetResponseStream()))
            {
                return Serializer.Deserialize<T>(reader);
            }
        }
    }

    public class TwitchApi
    {
        private JsonSerializer serializer;

        public TwitchApi(string clientId, bool disableClientIdValidation = false)
        {
            if (clientId == null)
                throw new ArgumentNullException(nameof(clientId));
            if (string.IsNullOrWhiteSpace(clientId))
                throw new ArgumentException("Value cannot be empty or whitespace.", nameof(clientId));

            ClientId = clientId;

            if (!disableClientIdValidation)
                ValidateClientId();

            serializer = JsonSerializer.CreateDefault();

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
            HttpWebRequest request;
            request.GetResponse().GetResponseStream()
            throw new NotImplementedException();
        }
    }

}
