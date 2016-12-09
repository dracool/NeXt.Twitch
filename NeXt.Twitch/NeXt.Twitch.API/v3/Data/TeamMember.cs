using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace NeXt.Twitch.API.Data
{
    /// <summary>
    /// Class representing a team member as returned by Twitch API.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    public sealed class TeamMember
    {
        [JsonConstructor]
        private TeamMember() { }

        /// <summary>
        /// Property representing the streamers status
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; private set; }

        /// <summary>
        /// Property representing the various image sizes.
        /// </summary>
        [JsonProperty("image")]
        public TeamImages Images { get; private set; }

        /// <summary>
        /// Property representing the current viewer count.
        /// </summary>
        [JsonProperty("current_viewers")]
        public int CurrentViewers { get; private set; }

        /// <summary>
        /// Property representing the current follower count.
        /// </summary>
        [JsonProperty("followers_count")]
        public int? FollowerCount { get; private set; }

        /// <summary>
        /// Property representing the total view count.
        /// </summary>
        [JsonProperty("total_views")]
        public int? TotalViews { get; private set; }

        /// <summary>
        /// Property representing the channel description.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; private set; }

        /// <summary>
        /// Property representing the streamer customized display name.
        /// </summary>
        [JsonProperty("display_name")]
        public string DisplayName { get; private set; }

        /// <summary>
        /// Property representing the link to the channel.
        /// </summary>
        [JsonProperty("link")]
        public string Link { get; private set; }

        /// <summary>
        /// Property representing the meta game of the channel.
        /// </summary>
        [JsonProperty("meta_game")]
        public string MetaGame { get; private set; }

        /// <summary>
        /// Property representing the name of the channel.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// Property representing the title of the channel.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; private set; }

        /// <summary>
        /// Property representing whether streamer is live.
        /// </summary>
        public bool IsLive() => string.Equals(Status, "live", StringComparison.Ordinal);
    }

    /// <summary>
    /// Class representing the various sizes of images.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    public sealed class TeamImages
    {
        [JsonConstructor]
        private TeamImages() { }

        /// <summary>
        /// Property representing the 28 size url.
        /// </summary>
        [JsonProperty("size28")]
        public string Size28 { get; private set; }

        /// <summary>
        /// Property representing the 50 size url.
        /// </summary>
        [JsonProperty("size50")]
        public string Size50 { get; private set; }

        /// <summary>
        /// Property representing the 70 size url.
        /// </summary>
        [JsonProperty("size70")]
        public string Size70 { get; private set; }

        /// <summary
        /// >Property representing the 150 size url.
        /// </summary>
        [JsonProperty("size150")]
        public string Size150 { get; private set; }

        /// <summary>
        /// Property representing the 300 size url.
        /// </summary>
        [JsonProperty("size300")]
        public string Size300 { get; private set; }

        /// <summary>
        /// Property representing the 600 size url.
        /// </summary>
        [JsonProperty("size600")]
        public string Size600 { get; private set; }
    }
}