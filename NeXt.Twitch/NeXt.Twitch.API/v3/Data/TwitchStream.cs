using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace NeXt.Twitch.API.Data
{
    /// <summary>
    /// Class representing a stream as returned by Twitch API
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    public sealed class TwitchStream
    {
        [JsonConstructor]
        private TwitchStream() { }

        /// <summary>
        /// Property representing whether or not the stream is playlist or live.
        /// </summary>
        [JsonProperty("is_playlist")]
        public bool? IsPlaylist { get; private set; }

        /// <summary>
        /// Property representing average frames per second.
        /// </summary>
        [JsonProperty("average_fps")]
        public double? AverageFps { get; private set; }

        /// <summary>
        /// Property representing any delay on the stream (in seconds)
        /// </summary>
        [JsonProperty("delay")]
        public int? Delay { get; private set; }

        /// <summary>
        /// Property representing height dimension.
        /// </summary>
        [JsonProperty("video_height")]
        public int? VideoHeight { get; private set; }

        /// <summary>
        /// Property representing number of current viewers.
        /// </summary>
        [JsonProperty("viewers")]
        public int? Viewers { get; private set; }

        /// <summary>
        /// Property representing the stream id.
        /// </summary>
        [JsonProperty("_id")]
        public long? Id { get; private set; }

        /// <summary>
        /// Property representing the preview images in an object.
        /// </summary>
        [JsonProperty("preview")]
        public SizedImageUrls Previews { get; private set; }

        /// <summary>
        /// Property representing the date time the stream was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; private set; }

        /// <summary>
        /// Property representing the current game.
        /// </summary>
        [JsonProperty("game")]
        public string Game { get; private set; }

        /// <summary>
        /// Property representing the channel the stream is from.
        /// </summary>
        [JsonProperty("channel")]
        public Channel Channel { get; private set; }
    }
}