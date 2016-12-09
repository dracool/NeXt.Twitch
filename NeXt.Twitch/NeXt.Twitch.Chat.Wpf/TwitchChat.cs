using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NeXt.Twitch.Chat.Connection;
using NeXt.Twitch.Chat.Messages;
using NeXt.Twitch.Chat.Wpf.Annotations;

namespace NeXt.Twitch.Chat.Wpf
{
    public partial class TwitchChat : TmiConnection, INotifyObject
    {
        private readonly NotifyObjectHelper notify;

        public TwitchChat(IUserCredentials credentials)
        {
            Credentials = credentials;
            UserName = Credentials.UserName.ToLowerInvariant();
            notify = new NotifyObjectHelper(this);
            Channels = new ChatChannelCollection(this);
        }

        internal readonly string UserName;

        /// <summary>
        /// The currently joined channels
        /// </summary>
        public ChatChannelCollection Channels { get; }

        /// <summary>
        /// the users credentials
        /// </summary>
        /// <seealso cref="IUserCredentials"/>
        public IUserCredentials Credentials { get; }

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

        internal void DoSend(string text, IMessageLimiter limiter, MessagePriority priority = MessagePriority.Default)
        {
            Send(text, limiter, priority);
        }

        internal void Part(string channelName)
        {
            Send(MessageBuilder.Part(channelName), null);
        }
    }
}