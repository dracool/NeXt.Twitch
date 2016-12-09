using System;
using NeXt.Twitch.Chat.Connection;
using NeXt.Twitch.Chat.Messages;

namespace NeXt.Twitch.Chat.Wpf
{
    public class ChatUser
    {
        private readonly TwitchChat chat;
        private readonly ChatChannel channel;

        internal ChatUser(string name, string displayName, TwitchChat chat ,ChatChannel chan)
        {
            Name = name;
            DisplayName = displayName;
            channel = chan;
            this.chat = chat;
        }

        public string Name { get; private set; }
        public string DisplayName { get; private set; }

        public void Ban(string reason = "")
        {
            channel.SendMessage(MessageBuilder.Ban(Name, reason), MessagePriority.High);
        }

        public void Timeout(int secondsDuration, string reason = "")
        {
            channel.SendMessage(MessageBuilder.Timeout(Name, secondsDuration, reason), MessagePriority.High);
        }

        public void Timeout(TimeSpan duration)
        {
            Timeout((int)duration.TotalSeconds);
        }

        public void Unban()
        {
            
        }

        public void Untimeout()
        {
            
        }

        public void Whisper(string text)
        {
            
        }
    }
}