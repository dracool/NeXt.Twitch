using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace NeXt.Twitch.API.Data
{
    /// <summary>
    /// Class representing a Badge as returned by the Twitch API
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]//used by Json.Net
    [SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]//Instantiated by Json.Net
    public sealed class Badge
    {
        [JsonConstructor]
        private Badge() { }

        /// <summary>
        /// Url to the alpha channel version of the badge
        /// </summary>
        [JsonProperty("alpha")]
        public string Alpha { get; private set; }

        /// <summary>
        /// Url to the default image of the badge
        /// </summary>
        [JsonProperty("image")]
        public string Image { get; private set; }

        /// <summary>
        /// Url to the vector based .svg version of the badge image
        /// </summary>
        [JsonProperty("svg")]
        public string Svg { get; private set; }
    }
}
