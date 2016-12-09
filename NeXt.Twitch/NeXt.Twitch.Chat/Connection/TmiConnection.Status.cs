using System;

namespace NeXt.Twitch.Chat.Connection
{
    public partial class TmiConnection
    {
        private class Status
        {
            // ReSharper disable once MemberHidesStaticFromOuterClass
            public Status(TmiConnection owner)
            {
                client = owner;
            }

            private readonly TmiConnection client;

            private readonly object sync = new object();
            
            private TwitchChatStatus value = TwitchChatStatus.None;

            public bool Set(TwitchChatStatus newValue)
            {
                StatusChangedEventArgs args;
                lock (sync)
                {
                    if (value == newValue) return false;
                    args = new StatusChangedEventArgs(value, newValue);
                    value = newValue;
                }
                client.OnStatusChanged(args);
                return true;
            }

            public bool SetConditional(TwitchChatStatus newValue,
                Func<TwitchChatStatus, bool> condition)
            {
                StatusChangedEventArgs args;
                lock (sync)
                {
                    if (!condition(value)) return false;
                    args = new StatusChangedEventArgs(value, newValue);
                    value = newValue;
                }
                client.OnStatusChanged(args);
                return true;
            }

            public bool ExchangeConditional(TwitchChatStatus newValue, out TwitchChatStatus old,
                Func<TwitchChatStatus, bool> condition)
            {
                old = value;
                StatusChangedEventArgs args;
                lock (sync)
                {
                    if (!condition(value)) return false;
                    args = new StatusChangedEventArgs(value, newValue);
                    value = newValue;
                }
                client.OnStatusChanged(args);
                return true;
            }
        }
    }
}