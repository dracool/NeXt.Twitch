using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace NeXt.Twitch.API.Data
{
    /// <summary>
    /// Class representing a follower as fetched via Twitch API
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    public sealed class Follower
    {
        [JsonConstructor]
        private Follower() { }

        /// <summary>
        /// Property representing whether notifications are enabled or not.
        /// </summary>
        [JsonProperty("notifications")]
        public bool Notifications { get; private set; }

        /// <summary>
        /// Property representing date time of follow.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; private set; }

        /// <summary>
        /// Property representing the follower user.
        /// </summary>
        [JsonProperty("user")]
        public User User { get; private set; }
    }
}