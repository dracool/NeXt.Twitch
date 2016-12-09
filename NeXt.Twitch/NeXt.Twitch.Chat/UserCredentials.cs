using System;

namespace NeXt.Twitch.Chat
{
    /// <summary>
    /// Holds user data for chat login
    /// </summary>
    public class UserCredentials : IUserCredentials
    {
        /// <summary>
        /// initializes a <see cref="UserCredentials"/> instance
        /// </summary>
        /// <param name="userName">the username of the user</param>
        /// <param name="token">an oauth token, with or without oauth: prefix</param>
        public UserCredentials(string userName, string token)
        {
            if (userName == null) throw new ArgumentNullException(nameof(userName));
            if (token == null) throw new ArgumentNullException(nameof(token));
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("Value cannot be empty or whitespace.", nameof(userName));
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Value cannot be empty or whitespace.", nameof(token));
            UserName = userName;
            Token = token;
        }

        /// <summary>
        /// The username of the user
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// Valid oauth token with chat_login scope
        /// </summary>
        public string Token { get; }
    }
}
