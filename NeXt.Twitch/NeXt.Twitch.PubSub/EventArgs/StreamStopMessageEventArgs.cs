namespace NeXt.Twitch.PubSub.EventArgs
{
    /// <summary>
    /// Represents a stream stop message from the pubsub system
    /// </summary>
    public class StreamStopMessageEventArgs : VideoMessageEventArgs
    {
        /// <summary>
        /// Initializes a StreamStopMessage
        /// </summary>
        /// <param name="delay">the delay in seconds</param>
        /// <param name="serverTime">the server time</param>
        /// <param name="topic">the topic</param>
        public StreamStopMessageEventArgs(int delay, string serverTime, string topic) : base(serverTime, topic)
        {
            Delay = delay;
        }

        /// <summary>
        /// The stream delay
        /// </summary>
        public int Delay { get; }
    }
}