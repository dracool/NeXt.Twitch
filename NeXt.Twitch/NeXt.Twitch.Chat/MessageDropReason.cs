namespace NeXt.Twitch.Chat
{
    /// <summary>
    /// Represents a reason 
    /// </summary>
    public enum MessageDropReason
    {
        /// <summary>
        /// The message failed validation
        /// </summary>
        Validation,

        /// <summary>
        /// The messages queue was full so the message was dropped
        /// </summary>
        QueueOverflow,
    }
}