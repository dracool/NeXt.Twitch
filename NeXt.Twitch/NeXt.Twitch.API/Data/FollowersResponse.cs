using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace NeXt.Twitch.API.Data
{
    /// <summary>
    /// Class representing response from Twitch API for followers.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    public sealed class FollowersResponse
    {
        [JsonConstructor]
        private FollowersResponse() { }

        /// <summary>
        /// Property representing list of Follower objects.
        /// </summary>
        [JsonProperty("follows")]
        public IReadOnlyList<Follower> Followers { get; private set; }

        /// <summary>
        /// Property representing total follower count.
        /// </summary>
        [JsonProperty("_total")]
        public int TotalFollowerCount { get; private set; }

        /// <summary>
        /// Property representing cursor for pagination.
        /// </summary>
        [JsonProperty("_cursor")]
        public string Cursor { get; private set; }
    }
    
}
