using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NeXt.Twitch.API.Data;

namespace NeXt.Twitch.API
{
    public partial class TwitchApi
    {
        /// <summary>
        /// Execute a search query on Twitch to find a list of channels.
        /// </summary>
        /// <param name="query">A url-encoded search query.</param>
        /// <param name="limit">Maximum number of objects in array. Default is 25. Maximum is 100.</param>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <returns>A list of Channel objects matching the query.</returns>
        public async Task<IReadOnlyList<Channel>> SearchChannels(string query, int limit = 25, int offset = 0)
        {
            var args = $"?query={query}&limit={limit}&offset={offset}";
            var resp = await MakeGetRequest($"https://api.twitch.tv/kraken/search/channels{args}");

            return JObject
                .Parse(resp)
                .SelectToken("channels")
                .ToObject<IReadOnlyList<Channel>>(serializer);
        }

        /// <summary>
        /// Execute a search query on Twitch to find a list of streams.
        /// </summary>
        /// <param name="query">A url-encoded search query.</param>
        /// <param name="limit">Maximum number of objects in array. Default is 25. Maximum is 100.</param>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <param name="hls">If set to true, only returns streams using HLS, if set to false only returns non-HLS streams. Default is null.</param>
        /// <returns>A list of Stream objects matching the query.</returns>
        public async Task<IReadOnlyList<TwitchStream>> SearchStreams(string query, int limit = 25, int offset = 0,
            bool? hls = null)
        {
            var hlsStr = "";
            if (hls == true) hlsStr = "&hls=true";
            if (hls == false) hlsStr = "&hls=false";
            var args = $"?query={query}&limit={limit}&offset={offset}{hlsStr}";
            var resp = await MakeGetRequest($"https://api.twitch.tv/kraken/search/streams{args}");

            return JObject
                .Parse(resp)
                .SelectToken("streams")
                .ToObject<IReadOnlyList<TwitchStream>>(serializer);
        }

        /// <summary>
        /// Execute a search query on Twitch to find a list of games.
        /// </summary>
        /// <param name="query">A url-encoded search query.</param>
        /// <param name="live">If set to true, only games with active streams will be found.</param>
        /// <returns>A list of Game objects matching the query.</returns>
        public async Task<IReadOnlyList<Game>> SearchGames(string query, bool live = false)
        {
            var returnedGames = new List<Game>();

            var args = $"?query={query}&type=suggest&live=" + live.ToString();
            var resp = await MakeGetRequest($"https://api.twitch.tv/kraken/search/games{args}");

            return JObject.Parse(resp).SelectToken("games").ToObject<IReadOnlyList<Game>>();   
        }

        /// <summary>
        /// Execute a query to return the games with the most current viewers.
        /// </summary>
        /// <param name="limit">The number of listings to return, default to 10.</param>
        /// <param name="offset">The number of listings to offset the returned listings, default to 0.</param>
        /// <returns>A list of Game objects matching the query.</returns>
        public async Task<IReadOnlyList<GameByPopularityListing>> GetGamesByPopularity(int limit = 10, int offset = 0)
        {
            var args = $"?limit={limit}&offset={offset}";
            var resp = await MakeGetRequest($"https://api.twitch.tv/kraken/games/top{args}");

            return JObject.Parse(resp).SelectToken("top").ToObject<IReadOnlyList<GameByPopularityListing>>();
        }
    }
}