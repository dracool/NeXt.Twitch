using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NeXt.Twitch.Chat.Wpf
{
    public class ChatUserCollection : NotifyCollectionBase<ChatUser>
    {
        private readonly ChatChannel channel;

        internal ChatUserCollection(ChatChannel owner)
        {
            channel = owner;
        }
    }
}