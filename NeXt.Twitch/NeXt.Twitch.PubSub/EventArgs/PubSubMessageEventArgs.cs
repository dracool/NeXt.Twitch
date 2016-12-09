using System;

namespace NeXt.Twitch.PubSub.EventArgs
{
    /// <summary>
    /// Base class for all PubSub mesages
    /// </summary>
    public abstract class PubSubMessageEventArgs : System.EventArgs
    {
        /// <summary>
        /// initializes the Message with topic and timestamp
        /// </summary>
        /// <param name="topic">the topic to initialize with</param>
        protected PubSubMessageEventArgs(string topic)
        {
            Topic = topic;
            Timestamp = DateTime.UtcNow;
        }

        /// <summary>
        /// The Topic the message had
        /// </summary>
        public string Topic { get; }

        /// <summary>
        /// The time the message was received
        /// </summary>
        public DateTime Timestamp { get; }
    }
}
