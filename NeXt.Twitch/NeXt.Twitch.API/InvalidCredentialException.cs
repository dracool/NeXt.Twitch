using System;
using System.Runtime.Serialization;

namespace NeXt.Twitch.API
{
    /// <summary>
    /// Thrown when a ClientId or access token was invalid
    /// </summary>
    [Serializable]
    public class InvalidApiCredentialException : Exception
    {
        /// <summary>
        /// Initializes the exception
        /// </summary>
        public InvalidApiCredentialException()
        {
        }

        /// <summary>
        /// Initializes the exception
        /// </summary>
        /// <param name="message">the message to set</param>
        public InvalidApiCredentialException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes the exception
        /// </summary>
        /// <param name="message">the message to set</param>
        /// <param name="inner">the inner exception to set</param>
        public InvalidApiCredentialException(string message, Exception inner)
            : base(message, inner) { }

        /// <summary>
        /// Initializes the exception
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected InvalidApiCredentialException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
