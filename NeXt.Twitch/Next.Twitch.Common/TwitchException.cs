using System;
using System.Runtime.Serialization;

namespace NeXt.Twitch.Common
{
    /// <summary>
    /// Base class for all twitch exceptions
    /// </summary>
    [Serializable]
    public class TwitchException : Exception
    {
        /// <summary>
        /// Initializes the <see cref="TwitchException"/>
        /// </summary>
        public TwitchException()
        {
        }

        /// <summary>
        /// Initializes the <see cref="TwitchException"/>
        /// </summary>
        /// <param name="message">the message to set</param>
        public TwitchException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes the <see cref="TwitchException"/>
        /// </summary>
        /// <param name="message">the message to set</param>
        /// <param name="inner">the inner exception to set</param>
        public TwitchException(string message, Exception inner)
            : base(message, inner) { }

        /// <summary>
        /// Initializes the <see cref="TwitchException"/>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected TwitchException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
