using System.ComponentModel;

#pragma warning disable 1591
namespace NeXt.Twitch.Chat.Messages
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class MessageBuilder
    {
        public static string Timeout(string user, int duration, string reason)
        {
            return $".timeout {user} {duration} {reason}";
        }

        public static string Ban(string user, string reason)
        {
            return $".ban {user} {reason}";
        }

        public static string Unban(string user)
        {
            return $".unban {user}";
        }

        public static string Whisper(string sourceUser, string targetUser, string message)
        {
            return PrivMsg(sourceUser, "jtv", $"/w {targetUser} {message}");
        }

        public static string Pass(string password)
        {
            return $"PASS {password}";
        }

        public static string Nick(string nickname)
        {
            return $"NICK {nickname}";
        }
        
        public static string Join(string channel)
        {
            return $"JOIN {channel}";
        }
        
        public static string Part(string channel)
        {
            return $"PART {channel}";
        }

        public static string PrivMsg(string user, string channel, string text)
        {
            return $":{user}!{user}@{user}tmi.twitch.tv PRIVMSG #{channel} :{text}";
        }

        public static string Action(string user, string channel, string text)
        {
            return PrivMsg(user, channel, $"\u0001 {text}\u0001");
        }
    }
}
