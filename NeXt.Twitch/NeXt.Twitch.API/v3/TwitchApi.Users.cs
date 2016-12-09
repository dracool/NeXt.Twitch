using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NeXt.Twitch.API.Data;

namespace NeXt.Twitch.API
{
    public partial class TwitchApi
    {
        /// <summary>
        /// Retrieves a list of blocked users a specific user has.
        /// <para>Authenticated, required scope: <code>user_blocks_read</code></para>
        /// </summary>
        /// <param name="username">Username of user to fetch blocked list of.</param>
        /// <param name="accessToken">This call requires an access token.</param>
        /// <param name="limit">Limit output from Twitch Api. Default 25, max 100.</param>
        /// <param name="offset">Offset out from Twitch Api. Default 0.</param>
        /// <returns>List of Block objects.</returns>
        public async Task<List<Block>> GetBlockedList(string username, string accessToken, int limit = 25,
            int offset = 0)
        {
            string args = $"?limit={limit}&offset={offset}";
            var resp = await MakeGetRequest($"https://api.twitch.tv/kraken/users/{username}/blocks{args}", accessToken);
            var json = JObject.Parse(resp);

            return (json.SelectToken("blocks") == null) 
                ? new List<Block>() 
                : json.SelectToken("blocks").Select(block => Deserialize<Block>(block.ToString())).ToList();
        }

        /// <summary>
        /// Blocks a user.
        /// <para>Authenticated, required scope: <code>user_blocks_edit</code></para>
        /// </summary>
        /// <param name="username">User who's blocked list to add to.</param>
        /// <param name="blockedUsername">User to block.</param>
        /// <param name="accessToken">This call requires an access token.</param>
        /// <returns>Block object.</returns>
        public async Task<Block> BlockUser(string username, string blockedUsername, string accessToken)
        {
            return Deserialize<Block>(
                await MakeRestRequest(
                    $"https://api.twitch.tv/kraken/users/{username}/blocks/{blockedUsername}",           
                    "PUT", 
                    "",
                    accessToken
                )
            );
        }

        /// <summary>
        /// Unblocks a user.
        /// <para>Authenticated, required scope: <code>user_blocks_edit</code></para>
        /// </summary>
        /// <param name="username">User who's blocked list to unblock from.</param>
        /// <param name="blockedUsername">User to unblock.</param>
        /// <param name="accessToken">This call requires an access token.</param>
        public async void UnblockUser(string username, string blockedUsername, string accessToken)
        {
            await
                MakeRestRequest($"https://api.twitch.tv/kraken/users/{username}/blocks/{blockedUsername}", "DELETE", "",
                    accessToken);
        }

        /// <summary>
        /// Retrieves a User object from Twitch Api and returns User object.
        /// <para>required scope: none</para>
        /// </summary>
        /// <param name="username">Name of the user you wish to fetch from Twitch.</param>
        /// <returns>User object containing details about the searched for user. Returns null if invalid user/error.</returns>
        public async Task<User> GetUser(string username)
        {
            try
            {
                var resp = await MakeGetRequest($"https://api.twitch.tv/kraken/users/{username}");
                var j = JObject.Parse(resp);
                if (j.SelectToken("error") != null)
                    return null;
                return Deserialize<User>(resp);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves whether a specified user is following the specified user.
        /// </summary>
        /// <param name="username">The user to check the follow status of.</param>
        /// <param name="channel">The channel to check against.</param>
        /// <returns>Returns Follow object representing follow relationship.</returns>
        public async Task<Follow> UserFollowsChannel(string username, string channel)
        {
            try
            {
                var resp = await MakeGetRequest($"https://api.twitch.tv/kraken/users/{username}/follows/channels/{channel}");
                return Deserialize<Follow>(resp);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Follows a channel given by <paramref name="channel"/>.
        /// <para>Authenticated, required scope: <code>user_follows_edit</code></para>
        /// </summary>
        /// <param name="username">The username of the user trying to follow the given channel.</param>
        /// <param name="channel">The channel to follow.</param>
        /// <param name="accessToken">An oauth token with the required scope.</param>
        /// <returns>A follow object representing the follow action.</returns>
        public async Task<Follow> FollowChannel(string username, string channel, string accessToken)
        {
            var resp = await MakeRestRequest($"https://api.twitch.tv/kraken/users/{username}/follows/channels/{channel}", "PUT", "", accessToken);
            return Deserialize<Follow>(resp);
        }

        /// <summary>
        /// Unfollows a channel given by <paramref name="channel"/>.
        /// <para>Authenticated, required scope: <code>user_follows_edit</code></para>
        /// </summary>
        /// <param name="username">The username of the user trying to follow the given channel.</param>
        /// <param name="channel">The channel to unfollow.</param>
        /// <param name="accessToken">An oauth token with the required scope.</param>
        public async void UnfollowChannel(string username, string channel, string accessToken)
        {
            await MakeRestRequest($"https://api.twitch.tv/kraken/users/{username}/follows/channels/{channel}", "DELETE", "", accessToken);
        }
    }
}