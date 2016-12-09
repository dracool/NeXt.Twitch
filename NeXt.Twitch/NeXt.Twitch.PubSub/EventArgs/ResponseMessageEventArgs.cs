namespace NeXt.Twitch.PubSub.EventArgs
{
    /// <summary>
    /// Represents a response message from the PubSub system
    /// </summary>
    public class ResponseMessageEventArgs : PubSubMessageEventArgs
    {
        /// <summary>
        /// Initializes a ResponseMessage
        /// </summary>
        /// <param name="error">the error text received</param>
        /// <param name="topic">the topic</param>
        public ResponseMessageEventArgs(string error, string topic) : base(topic)
        {
            Error = error;
            Success = string.IsNullOrWhiteSpace(error);
        }

        /// <summary>
        /// The Error text received
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// False if an error occured, True otherwise
        /// </summary>
        public bool Success { get; }
    }
}
