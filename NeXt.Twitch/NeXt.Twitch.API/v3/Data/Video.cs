using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace NeXt.Twitch.API.Data
{
    /// <summary>
    /// Class representing returned Video object.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    [SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]
    public sealed class Video
    {
        [JsonConstructor]
        private Video() { }

        /// <summary>
        /// Object representing all channel data returned by this request.
        /// </summary>
        [JsonProperty("channel")]
        public ChannelInfo Channel { get; private set; }

        /// <summary>
        /// Object representing the available FPSs of versions of the video
        /// </summary>
        [JsonProperty("fps")]
        public FpsInfo Fps { get; private set; }

        /// <summary>
        /// Length of video in seconds.
        /// </summary>
        [JsonProperty("length")]
        public int? Length { get; private set; }

        /// <summary>
        /// Number of recorded views.
        /// </summary>
        [JsonProperty("views")]
        public int? Views { get; private set; }

        /// <summary>
        /// All available resolutions of video.
        /// </summary>
        [JsonProperty("resolutions")]
        public ResolutionInfo Resolutions { get; private set; }

        /// <summary>
        /// Unique identifier assigned to broadcast video originated from.
        /// </summary>
        [JsonProperty("broadcast_id")]
        public string BroadcastId { get; private set; }

        /// <summary>
        /// Creator's description of video.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; private set; }

        /// <summary
        /// >Game being played in the video.
        /// </summary>
        [JsonProperty("game")]
        public string Game { get; private set; }

        /// <summary>
        /// Id of the particular video.
        /// </summary>
        [JsonProperty("_id")]
        public string Id { get; private set; }

        /// <summary>
        /// Video preview image link.
        /// </summary>
        [JsonProperty("preview")]
        public string Preview { get; private set; }

        /// <summary>
        /// Date and time string representing recorded date.
        /// </summary>
        [JsonProperty("recorded_at")]
        public DateTime? RecordedAt { get; private set; }

        /// <summary>
        /// Current status of the recorded video.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; private set; }

        /// <summary>
        /// Tags assigned to video either automatically or by content creator.
        /// </summary>
        [JsonProperty("tag_list")]
        public string TagList { get; private set; }

        /// <summary>
        /// Title of video.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; private set; }

        /// <summary>
        /// Twitch URL to video.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; private set; }

        /// <summary>
        /// Class representing resolution data.
        /// </summary>
        [JsonObject(MemberSerialization.OptIn)]
        public sealed class ResolutionInfo
        {
            /// <summary>
            /// Property representing resolution for medium quality.
            /// </summary>
            [JsonProperty("medium")]
            public string Medium { get; private set; }

            /// <summary>
            /// Property representing resolution for mobile quality.
            /// </summary>
            [JsonProperty("mobile")]
            public string Mobile { get; private set; }

            /// <summary>
            /// Property representing resolution for high quality.
            /// </summary>
            [JsonProperty("high")]
            public string High { get; private set; }

            /// <summary>
            /// Property representing resolution for low quality.
            /// </summary>
            [JsonProperty("low")]
            public string Low { get; private set; }

            /// <summary>
            /// Property representing resolution for chunked quality.
            /// </summary>
            [JsonProperty("chunked")]
            public string Chunked { get; private set; }
        }

        /// <summary>
        /// Class representing channel data.
        /// </summary>
        [JsonObject(MemberSerialization.OptIn)]
        public sealed class ChannelInfo
        {
            [JsonConstructor]
            private ChannelInfo() { }

            /// <summary>
            /// Property representing Name of channel.
            /// </summary>
            [JsonProperty("name")]
            public string Name { get; private set; }

            /// <summary>
            /// Property representing DisplayName of channel.
            /// </summary>
            [JsonProperty("display_name")]
            public string DisplayName { get; private set; }
        }

        /// <summary>
        /// Class representing FPS data.
        /// </summary>
        [JsonObject(MemberSerialization.OptIn)]
        public sealed class FpsInfo
        {
            [JsonConstructor]
            private FpsInfo() { }

            /// <summary>
            /// Property representing FPS for audio only.
            /// </summary>
            [JsonProperty("audio_only")]
            public double? AudioOnly { get; private set; }

            /// <summary>
            /// Property representing FPS for medium quality.
            /// </summary>
            [JsonProperty("medium")]
            public double? Medium { get; private set; }

            /// <summary>
            /// Property representing FPS for mobile quality.
            /// </summary>
            [JsonProperty("mobile")]
            public double? Mobile { get; private set; }

            /// <summary>
            /// Property representing FPS for high quality.
            /// </summary>
            [JsonProperty("high")]
            public double? High { get; private set; }

            /// <summary>
            /// Property representing FPS for low quality.
            /// </summary>
            [JsonProperty("low")]
            public double? Low { get; private set; }

            /// <summary>
            /// Property representing FPS for chunked quality.
            /// </summary>
            [JsonProperty("chunked")]
            public double? Chunked { get; private set; }
        }
    }


}