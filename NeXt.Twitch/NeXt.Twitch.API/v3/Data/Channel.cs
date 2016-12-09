using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace NeXt.Twitch.API.Data
{
    /// <summary>
    /// Class representing a channel object from Twitch API.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]//used by Json.Net
    [SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]//Instantiated by Json.Net
    public sealed class Channel
    {
        [JsonConstructor]
        private Channel() { }

        /// <summary>
        /// Property representing whether channel is mature or not.
        /// </summary>
        [JsonProperty("mature")]
        public bool Mature { get; private set; }

        /// <summary>
        /// Property representing whether the channel is partnered or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the channel is partnered; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("partner")]
        public bool Partner { get; private set; }

        /// <summary>
        /// Property representing the number of followers the channel has.
        /// </summary>
        [JsonProperty("followers")]
        public int Followers { get; private set; }

        /// <summary>
        /// Property representing number of views channel has.
        /// </summary>
        [JsonProperty("views")]
        public int Views { get; private set; }

        /// <summary>
        /// Property representing channel Id.
        /// </summary>
        [JsonProperty("_id")]
        public long Id { get; private set; }

        /// <summary>
        /// Property representing the background image url.
        /// </summary>
        [JsonProperty("background")]
        public string Background { get; private set; }

        /// <summary>
        /// Property representing the language the broadcaster has flagged their channel as.
        /// </summary>
        [JsonProperty("broadcaster_language")]
        public string BroadcasterLanguage { get; private set; }

        /// <summary>
        /// Property representing date time string of channel creation.
        /// </summary>
        [JsonProperty("created_at")]
        public string CreatedAt { get; private set; }

        /// <summary>
        /// Property representing channel delay, if applied.
        /// </summary>
        [JsonProperty("delay")]
        public string Delay { get; private set; }

        /// <summary>
        /// Property representing customized display name.
        /// </summary>
        [JsonProperty("display_name")]
        public string DisplayName { get; private set; }

        /// <summary>
        /// Property representing the game the channel is playing.
        /// </summary>
        [JsonProperty("game")]
        public string Game { get; private set; }

        /// <summary>
        /// Property representing the signed language.
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; private set; }

        /// <summary>
        /// Property representing the logo of the channel.
        /// </summary>
        [JsonProperty("logo")]
        public string Logo { get; private set; }

        /// <summary>
        /// Property representing the channel name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// Property representing the banner that stretches across the top.
        /// </summary>
        [JsonProperty("profile_banner")]
        public string ProfileBanner { get; private set; }

        /// <summary>
        /// Property representing current channel status.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; private set; }

        /// <summary>
        /// Property representing date time of last channel update.
        /// </summary>
        [JsonProperty("updated_at")]
        public string UpdatedAt { get; private set; }
    }
}