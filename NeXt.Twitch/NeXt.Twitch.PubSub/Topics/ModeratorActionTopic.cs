namespace NeXt.Twitch.PubSub.Topics
{
    /// <summary>
    /// Topic to listen to Moderator Actions on a Channel
    /// </summary>
    public sealed class ModeratorActionTopic : PubSubTopic
    {
        /// <summary>
        /// Creates an instance of ModeratorActionTopic
        /// </summary>
        /// <param name="channelId">the channel to listen on</param>
        /// <param name="userId">the listening user</param>
        /// <param name="oAuthToken">a valid token for the listening user</param>
        public ModeratorActionTopic(long channelId, int userId, string oAuthToken)
        {
            ChannelId = channelId;
            UserId = userId;
            OAuthToken = oAuthToken;
        }

        /// <summary>
        /// The Channel Id to listen in
        /// </summary>
        public long ChannelId { get; }

        /// <summary>
        /// The User Id of the listening user
        /// </summary>
        public int UserId { get; }
        
        internal override string Topic => $"chat_moderator_actions.{UserId}.{ChannelId}";
        internal override string OAuthToken { get; }
        internal override bool IsAuthorized => true;
    }
}