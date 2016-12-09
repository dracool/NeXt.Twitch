namespace NeXt.Twitch.PubSub.Topics
{
    /// <summary>
    /// Topic to listen to bits events
    /// </summary>
    public sealed class BitsTopic : PubSubTopic
    {
        /// <summary>
        /// Creates an instance of BitsTopic with the specified parameters
        /// </summary>
        /// <param name="channelId">the channel id of the channel to listen in</param>
        /// <param name="oAuthToken">a valid oauth token for the channel</param>
        public BitsTopic(long channelId, string oAuthToken)
        {
            ChannelId = channelId;
            OAuthToken = oAuthToken;
        }

        /// <summary>
        /// The Channel Id of the channel to listen in
        /// </summary>
        public long ChannelId { get; }

        internal override string Topic => $"channel-bitsevents.{ChannelId}";
        internal override string OAuthToken { get; }
        internal override bool IsAuthorized => true;
    }
}