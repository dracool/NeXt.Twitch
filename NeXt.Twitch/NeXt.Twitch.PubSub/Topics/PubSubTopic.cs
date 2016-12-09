namespace NeXt.Twitch.PubSub.Topics
{
    /// <summary>
    /// Represents a PubSubTopic to listen to
    /// </summary>
    public abstract class PubSubTopic
    {
        /// <summary>
        /// the full topic string
        /// </summary>
        internal abstract string Topic { get; }

        /// <summary>
        /// The oauth token, value is ignored if <see cref="P:TwitchLib.PubSub.PubSubTopic.IsAuthorized" /> is <code>true</code>
        /// </summary>
        internal abstract string OAuthToken { get; }

        /// <summary>
        /// Whether the topic uses an oauth token
        /// </summary>
        internal abstract bool IsAuthorized { get; }
    }
}