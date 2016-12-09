namespace NeXt.Twitch.PubSub.Topics
{
    /// <summary>
    /// Topic to listen to video playback events
    /// </summary>
    public sealed class VideoPlaybackTopic : PubSubTopic
    {
        /// <summary>
        /// Creates an instance of VideoPlaybackTopic with the specified parameters
        /// </summary>
        /// <param name="channelId">the channel id of the channel to listen in</param>
        public VideoPlaybackTopic(long channelId)
        {
            ChannelId = channelId;
        }

        /// <summary>
        /// The channel id of the channel to listen in
        /// </summary>
        public long ChannelId { get; }

        internal override string Topic => $"video-playback.{ChannelId}";
        internal override string OAuthToken => string.Empty;
        internal override bool IsAuthorized => false;
    }
}