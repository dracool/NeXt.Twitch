namespace NeXt.Twitch.PubSub.EventArgs
{
    /// <summary>
    /// Represents a viewer count message from the pubsub system
    /// </summary>
    public class ViewerCountMessageEventArgs : VideoMessageEventArgs
    {
        /// <summary>
        /// Initializes a ViewerCountMessage
        /// </summary>
        /// <param name="viewerCount">the viewer count</param>
        /// <param name="serverTime">the server time</param>
        /// <param name="topic">the topic</param>
        public ViewerCountMessageEventArgs(int viewerCount, string serverTime, string topic) : base(serverTime, topic)
        {
            ViewerCount = viewerCount;
        }

        /// <summary>
        /// The received viewer count
        /// </summary>
        public int ViewerCount { get; }
    }
}
