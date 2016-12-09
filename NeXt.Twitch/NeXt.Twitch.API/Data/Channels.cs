using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace NeXt.Twitch.API.Data
{
    /// <summary>
    /// Class representing Channels object.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]//used by Json.Net
    [SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]//Instantiated by Json.Net
    public sealed class Channels
    {
        [JsonConstructor]
        private Channels() { }

        /// <summary>
        /// Property representing the display name of a user.
        /// </summary>
        [JsonProperty("display_name")]
        public string DisplayName { get; private set; }

        /// <summary>
        /// Property representing the game a channel is playing.
        /// </summary>
        [JsonProperty("game")]
        public string Game { get; private set; }

        /// <summary>
        /// Property representing the status of a specific channel.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; private set; }

        /// <summary>
        /// Property representing whether or not a channel is fighting adblock.
        /// </summary>
        [JsonProperty("fight_ad_block")]
        public bool FightAdBlock { get; private set; }

        /// <summary>
        /// Property representing internal Id variable.
        /// </summary>
        [JsonProperty("_id")]
        public long? Id { get; private set; }

        /// <summary>
        /// Property representing the name of a channel.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }

        /// <summary>P
        /// roperty representing the partner status of the channel.
        /// </summary>
        [JsonProperty("partner")]
        public bool Partner { get; private set; }

        /// <summary>
        /// Property represeting Twitch's channel Liverail Id
        /// </summary>
        [JsonProperty("twitch_liverail_id")]
        public long? TwitchLiverailId { get; private set; }

        /// <summary>
        /// Property representing the LiverailId.
        /// </summary>
        [JsonProperty("liverail_id")]
        public long? LiverailId { get; private set; }

        /// <summary>
        /// Property representing the comscore id.
        /// </summary>
        [JsonProperty("comscore_id")]
        public string ComscoreId { get; private set; }

        /// <summary>
        /// Property representing Comscore6(?).
        /// </summary>
        [JsonProperty("comscore_c6")]
        public string ComscoreC6 { get; private set; }

        /// <summary>
        /// Property representing the Steam Id of the user (if available).
        /// </summary>
        [JsonProperty("steam_id")]
        public long? SteamId { get; private set; }

        /// <summary>
        /// Property representing the PPV status of the channel.
        /// </summary>
        [JsonProperty("ppv")]
        public bool PPV { get; private set; }

        /// <summary>
        /// Property representing the broadcaster software (fairly unreliable).
        /// </summary>
        [JsonProperty("broadcaster_software")]
        public string BroadcasterSoftware { get; private set; }

        /// <summary>
        /// Property representing the preroll status of the channel (preroll ads)
        /// </summary>
        [JsonProperty("prerolls")]
        public bool Prerolls { get; private set; }

        /// <summary>
        /// Property representing the postrolls status of the channel (postroll ads)
        /// </summary>
        [JsonProperty("postrolls")]
        public bool Postrolls { get; private set; }
    }
}
