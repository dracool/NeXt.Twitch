namespace NeXt.Twitch.PubSub.EventArgs
{
    /// <summary>
    /// Represents a timeout message from the pubsub system
    /// </summary>
    public class TimeoutMessageEventArgs : ModeratorActionMessageEventArgs
    {
        /// <summary>
        /// Initializes a Timeout message
        /// </summary>
        /// <param name="moderator">the moderator issuing the command</param>
        /// <param name="targetUser">the user that was timed out</param>
        /// <param name="duration">the timeout duration in seconds</param>
        /// <param name="reason">the reason the user was timed out</param>
        /// <param name="topic">the topic</param>
        public TimeoutMessageEventArgs(string moderator, string targetUser, int duration, string reason, string topic)
            : base(moderator, topic)
        {
            TargetUser = targetUser;
            Duration = duration;
            Reason = reason;
        }

        /// <summary>
        /// The moderator supplied reason for the timeout
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// The user who was affected by the command
        /// </summary>
        public string TargetUser { get;  }

        /// <summary>
        /// Timeout Duration in seconds
        /// </summary>
        public int Duration { get; }

    }
}