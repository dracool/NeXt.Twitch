using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using NeXt.Twitch.Chat.Connection;
using NeXt.Twitch.Chat.Messages;

namespace NeXt.Twitch.Chat.Client
{
    //TODO: add support for timeout, ban, unban, untimeout, whisper, etc. etc.
    /// <summary>
    /// Represents a low-level chat client that does not keep channel or user state.
    /// </summary>
    /// <example>
    /// Initializes a client and API instance, gets an appropriate server address,
    /// connects to the server, joins a channel, sends Hello World and then disconnects.
    /// <para>
    /// Note: It is not neccessarily required to use the API to obtain a server address.
    /// It is however highly encouraged to support possible server changes in the future.
    /// </para>
    /// <code>
    /// class Program
    /// {
    ///	    private const string ChannelName = "&lt;channel&gt;";
    ///	    private const string ClientId = "&lt;client-id&gt;";
    ///	    private const string userName = "&lt;username&gt;";
    ///	    private const string oAuthT = "&lt;oauth-token&gt;";
    ///	    
    ///	    private static TwitchApi;
    ///	    private static TmiClient;
    ///	    
    ///	    async Task Run()
    ///	    {
    ///	    	twitchApi = new TwitchApi(ClientId);
    ///	    	
    ///	    	var server = (await twitchApi.GetChatServers(ChannelName))
    ///	    		.FirstOrDefault(ep =&gt; ep.Port == 6667);
    ///	    	
    ///	    	client = new TmiClient(new UserCredentials(userName, oAuthT), server)
    ///	    	{
    ///	    		ChatLimiter = new DefaultMessageLimiter(10, TimeSpan.FromSeconds(30)),
    ///	    		JoinLimiter = new DefaultMessageLimiter(5, TimeSpan.FromSeconds(30)),
    ///	    		ReconnectStrategy = new ExponentialBackoffStrategy(),
    ///	    		QueueSize = 50,
    ///	    	};	
    ///	    	
    ///	    	client.Login += ClientOnLogin;
    ///	    	client.Disconnected += ClientOnDisconnected;
    ///            client.ChannelJoined += ClientOnChannelJoined;
    ///	    	
    ///	    	await client.ConnectAsync();
    ///	    }
    ///	    
    ///	    private static async void ClientOnChannelJoined(object sender, ChannelConnectionChangedEventArgs e)
    ///	    {
    ///	    	client.SendMessage(e.Channel, "Hello World");
    ///	    	await client.DisconnectAsync();
    ///	    	client.Dispose();
    ///	    }
    ///	    
    ///	    private static void ClientOnLogin(object sender, EventArgs eventArgs)
    ///	    {
    ///	    	client.Join(ChannelName);
    ///	    }
    ///
    ///	    void Main(string[] args)
    ///	    {
    ///	    	Run().GetAwaiter().GetResult();
    ///	    	Console.ReadLine();
    ///	    }
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="TmiConnection"/>
    public class TmiClient : TmiConnection
    {
        /// <summary>
        /// Initializes a new instance of <see cref="TmiClient"/>
        /// </summary>
        /// <param name="credentials">the credentials to use for login</param>
        /// <seealso cref="IUserCredentials"/>
        /// <see cref="UserCredentials"/>
        public TmiClient(IUserCredentials credentials)
        {
            if (credentials == null) throw new ArgumentNullException(nameof(credentials));
            Credentials = credentials;
            userName = Credentials.UserName.ToLowerInvariant();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TmiClient"/>
        /// </summary>
        /// <param name="credentials">the credentials to use for login</param>
        /// <param name="server">the server to connect to</param>
        /// <seealso cref="IUserCredentials"/>
        /// <see cref="UserCredentials"/>
        public TmiClient(IUserCredentials credentials, DnsEndPoint server) 
            : base(server)
        {
            if (credentials == null) throw new ArgumentNullException(nameof(credentials));
            Credentials = credentials;
            userName = Credentials.UserName.ToLowerInvariant();
        }
        
        /// <summary>
        /// the users credentials
        /// </summary>
        /// <seealso cref="IUserCredentials"/>
        public IUserCredentials Credentials { get; }

        /// <summary>
        /// the all lowercase username of the credentials
        /// </summary>
        private readonly string userName;

        /// <summary>
        /// Limiter used when sending messages to a channel
        /// </summary>
        /// <seealso cref="IMessageLimiter"/>
        /// <seealso cref="DefaultMessageLimiter"/>
        public IMessageLimiter ChatLimiter { get; set; }

        /// <summary>
        /// Limiter used when joining channels
        /// </summary>
        /// <seealso cref="IMessageLimiter"/>
        /// <seealso cref="DefaultMessageLimiter"/>
        public IMessageLimiter JoinLimiter { get; set; }

        /// <summary>
        /// Limiter used when sending whispers
        /// </summary>
        /// <seealso cref="IMessageLimiter"/>
        /// <seealso cref="DefaultMessageLimiter"/>
        public IMessageLimiter WhisperLimiter { get; set; }

        /// <summary>
        /// Sends a raw irc text to the client
        /// </summary>
        /// <remarks>This message is limited by the <see cref="ChatLimiter"/> instance set on this object.</remarks>
        /// <param name="raw">the raw message, NOT VALIDATED</param>
        /// <seealso cref="IMessageLimiter"/>
        public void SendRaw(string raw)
        {
            Send(raw, ChatLimiter);
        }

        /// <summary>
        /// joins a channel
        /// </summary>
        /// <remarks>This message is limited by the <see cref="JoinLimiter"/> instance set on this object.</remarks>
        /// <param name="channel">the channel name</param>
        /// <seealso cref="IMessageLimiter"/>
        public void Join(string channel)
        {
            Send(MessageBuilder.Join(channel), JoinLimiter);
        }

        /// <summary>
        /// Leaves a channel
        /// </summary>
        /// <param name="channel">the channel name</param>
        public void Part(string channel)
        {
            Send(MessageBuilder.Part(channel), null);
        }

        /// <summary>
        /// Send a PRIVMSG on the channel
        /// </summary>
        /// <param name="channel">the channel to send on</param>
        /// <param name="text">the text to send</param>
        public void SendMessage(string channel, string text)
        {
            Send(MessageBuilder.PrivMsg(userName,channel, text), ChatLimiter);
        }

        /// <inheritdoc/>
        protected override void OnConnected()
        {
            DoUserLogin();
            base.OnConnected();
        }

        /// <summary>
        /// sends messages to log in the user
        /// </summary>
        private void DoUserLogin()
        {
            Send(MessageBuilder.Pass(
                    Credentials.Token.StartsWith("oauth:")
                        ? Credentials.Token
                        : "oauth:" + Credentials.Token),
                null,
                MessagePriority.Critical);
            Send(MessageBuilder.Nick(userName), null, MessagePriority.Critical);
        }

        /// <summary>
        /// Called when one of the non-useful messages was received.
        /// This Method does nothing by default.
        /// </summary>
        /// <remarks>
        /// The following commands are handled by this Method:
        /// <c>001, 002, 003, 353, 366, 372, 375, 376</c>
        /// </remarks>
        /// <param name="msg">the message</param>
        [IrcCommand("001", "002", "003", "353", "366", "372", "375", "376")]
        protected virtual void OnIgnored(TmiMessage msg)
        {
            
        }

        /// <summary>
        /// Called when a ping message was received.
        /// This method sends an appropriate PONG reply back to the server.
        /// </summary>
        /// <remarks>
        /// The following commands are handled by this Method:
        /// <c>PING</c>
        /// </remarks>
        /// <param name="msg">the message received</param>
        [IrcCommand("PING")]
        protected virtual void OnPing(TmiMessage msg) => Send($"PONG {msg.RawPrefix}", null, MessagePriority.Critical);

        /// <summary>
        /// Fired when the client was successfully logged in
        /// </summary>
        public event EventHandler Login;

        /// <summary>
        /// Fired when a channel was joined
        /// </summary>
        public event EventHandler<ChannelConnectionChangedEventArgs> ChannelJoined;

        /// <summary>
        /// Fired when a channel was left
        /// </summary>
        public event EventHandler<ChannelConnectionChangedEventArgs> ChannelParted;

        /// <summary>
        /// Called after a channel was joined
        /// </summary>
        /// <remarks>
        /// The following commands are handled by this Method:
        /// <c>JOIN</c>
        /// </remarks>
        /// <param name="msg">the received irc message</param>
        [IrcCommand("JOIN")]
        protected virtual void OnChannelJoined(TmiMessage msg)
        {
            ChannelJoined?.Invoke(this, new ChannelConnectionChangedEventArgs(msg.Parameters[0]));
        }

        /// <summary>
        /// Called when the login process completes
        /// </summary>
        /// <remarks>
        /// The following commands are handled by this Method:
        /// <c>004</c>
        /// </remarks>
        /// <param name="msg">the message received</param>
        [IrcCommand("004")]
        protected virtual void OnLogin(TmiMessage msg)
        {
            Login?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called after a channel was left
        /// </summary>
        /// <remarks>
        /// The following commands are handled by this Method:
        /// <c>PART</c>
        /// </remarks>
        /// <param name="msg">the message received</param>
        [IrcCommand("PART")]
        protected virtual void OnChannelParted(TmiMessage msg)
        {
            ChannelParted?.Invoke(this, new ChannelConnectionChangedEventArgs(msg.Parameters[0]));
        }
    }
}
