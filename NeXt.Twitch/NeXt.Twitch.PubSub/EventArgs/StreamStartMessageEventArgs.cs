namespace NeXt.Twitch.PubSub.EventArgs
{   
     /// <summary>
     /// Represents a stream start message from the pubsub system
     /// </summary>
    public class StreamStartMessageEventArgs : VideoMessageEventArgs
    {
        /// <summary>
        /// Initializes a StreamStartMessage
        /// </summary>
        /// <param name="delay">the delay in seconds</param>
        /// <param name="serverTime">the server time</param>
        /// <param name="topic">the topic</param>
        public StreamStartMessageEventArgs(int delay, string serverTime, string topic) : base(serverTime, topic)
        {
            Delay = delay;
        }

        /// <summary>
        /// The stream delay
        /// </summary>
        public int Delay { get; }
    }
}
