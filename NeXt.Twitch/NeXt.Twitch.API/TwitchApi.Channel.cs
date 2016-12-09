using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NeXt.Twitch.API.Data;

namespace NeXt.Twitch.API
{
    public partial class TwitchApi
    {
        /// <summary>
        /// Posts to a Twitch channel's feed.
        /// </summary>
        /// <param name="content">The content of the message being posted.</param>
        /// <param name="accessToken">OAuth access token with channel_feed_edit scope.</param>
        /// <param name="channel">Channel to post feed post to.</param>
        /// <param name="share">If set to true, and enabled on account, will tweet out post.</param>
        /// <returns>Returns object with Post object and URL to tweet if available.</returns>
        public async Task<PostToChannelFeedResponse> PostToChannelFeed(string content, bool share, string channel,
            string accessToken)
        {
            return
                new PostToChannelFeedResponse(
                    JObject.Parse(
                        await
                            MakeRestRequest($"https://api.twitch.tv/kraken/feed/{channel}/posts", "POST",
                                $"content={content}&share={(share ? "true" : "false")}", accessToken)));
        }

        /// <summary>
        /// Deletes a post on a Twitch channel's feed.
        /// </summary>
        /// <param name="postId">Integer Id of feed post to delete.</param>
        /// <param name="channel">Channel where the post resides.</param>
        /// <param name="accessToken">OAuth access token with channel_feed_edit scope.</param>
        public async void DeleteChannelFeedPost(string postId, string channel, string accessToken)
        {
            await MakeRestRequest($"https://api.twitch.tv/kraken/feed/{channel}/posts/{postId}", "DELETE", null, accessToken);
        }

