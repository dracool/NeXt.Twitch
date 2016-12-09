using System;
using NeXt.Twitch.Chat.Messages;

namespace NeXt.Twitch.Chat.Connection
{
    /// <summary>
    /// An EventArgs class that is used to 
    /// </summary>
    public class UnknownMessageEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownMessageEventArgs"/> class
        /// </summary>
        /// <param name="message">the unknown mssage</param>
        public UnknownMessageEventArgs(TmiMessage message)
        {
            Message = message;
        }

        /// <summary>
        /// The message that wasn't recognized
        /// </summary>
        public TmiMessage Message { get; }
    }
}