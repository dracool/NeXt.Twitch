using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace NeXt.Twitch.API.Data
{
    /// <summary>
    /// Class representing a game by popularity listing received from Twitch Api.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    public sealed class GameByPopularityListing
    {
        [JsonConstructor]
        private GameByPopularityListing() { }

        /// <summary>
        /// Property representing the Game object.
        /// </summary>
        [JsonProperty("game")]
        public Game Game { get; private set; }

        /// <summary>
        /// Property representing the number of viewers the game currently has.
        /// </summary>
        [JsonProperty("viewers")]
        public int? Viewers { get; private set; }

        /// <summary>
        /// Property representing the number of channels currently broadcasting the game.
        /// </summary>
        [JsonProperty("channels")]
        public int? Channels { get; private set; }
    }
}
