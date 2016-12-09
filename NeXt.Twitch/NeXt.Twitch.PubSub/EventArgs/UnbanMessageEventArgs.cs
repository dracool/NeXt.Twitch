namespace NeXt.Twitch.PubSub.EventArgs
{
    /// <summary>
    /// Represents an unban message from the pubsub system
    /// </summary>
    public class UnbanMessageEventArgs : ModeratorActionMessageEventArgs
    {
        /// <summary>
        /// Initializes an UnbanMessage
        /// </summary>
        /// <param name="moderator">the moderator issuing the command</param>
        /// <param name="targetUser">the unbanned user</param>
        /// <param name="topic">the topic</param>
        public UnbanMessageEventArgs(string moderator, string targetUser, string topic) : base(moderator, topic)
        {
            TargetUser = targetUser;
        }

        /// <summary>
        /// The user affected by the command
        /// </summary>
        public string TargetUser { get; }
    }
}