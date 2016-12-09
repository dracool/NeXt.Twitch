using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace NeXt.Twitch.API.Data
{
    /// <summary>
    /// Class representing game art images in various sizes.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    public sealed class SizedImageUrls
    {
        [JsonConstructor]
        private SizedImageUrls() { }

        /// <summary>
        /// Large game logo.
        /// </summary>
        [JsonProperty("large")]
        public string Large { get; private set; }

        /// <summary>
        /// Medium game logo.
        /// </summary>
        [JsonProperty("medium")]
        public string Medium { get; private set; }

        /// <summary>
        /// Small game logo.
        /// </summary>
        [JsonProperty("small")]
        public string Small { get; private set; }

        /// <summary>
        /// Template game logo.
        /// </summary>
        [JsonProperty("template")]
        public string Template { get; private set; }
    }
}