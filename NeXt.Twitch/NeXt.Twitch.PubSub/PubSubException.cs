using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using NeXt.Twitch.Common;

namespace NeXt.Twitch.PubSub
{
    /// <summary>
    /// Base Exception for all exceptions in the <see cref="TwitchPubSub"/> class
    /// </summary>
    [Serializable]
    public class PubSubException : TwitchException
    {
        /// <summary>
        /// Initializes the <see cref="PubSubException"/>
        /// </summary>
        public PubSubException() { }

        /// <summary>
        /// Initializes the <see cref="PubSubException"/>
        /// </summary>
        /// <param name="message">the message to set</param>
        public PubSubException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes the <see cref="PubSubException"/>
        /// </summary>
        /// <param name="message">the message to set</param>
        /// <param name="inner">the inner exception to set</param>
        public PubSubException(string message, Exception inner)
            : base(message, inner) { }

        /// <summary>
        /// Initializer when deserializing
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected PubSubException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        /// <summary>
        /// GetObjectData override for serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}