namespace NeXt.Twitch.Chat.Messages
{
    /// <summary>
    /// Represents a parsed tmi message prefix
    /// </summary>
    public class TmiMessagePrefix
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TmiMessagePrefix"/>
        /// </summary>
        /// <param name="hostname">The hostname.</param>
        /// <param name="nickname">The nickname.</param>
        /// <param name="username">The username.</param>
        public TmiMessagePrefix(string hostname, string nickname, string username)
        {
            Hostname = hostname;
            Nickname = nickname;
            Username = username;
        }

        /// <summary>
        /// The Hostname
        /// </summary>
        public string Hostname { get; }

        /// <summary>
        /// The Nickname
        /// </summary>
        public string Nickname { get; }

        /// <summary>
        /// The Username
        /// </summary>
        public string Username { get; }
    }
}