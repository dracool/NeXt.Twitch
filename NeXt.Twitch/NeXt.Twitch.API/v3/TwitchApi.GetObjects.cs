using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NeXt.Twitch.API.Data;

namespace NeXt.Twitch.API
{
    public partial class TwitchApi
    {
        /// <summary>
        /// Retrieves a Channels object regarding a specific channel.
        /// <para>required scope: none</para>
        /// </summary>
        /// <param name="channel">The channel to fetch Channels object about.</param>
        /// <returns>Channels object.</returns>
        public async Task<Channels> GetChannelsObject(string channel)
        {
            return Deserialize<Channels>(await MakeGetRequest($"https://api.twitch.tv/api/channels/{channel}"));
        }

        /// <summary>
        /// Retrieves a channel's list of available chat badges.
        /// <para>required scope: none</para>
        /// </summary>
        /// <param name="channel">The channel to fetch available badges from.</param>
        /// <returns>Available badges.</returns>
        public async Task<IReadOnlyDictionary<string, Badge>> GetChannelBadges(string channel)
        {
            var dict = JObject
                .Parse(await MakeGetRequest($"https://api.twitch.tv/kraken/chat/{channel}/badges"))
                .Properties()
                .Where(p => p.Name != "_links")
                .ToDictionary(
                    p => p.Name,
                    p => p.Value.ToObject<Badge>(serializer)
                );
            return new ReadOnlyDictionary<string, Badge>(dict);
        }

        /// <summary>
        /// Retrieves a string list of channel editor users.
        /// <para>required scope: <code>channel_read</code></para>
        /// </summary>
        /// <param name="channel">The channel to fetch editors from.</param>
        /// <param name="accessToken">An access token with the required scope.</param>
        /// <returns>A list of User objects that are channel editors.</returns>
        public async Task<List<User>> GetChannelEditors(string channel, string accessToken)
        {
            var json =
                JObject.Parse(
                    await MakeGetRequest($"https://api.twitch.tv/kraken/channels/{channel}/editors", accessToken));
            return json.SelectToken("users").Select(editor => Deserialize<User>(editor.ToString())).ToList();
        }

        /// <summary>
        /// Retrieves a string list of channels hosting a specified channel.
        /// <para>required scope: none</para>
        /// <para>Note: This uses an undocumented API endpoint and reliability is not guaranteed. Additionally, this makes 2 API calls so limited use is recommended.</para>
        /// </summary>
        /// <param name="channel">The name of the channel to search for.</param>
        /// <returns>A list of all channels that are currently hosting the specified channel.</returns>
        public async Task<List<string>> GetChannelHosts(string channel)
        {
            var hosts = new List<string>();
            var resp = await MakeGetRequest($"https://api.twitch.tv/kraken/users/{channel}");
            var json = JObject.Parse(resp);
            if (json.SelectToken("_id") == null) return hosts;
            resp = await MakeGetRequest($"http://tmi.twitch.tv/hosts?include_logins=1&target={json.SelectToken("_id")}");
            json = JObject.Parse(resp);
            hosts.AddRange(json.SelectToken("hosts").Select(host => host.SelectToken("host_login").ToString()));
            return hosts;
        }

        /// <summary>
        /// Retrieves a TwitchTeamMember list of all members in a Twitch team.
        /// <para>required scope: none</para>
        /// <para>Note: This uses an undocumented API endpoint and reliability is not guaranteed.</para>
        /// </summary>
        /// <param name="teamName">The name of the Twitch team to search for.</param>
        /// <returns>A TwitchTeamMember list of all members in a Twitch team.</returns>
        public async Task<IReadOnlyList<TeamMember>> GetTeamMembers(string teamName)
        {
            var resp = await MakeGetRequest($"http://api.twitch.tv/api/team/{teamName}/all_channels.json");
            return JObject
                .Parse(resp)
                .SelectToken("channels")
                .ToObject<IReadOnlyList<TeamMember>>(serializer);
        }