        /// <summary>
        /// Fetches Twitch channel name from a steam Id, if their Steam is connected to their Twitch.
        /// </summary>
        /// <param name="steamId">The steam id of the user whose Twitch channel is requested.</param>
        /// <returns>Returns channel name if available, or null.</returns>
        public async Task<string> GetChannelFromSteamId(string steamId)
        {
            try
            {
                var resp = await MakeGetRequest($"https://api.twitch.tv/api/steam/{steamId}");
                return JObject.Parse(resp).SelectToken("name").ToString();
            } catch(Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a list of videos ordered by time of creation, starting with the most recent.
        /// </summary>
        /// <param name="channel">The channel to retrieve the list of videos from.</param>
        /// <param name="limit">Maximum number of objects in array. Default is 10. Maximum is 100.</param>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <param name="onlyBroadcasts">Returns only broadcasts when true. Otherwise only highlights are returned. Default is false.</param>
        /// <param name="onlyHls">Returns only HLS VoDs when true. Otherwise only non-HLS VoDs are returned. Default is false.</param>
        /// <returns>A list of TwitchVideo objects the channel has available.</returns>
        public async Task<IReadOnlyList<Video>> GetChannelVideos(string channel, int limit = 10,
            int offset = 0, bool onlyBroadcasts = false, bool onlyHls = false)
        {
            var args = $"?limit={limit}&offset={offset}&broadcasts={onlyBroadcasts}&hls={onlyHls}";
            var resp = await MakeGetRequest($"https://api.twitch.tv/kraken/channels/{channel}/videos{args}");
            return JObject
                .Parse(resp)
                .SelectToken("videos")
                .ToObject<IReadOnlyList<Video>>(serializer);
        }

        /// <summary>
        /// Retrieves the current status of the broadcaster.
        /// </summary>
        /// <param name="channel">The name of the broadcaster to check.</param>
        /// <returns>True if the broadcaster is online, false otherwise.</returns>
        public async Task<bool> BroadcasterOnline(string channel)
        {
            try
            {
                var resp = await MakeGetRequest($"https://api.twitch.tv/kraken/streams/{channel}");
                return resp.Contains("{\"stream\":{\"_id\":");
            }
            catch (InvalidCredentialException)
            {
                throw;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Retrieves whether a <paramref name="username"/> is subscribed to a <paramref name="channel"/>.
        /// <para>Authenticated, required scope: <code>channel_check_subscription</code></para>
        /// </summary>
        /// <param name="channel">The channel to check against.</param>
        /// <param name="username">The user to check subscription status for.</param>
        /// <param name="accessToken">An oauth token with the required scope.</param>
        public async Task<ChannelHasUserSubscribedResponse> ChannelHasUserSubscribed(string channel, string username, string accessToken)
        {
            return Deserialize<ChannelHasUserSubscribedResponse>(await MakeGetRequest($"https://api.twitch.tv/kraken/channels/{channel}/subscriptions/{username}", accessToken));
        }

        /// <summary>
        /// Returns the amount of subscribers <paramref name="channel"/> has.
        /// <para>Authenticated, required scope: <code>channel_subscriptions</code></para>
        /// </summary>
        /// <param name="channel">The channel to retrieve the subscriptions from.</param>
        /// <param name="accessToken">An oauth token with the required scope.</param>
        /// <returns>An integer of the total subscription count.</returns>
        public async Task<int> GetSubscriberCount(string channel, string accessToken)
        {
            var resp =
                await MakeGetRequest($"https://api.twitch.tv/kraken/channels/{channel}/subscriptions", accessToken);
            var json = JObject.Parse(resp);
            return int.Parse(json.SelectToken("_total").ToString());
        }

        /// <summary>
        /// Retrieves a list of followed users a specific user has.
        /// </summary>
        /// <param name="channel">Channel to fetch followed users</param>
        /// <param name="limit">Default is 25, max is 100, min is 0</param>
        /// <param name="offset">Integer representing list offset</param>
        /// <param name="sortKey">Enum representing sort order.</param>
        /// <returns>FollowedUsersResponse object.</returns>
        public async Task<FollowedUsersResponse> GetFollowedUsers(string channel, int limit = 25, int offset = 0, SortKey sortKey = SortKey.CreatedAt)
        {
            var args = "";
            args += "?limit=" + limit;
            args += "&offset=" + offset;
            switch (sortKey)
            {
                case SortKey.CreatedAt:
                    args += "&sortby=created_at";
                    break;
                case SortKey.LastBroadcaster:
                    args += "&sortby=last_broadcast";
                    break;
                case SortKey.Login:
                    args += "&sortby=login";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sortKey), sortKey, null);
            }

            var resp = await MakeGetRequest($"https://api.twitch.tv/kraken/users/{channel}/follows/channels{args}");
            return Deserialize<FollowedUsersResponse>(resp);
        }

        /// <summary>
        /// Retrieves an ascending or descending list of followers from a specific channel.
        /// </summary>
        /// <param name="channel">The channel to retrieve the followers from.</param>
        /// <param name="limit">Maximum number of objects in array. Default is 25. Maximum is 100.</param>
        /// <param name="cursor">Twitch uses cursoring to paginate long lists of followers. Check <code>_cursor</code> in response body and set <code>cursor</code> to this value to get the next page of results, or use <code>_links.next</code> to navigate to the next page of results.</param>
        /// <param name="direction">Creation date sorting direction.</param>
        /// <returns>A list of TwitchFollower objects.</returns>
        public async Task<FollowersResponse> GetTwitchFollowers(string channel, int limit = 25,
            string cursor = "-1", SortDirection direction = SortDirection.Descending)
        {
            var args = "";

            args += "?limit=" + limit;
            args += cursor != "-1" ? $"&cursor={cursor}" : "";
            args += "&direction=" + (direction == SortDirection.Descending ? "desc" : "asc");

            var resp = await MakeGetRequest($"https://api.twitch.tv/kraken/channels/{channel}/follows{args}");
            return Deserialize<FollowersResponse>(resp);
        }

        /// <summary>
        /// Retrieves a list of all people currently chatting in a channel's chat.
        /// </summary>
        /// <param name="channel">The channel to retrieve the chatting people for.</param>
        /// <returns>A list of Chatter objects detailing each chatter in a channel.</returns>
        public async Task<Chatters> GetChatters(string channel)
        {
            return Deserialize<Chatters>(await MakeGetRequest($"https://tmi.twitch.tv/group/user/{channel.ToLower()}/chatters"));
        }
    }
}