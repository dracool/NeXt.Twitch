using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace NeXt.Twitch.API.Data
{
    /// <summary>
    /// Class representing a response from Twitch API for ChannelUserHasSubscribed
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]//used by Json.Net
    [SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]//Instantiated by Json.Net
    public sealed class ChannelHasUserSubscribedResponse
    {
        [JsonConstructor]
        private ChannelHasUserSubscribedResponse() { }

        /// <summary>
        /// Property representing internal variable Id
        /// </summary>
        [JsonProperty("_id")]
        public string Id { get; private set; }

        /// <summary>
        /// Property representing a User object.
        /// </summary>
        [JsonProperty("user")]
        public User User { get; private set; }

        /// <summary>
        /// The time the subscription was created
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; private set; }
    }
}