        /// <summary>
        /// Retrieves a TwitchStream object containing API data related to a stream.
        /// <para>required scope: none</para>
        /// </summary>
        /// <param name="channel">The name of the channel to search for.</param>
        /// <returns>A TwitchStream object containing API data related to a stream.</returns>
        public async Task<Channel> GetTwitchChannel(string channel)
        {
            var resp = "";
            try
            {
                resp = await MakeGetRequest($"https://api.twitch.tv/kraken/channels/{channel}");
            }
            catch
            {
                throw new InvalidChannelException(resp);
            }
            var json = JObject.Parse(resp);
            if (json.SelectToken("error") != null) throw new InvalidChannelException(resp);
            return Deserialize<Channel>(resp);
        }

        /// <summary>
        /// Retrieves the current uptime of a stream, if it is online.
        /// <para>required scope: none</para>
        /// </summary>
        /// <param name="channel">The channel to retrieve the uptime for.</param>
        /// <returns>A TimeSpan object representing time between creation_at of stream, and now.</returns>
        public async Task<TimeSpan> GetUptime(string channel)
        {
            var stream = await GetTwitchStream(channel);
            if (stream == null)
                return TimeSpan.Zero;
            var time = Convert.ToDateTime(stream.CreatedAt);
            return DateTime.UtcNow - time;
        }

        /// <summary>
        /// Retrieves channel feed posts.
        /// <para>required scope: none</para>
        /// </summary>
        /// <param name="channel">Channel to fetch feed posts from.</param>
        /// <param name="limit">Applied limit (default 10, max 100)</param>
        /// <param name="cursor">Used for pagination.</param>
        /// <returns></returns>
        public async Task<FeedResponse> GetChannelFeed(string channel, int limit = 10, string cursor = null)
        {
            var args = $"?limit={limit}";
            if (cursor != null)
                args += $"&cursor={cursor};";
            var resp = await MakeGetRequest($"https://api.twitch.tv/kraken/feed/{channel}/posts{args}");
            return new FeedResponse(JObject.Parse(resp));
        }

        /// <summary>
        /// Retrieves a collection of API data from a stream.
        /// <para>required scope: none</para>
        /// </summary>
        /// <param name="channel">The channel to retrieve the data for.</param>
        /// <returns>A TwitchStream object containing API data related to a stream.</returns>
        public async Task<TwitchStream> GetTwitchStream(string channel)
        {
            try
            {
                var resp = await MakeGetRequest($"https://api.twitch.tv/kraken/streams/{channel}");
                var json = JObject.Parse(resp);
                return json
                    .SelectToken("stream")
                    .ToObject<TwitchStream>(serializer);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves a collection of API data from multiple streams
        /// <para>required scope: none</para>
        /// </summary>
        /// <param name="channels">List of channels.</param>
        /// <returns>A list of stream objects for each stream.</returns>
        public async Task<IReadOnlyList<TwitchStream>> GetTwitchStreams(List<string> channels)
        {
            try
            {
                var resp =
                    await MakeGetRequest($"https://api.twitch.tv/kraken/streams?channel={string.Join(",", channels)}");
                var json = JObject.Parse(resp);
                return json
                    .SelectToken("streams")
                    .ToObject<IReadOnlyList<TwitchStream>>(serializer);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves all featured streams.
        /// <para>required scope: none</para>
        /// </summary>
        /// <returns>A list of featured stream objects for each featured stream.</returns>
        public async Task<IReadOnlyList<FeaturedStream>> GetFeaturedStreams(int limit = 25, int offset = 0)
        {
            try
            {
                var resp =
                    await MakeGetRequest($"https://api.twitch.tv/kraken/streams/featured?limit={limit}&offset={offset}");
                var json = JObject.Parse(resp);
                return json
                    .SelectToken("featured")
                    .ToObject<IReadOnlyList<FeaturedStream>>(serializer);
            }
            catch
            {
                return null;
            }
        }
    }
}