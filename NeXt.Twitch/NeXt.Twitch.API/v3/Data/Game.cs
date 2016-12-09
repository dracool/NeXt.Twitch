using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace NeXt.Twitch.API.Data
{
    /// <summary>
    /// Class representing Game object returned from Twitch API.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    public sealed class Game
    {
        [JsonConstructor]
        private Game() { }

        /// <summary>
        /// Name of returned game.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// Popularity of returned game, if any
        /// </summary>
        [JsonProperty("popularity")]
        public int? Popularity { get; private set; }

        /// <summary>
        /// Twitch ID of returned game, if any
        /// </summary>
        [JsonProperty("_id")]
        public int? Id { get; private set; }

        /// <summary>
        /// GiantBomb ID of returned game, if any
        /// </summary>
        [JsonProperty("giantbomb_id")]
        public int? GiantBombId { get; private set; }

        /// <summary>
        /// Box class representing Box image URLs
        /// </summary>
        [JsonProperty("box")]
        public SizedImageUrls Box { get; private set; }

        /// <summary>
        /// Logo class representing Logo image URLs
        /// </summary>
        [JsonProperty("logo")]
        public SizedImageUrls Logo { get; private set; }
    }
}
