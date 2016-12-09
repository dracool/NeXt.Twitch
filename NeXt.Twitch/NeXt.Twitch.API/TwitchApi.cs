using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NeXt.Twitch.API
{
    /// <summary>
    /// Classes to interact with the Twitch REST API
    /// </summary>
    public partial class TwitchApi
    {
        private readonly JsonSerializer serializer;

        /// <summary>
        /// Creates an instance of the twitch api
        /// </summary>
        /// <param name="clientId">the client id to use</param>
        /// <param name="disableIdValidation">set to true to skip validation of the client id on creation</param>
        /// <exception cref="ArgumentNullException">if parameter is null</exception>
        /// <exception cref="ArgumentException">if parameter is empty or whitespace</exception>
        public TwitchApi(string clientId, bool disableIdValidation = false)
        {
            if (clientId == null)
                throw new ArgumentNullException(nameof(clientId));
            if (string.IsNullOrWhiteSpace(clientId))
                throw new ArgumentException("Value cannot be empty or whitespace.", nameof(clientId));
            ClientId = clientId;
            if (!disableIdValidation)
                ValidateClientId();

            serializer = JsonSerializer.CreateDefault();
        }

        /// <summary>
        /// The client Id used for all api requests
        /// </summary>
        public string ClientId { get; }

        /// <summary>
        /// Validates the set ClientId
        /// </summary>
        /// <exception cref="InvalidApiCredentialException">if the ClientId is not valid</exception>
        public async void ValidateClientId()
        {
            var resp = await MakeGetRequest("https://api.twitch.tv/kraken");
            var json = JObject.Parse(resp);
            if ((json.SelectToken("identified") != null) && (bool)json.SelectToken("identified")) return;
            throw new InvalidApiCredentialException("The provided Client-Id is invalid. Create an application here and obtain a Client-Id from it here: https://www.twitch.tv/settings/connections");
        }

        private T Deserialize<T>(string json)
        {
            using (var jReader = new JsonTextReader(new StringReader(json)))
            {
                return serializer.Deserialize<T>(jReader);
            }
        }

        private async Task<string> MakeGetRequest(string url, string accessToken = null)
        {
            if (string.IsNullOrEmpty(ClientId) && string.IsNullOrWhiteSpace(accessToken))
                throw new InvalidChannelException("All API calls require Client-Id");

            accessToken = accessToken?.ToLower().Replace("oauth:", "");

            // If the URL already has GET parameters, we cannot use the GET parameter initializer '?'
            var request = url.Contains("?")
                ? (HttpWebRequest)WebRequest.Create(new Uri($"{url}&client_id={ClientId}"))
                : (HttpWebRequest)WebRequest.Create(new Uri($"{url}?client_id={ClientId}"));
            request.Method = "GET";
            request.Accept = "application/vnd.twitchtv.v3+json";
            request.ContentType = "application/json";
            request.Headers.Add("Client-ID", ClientId);

            if (!string.IsNullOrWhiteSpace(accessToken))
                request.Headers.Add("Authorization", $"OAuth {accessToken}");

            using (var responseStream = await request.GetResponseAsync())
            {
                return await new StreamReader(responseStream.GetResponseStream(), Encoding.Default, true).ReadToEndAsync();
            }
        }

        private async Task<string> MakeRestRequest(string url, string method, string requestData = null,
            string accessToken = null)
        {
            if (string.IsNullOrWhiteSpace(ClientId) && string.IsNullOrWhiteSpace(accessToken))
                throw new InvalidChannelException("All API calls require Client-Id or OAuth token.");

            var data = new UTF8Encoding().GetBytes(requestData ?? "");
            accessToken = accessToken?.ToLower().Replace("oauth:", "");

            var request = (HttpWebRequest)WebRequest.Create(new Uri($"{url}?client_id={ClientId}"));
            request.Method = method;
            request.Accept = "application/vnd.twitchtv.v3+json";
            request.ContentType = method == "POST"
                ? "application/x-www-form-urlencoded"
                : "application/json";
            request.Headers.Add("Client-ID", ClientId);

            if (!string.IsNullOrWhiteSpace(accessToken))
                request.Headers.Add("Authorization", $"OAuth {accessToken}");

            using (var requestStream = await request.GetRequestStreamAsync())
            {
                await requestStream.WriteAsync(data, 0, data.Length);
            }

            using (var responseStream = await request.GetResponseAsync())
            {
                return await new StreamReader(responseStream.GetResponseStream(), Encoding.Default, true).ReadToEndAsync();
            }
        }
    }
}
