namespace NeXt.Twitch.PubSub.EventArgs
{
    /// <summary>
    /// Represents an unknown message from the PubSub system, useful for adding support
    /// </summary>
    public class UnknownMessageEventArgs : PubSubMessageEventArgs
    {
        /// <summary>
        /// Initializes an UnknownMessage from the PubSub system
        /// </summary>
        /// <param name="type">the type of unknown message</param>
        /// <param name="json">the raw json text</param>
        /// <param name="topic">the topic</param>
        public UnknownMessageEventArgs(PubSubMessageType type, string json, string topic) : base(topic)
        {
            Type = type;
            Json = json;
        }

        /// <summary>
        /// The exact type of unknown message
        /// </summary>
        public PubSubMessageType Type { get; }

        /// <summary>
        /// The full unaltered received json text
        /// </summary>
        public string Json { get; }
    }
}
