using System;

namespace NeXt.Twitch.PubSub.EventArgs
{

    //JToken json = JObject.Parse(jsonStr);
    //Username = json.SelectToken("user_name")?.ToString();
    //ChannelName = json.SelectToken("channel_name")?.ToString();
    //UserId = json.SelectToken("user_id")?.ToString();
    //ChannelId = json.SelectToken("channel_id")?.ToString();
    //Time = json.SelectToken("time")?.ToString();
    //ChatMessage = json.SelectToken("chat_message")?.ToString();
    //BitsUsed = int.Parse(json.SelectToken("bits_used").ToString());
    //TotalBitsUsed = int.Parse(json.SelectToken("total_bits_used").ToString());
    //Context = json.SelectToken("context")?.ToString();

    /// <summary>
    /// Represents a bits message from the pubsub system
    /// </summary>
    public class BitsMessageEventArgs : PubSubMessageEventArgs
    {
        /// <summary>
        /// Initializes a BitsMessage from the pubsub system
        /// </summary>
        /// <param name="userName">the user name</param>
        /// <param name="channelName">the channel name</param>
        /// <param name="userId">the user id</param>
        /// <param name="channelId">the channel id</param>
        /// <param name="time">the timestamp</param>
        /// <param name="chatMessage">the chat message</param>
        /// <param name="bitsUsed">the amount of bits used</param>
        /// <param name="totalBitsUsed">the total amount of bits used</param>
        /// <param name="context">the context</param>
        /// <param name="topic">the topic</param>
        public BitsMessageEventArgs(string userName, string channelName, int userId, long channelId, DateTime time, string chatMessage, int bitsUsed, int totalBitsUsed, string context, string topic) : base(topic)
        {
            UserName = userName;
            ChannelName = channelName;
            UserId = userId;
            ChannelId = channelId;
            Time = time;
            ChatMessage = chatMessage;
            BitsUsed = bitsUsed;
            TotalBitsUsed = totalBitsUsed;
            Context = context;
        }

        /// <summary>
        /// Login name of the person who used Bits.
        /// </summary>
        public string UserName { get;}

        /// <summary>
        /// Channel name of where Bits were used.
        /// </summary>
        public string ChannelName { get; }

        /// <summary>
        /// ID of the user who used Bits.
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// ID of the channel where Bits were used.
        /// </summary>
        public long ChannelId { get; }

        /// <summary>
        /// Time of the event
        /// </summary>
        public DateTime Time { get; }

        /// <summary>
        /// Chat message sent with the Bits used.
        /// </summary>
        public string ChatMessage { get; }

        /// <summary>
        /// Number of Bits used
        /// </summary>
        public int BitsUsed { get; }

        /// <summary>
        /// All-time total number of Bits used on this channel by the user.
        /// </summary>
        public int TotalBitsUsed { get; }

        /// <summary>
        /// Event type associated with this Bits usage (e.g. cheer).
        /// </summary>
        public string Context { get;  }
    }
}
