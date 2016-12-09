using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace NeXt.Twitch.API.Data
{
    /// <summary>
    /// Class representing a single featured stream from a Twitch API request.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]//used by Json.Net
    [SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]//Instantiated by Json.Net
    public sealed class FeaturedStream
    {
        [JsonConstructor]
        private FeaturedStream() { }

        /// <summary>
        /// Property representing a Html text description of the featured channel.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; private set; }

        /// <summary>
        /// Property representing whether or not the featured channel is sponsored.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("sponsored", DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool Sponsored { get; private set; }

        /// <summary>
        /// Property representing the priority of the featured stream
        /// </summary>
        [JsonProperty("priority")]
        public int? Priority { get; private set; }

        /// <summary>
        /// Property representing whether or not a stream is a scheduled feature.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("scheduled", DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool Scheduled { get; private set; }

        /// <summary>
        /// Property representing the url to the image shown in the tite on the home page.
        /// </summary>
        [JsonProperty("image")]
        public string Image { get; private set; }

        /// <summary>
        /// Property representing the stream object housing all stream details.
        /// </summary>
        [JsonProperty("stream")]
        public TwitchStream TwitchStream { get; private set; }
    }
}
