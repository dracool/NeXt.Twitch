namespace NeXt.Twitch.Chat.Connection
{
    /// <summary>
    /// Represents a status change in the <see cref="TmiConnection"/> client
    /// </summary>
    public class StatusChangedEventArgs : System.EventArgs
    { 
        /// <summary>
        /// Initializes an instance of the StatusChangedEventArgs
        /// </summary>
        /// <param name="oldStatus">the old status</param>
        /// <param name="newStatus">the new status</param>
        public StatusChangedEventArgs(TwitchChatStatus oldStatus, TwitchChatStatus newStatus)
        {
            Old = oldStatus;
            New = newStatus;
        }

        /// <summary>
        /// The previous status
        /// </summary>
        public TwitchChatStatus Old { get; }

        /// <summary>
        /// The new status
        /// </summary>
        public TwitchChatStatus New { get; }
    }
}
