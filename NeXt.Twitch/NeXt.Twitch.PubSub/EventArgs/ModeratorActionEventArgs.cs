namespace NeXt.Twitch.PubSub.EventArgs
{
    /// <summary>
    /// Base class for all moderator action messages
    /// </summary>
    public abstract class ModeratorActionMessageEventArgs : PubSubMessageEventArgs
    {
        /// <summary>
        /// Initializes a ModeratorActionMessage from the pubsub system
        /// </summary>
        /// <param name="topic">the topic</param>
        /// <param name="moderator">the moderator that sent the command</param>
        protected ModeratorActionMessageEventArgs(string moderator, string topic) : base(topic)
        {
            Moderator = moderator;
            var sp = topic.Split('.');
            SelfId = int.Parse(sp[1]);
            ChannelId = long.Parse(sp[2]);
        }

        /// <summary>
        /// The Source channels id
        /// </summary>
        public long ChannelId { get; }

        /// <summary>
        /// The Id of the user listening to actions
        /// </summary>
        public int SelfId { get; }

        /// <summary>
        /// The moderator who sent the command
        /// </summary>
        public string Moderator { get; }
    }
}
