namespace NeXt.Twitch.PubSub.EventArgs
{
    /// <summary>
    /// Base class for all Video related events
    /// </summary>
    public abstract class VideoMessageEventArgs : PubSubMessageEventArgs
    {
        /// <summary>
        /// Initializes a VideoMessage
        /// </summary>
        /// <param name="serverTime">the server time</param>
        /// <param name="topic">the topic</param>
        protected VideoMessageEventArgs(string serverTime, string topic) : base(topic)
        {
            ServerTime = serverTime;
        }

        /// <summary>
        /// The server time sent by the message
        /// </summary>
        public string ServerTime { get; }
    }
}
