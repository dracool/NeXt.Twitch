namespace NeXt.Twitch.Chat
{
    /// <summary>
    /// Represents user data used to connect to the chat. Use <see cref="UserCredentials"/>
    /// if you are not using some other class to hold that data already
    /// </summary>
    /// <seealso cref="UserCredentials"/>
    /// <seealso cref="Client.TmiClient"/>
    public interface IUserCredentials
    {
        /// <summary>
        /// The username of the user
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Valid oauth token with chat_login scope
        /// </summary>
        string Token { get; }
    }
}