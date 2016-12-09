using System.Collections.Specialized;
using System.ComponentModel;

namespace NeXt.Twitch.Chat.Wpf
{
    public class ChatChannelCollection : NotifyCollectionBase<ChatChannel>
    {
        private readonly TwitchChat chat;

        internal ChatChannelCollection(TwitchChat owner)
        {
            chat = owner;
        }

        public void Join(string name)
        {
            
        }
    }
}