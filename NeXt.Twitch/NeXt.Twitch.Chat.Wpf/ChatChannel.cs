using NeXt.Twitch.Chat.Connection;
using NeXt.Twitch.Chat.Messages;

namespace NeXt.Twitch.Chat.Wpf
{
    public class ChatChannel
    {
        private readonly TwitchChat chat;
        internal ChatChannel(TwitchChat chat, string name)
        {
            Name = name;
            this.chat = chat;
        }

        public string Name { get; }

        public void Leave()
        {
            chat.Part(Name);
        }

        internal void SendMessage(string text, MessagePriority priority = MessagePriority.Default)
        {
            chat.DoSend(MessageBuilder.PrivMsg(chat.UserName, Name, text), chat.ChatLimiter, priority);
        }
    }
}