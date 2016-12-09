using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace NeXt.Twitch.API.Data
{
    /// <summary>
    /// Object representing a follow between a user/viewer and a channel/streamer.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]//used by Json.Net
    [SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]//Instantiated by Json.Net
    public sealed class Follow
    {
        [JsonConstructor]
        private Follow() { }

        /// <summary>
        /// DateTime object representing when a follow was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// Value representing whether or not the user receives notificaitons for their follow.
        /// </summary>
        [JsonProperty("notifications")]
        public bool Notifications { get; private set; }

        /// <summary>
        /// Channel details returned along with the request.
        /// </summary>
        [JsonProperty("channel")]
        public Channel Channel { get; private set; }
    }
}
