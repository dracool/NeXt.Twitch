namespace NeXt.Twitch.Chat.Connection
{
    /// <summary>
    /// Represents data about a message that was dropped by the <see cref="TmiConnection"/>.
    /// </summary>
    public class MessageDroppedEventArgs : System.EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDroppedEventArgs"/>
        /// </summary>
        /// <param name="message">the raw message that was dropped</param>
        /// <param name="reason">the reason it was dropped</param>
        public MessageDroppedEventArgs(string message, MessageDropReason reason)
        {
            Message = message;
            Reason = reason;
        }

        /// <summary>
        /// The message that was dropped
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// The reason the message was dropped
        /// </summary>
        public MessageDropReason Reason { get; }
    }
}
