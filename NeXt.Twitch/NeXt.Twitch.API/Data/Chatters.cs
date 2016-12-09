using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace NeXt.Twitch.API.Data
{
    /// <summary>
    /// Contains a Chatters response returned by the Twitch API
    /// </summary>
    /// <seealso cref="TwitchApi"/>
    [JsonObject(MemberSerialization.OptIn)]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]//used by Json.Net
    [SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]//Instantiated by Json.Net
    public sealed class Chatters
    {
        [JsonConstructor]
        private Chatters() { }

        [JsonProperty("chatters")]
        private ChattersObject Data { get; set; }

        /// <summary>
        /// The total number of chatters in the channel
        /// </summary>
        [JsonProperty("chatter_count")]
        public int Count { get; private set; }

        /// <summary>
        /// The moderators in the channel.
        /// </summary>
        public IReadOnlyList<string> Moderators => Data?.Moderators;

        /// <summary>
        /// The staff members in the channel
        /// </summary>
        public IReadOnlyList<string> Staff => Data?.Staff;

        /// <summary>
        /// The admins in the channel
        /// </summary>
        public IReadOnlyList<string> Admins => Data?.Admins;

        /// <summary>
        /// The global moderators in the channel
        /// </summary>
        public IReadOnlyList<string> GlobalModerators => Data?.GlobalModerators;

        /// <summary>
        /// The viewers in the channel
        /// </summary>
        public IReadOnlyList<string> Viewers => Data?.Viewers;

        [JsonObject(MemberSerialization.OptIn)]
        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        private sealed class ChattersObject
        {
            [JsonConstructor]
            private ChattersObject() { }

            [JsonProperty("moderators")]
            public IReadOnlyList<string> Moderators { get; private set; }

            [JsonProperty("staff")]
            public IReadOnlyList<string> Staff { get; private set; }

            [JsonProperty("admins")]
            public IReadOnlyList<string> Admins { get; private set; }

            [JsonProperty("global_mods")]
            public IReadOnlyList<string> GlobalModerators { get; private set; }

            [JsonProperty("viewers")]
            public IReadOnlyList<string> Viewers { get; private set; }
        }
    }
}