namespace NeXt.Twitch.PubSub.EventArgs
{
    /// <summary>
    /// Represents a ban message from the pubsub system
    /// </summary>
    public class BanMessageEventArgs : ModeratorActionMessageEventArgs
    {
        /// <summary>
        /// Initializes a BanMessage
        /// </summary>
        /// <param name="moderator">The moderator issuing the command</param>
        /// <param name="targetUser">the banned user</param>
        /// <param name="reason">the reason the user was banned</param>
        /// <param name="topic">the topic</param>
        public BanMessageEventArgs(string moderator, string targetUser, string reason, string topic) 
            : base(
                  topic: topic,
                  moderator: moderator
            )
        {
            TargetUser = targetUser;
            Reason = reason;
        }
        
        /// <summary>
        /// The moderator supplied reason
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// The user affected by the command
        /// </summary>
        public string TargetUser { get; }
    }
}