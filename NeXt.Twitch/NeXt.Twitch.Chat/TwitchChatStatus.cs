namespace NeXt.Twitch.Chat
{
    /// <summary>
    /// Reprsents the status of the client
    /// </summary>
    public enum TwitchChatStatus
    {
        /// <summary>
        /// Initial value, only set when the client was not used
        /// </summary>
        None,

        /// <summary>
        /// The client is currently connecting
        /// </summary>
        Connecting,

        /// <summary>
        /// The client is currently connected
        /// </summary>
        Connected,

        /// <summary>
        /// The client is currently disconnecting
        /// </summary>
        Disconnecting,

        /// <summary>
        /// The client is currently  disconnected
        /// </summary>
        Disconnected,

        /// <summary>
        /// The client is currently handling an unexpected disconnect
        /// </summary>
        Reconnecting,
    }
}
