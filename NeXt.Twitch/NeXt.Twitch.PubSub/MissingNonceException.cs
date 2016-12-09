using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace NeXt.Twitch.PubSub
{
    /// <summary>
    /// Signals that a nonce was not found when it needed to be resolved
    /// </summary>
    [Serializable]
    public class MissingNonceException : PubSubException
    {
        /// <summary>
        /// The Nonce t hat was missing
        /// </summary>
        public string Nonce { get; set; }

        /// <summary>
        /// Initializes the exception
        /// </summary>
        public MissingNonceException()
        {
        }

        /// <summary>
        /// Initializes the exception
        /// </summary>
        /// <param name="message">the message to set</param>
        public MissingNonceException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes the exception
        /// </summary>
        /// <param name="message">the message to set</param>
        /// <param name="inner">the inner exception to set</param>
        public MissingNonceException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes the exception
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected MissingNonceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Nonce = info.GetString(nameof(Nonce));
        }

        /// <summary>
        /// GetObjectData override for serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            info.AddValue(nameof(Nonce), Nonce);
            base.GetObjectData(info, context);
        }
    }
}
