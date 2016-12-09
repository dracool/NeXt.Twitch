using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using NeXt.Twitch.Common;

namespace NeXt.Twitch.Chat.Messages
{
    /// <summary>
    /// An Exception that holds data about a parsing error in the <see cref="TmiMessageParser"/>
    /// </summary>
    [Serializable]
    public class MessageParsingException : TwitchException
    {
        /// <summary>
        /// The Raw Text of the message that failed to parse
        /// </summary>
        public string RawMessage { get; set; }

        /// <summary>
        /// Initializes the <see cref="MessageParsingException"/>
        /// </summary>
        public MessageParsingException()
        {
        }

        /// <summary>
        /// Initializes the <see cref="MessageParsingException"/>
        /// </summary>
        /// <param name="message">the message to set</param>
        public MessageParsingException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes the <see cref="MessageParsingException"/>
        /// </summary>
        /// <param name="message">the message to set</param>
        /// <param name="inner">the inner exception to set</param>
        public MessageParsingException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes the <see cref="MessageParsingException"/>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected MessageParsingException(SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
            RawMessage = info.GetString(nameof(RawMessage));
        }

        /// <summary>
        /// GetObjectData override for serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info,
            StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            info.AddValue(nameof(RawMessage), RawMessage);
            base.GetObjectData(info, context);
        }
    }
}