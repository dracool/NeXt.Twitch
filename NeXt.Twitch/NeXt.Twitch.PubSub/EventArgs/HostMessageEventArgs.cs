namespace NeXt.Twitch.PubSub.EventArgs
{
    /// <summary>
    /// Represents a host message from the PubSub system
    /// </summary>
    public class HostMessageEventArgs : ModeratorActionMessageEventArgs
    {
        /// <summary>
        /// Initializes a HostMessage from the pubsub system
        /// </summary>
        /// <param name="channel">the channel being hosted</param>
        /// <param name="moderator">the moderator</param>
        /// <param name="topic">the topic</param>
        public HostMessageEventArgs(string channel, string moderator, string topic) : base(
            moderator: moderator, 
            topic: topic)
        {
            Channel = channel;
        }

        /// <summary>
        /// The channel being hosted, null or empty when unhosting
        /// </summary>
        public string Channel { get; }
    }
}
