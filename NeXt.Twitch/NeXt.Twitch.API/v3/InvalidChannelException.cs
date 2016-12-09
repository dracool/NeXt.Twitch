using System;
using System.Runtime.Serialization;

namespace NeXt.Twitch.API
{
    /// <summary>
    /// Thrown when a channel name was invalid
    /// </summary>
    [Serializable]
    public class InvalidChannelException : Exception
    {
        /// <summary>
        /// Initializes the exception
        /// </summary>
        public InvalidChannelException()
        {
        }

        /// <summary>
        /// Initializes the exception
        /// </summary>
        /// <param name="message">the message to set</param>
        public InvalidChannelException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes the exception
        /// </summary>
        /// <param name="message">the message to set</param>
        /// <param name="inner">the inner exception to set</param>
        public InvalidChannelException(string message, Exception inner)
            : base(message, inner) { }

        /// <summary>
        /// Initializes the exception
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected InvalidChannelException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}