using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Newtonsoft.Json.Linq;
using NeXt.Twitch.PubSub.EventArgs;

namespace NeXt.Twitch.PubSub
{
    /// <summary>
    /// Handles parsing of pubsub text responses to specialized PubSubMessage objects
    /// </summary>
    internal class MessageParser
    {
        /// <summary>
        /// Initializes an instance of the message parser
        /// </summary>
        public MessageParser()
        {
            nonceSync = new object();
            nonceDictionary = new Dictionary<string, string>();
        }

        /// <summary>
        /// used for locking on the dictionary
        /// </summary>
        private readonly object nonceSync;

        /// <summary>
        /// Used to hold nonces to resolve
        /// </summary>
        private readonly Dictionary<string, string> nonceDictionary;

        /// <summary>
        /// Resolves a nonce to a topic
        /// </summary>
        /// <param name="nonce">the nonce to resolve</param>
        /// <returns>the topic</returns>
        private string ResolveNonce(string nonce)
        {
            lock (nonceSync)
            {
                string topic;
                if (!nonceDictionary.TryGetValue(nonce, out topic)) throw new MissingNonceException("The Nonce was missing when resolved: ") { Nonce = nonce};
                nonceDictionary.Remove(nonce);
                return topic;
            }
        }

        /// <summary>
        /// Adds a nonce to the resolve list
        /// </summary>
        /// <param name="nonce">the nonce to add</param>
        /// <param name="topic">the topic of the nonce</param>
        public bool AddNonce(string nonce, string topic)
        {
            lock (nonceSync)
            {
                if (nonceDictionary.ContainsKey(nonce)) return false;
                nonceDictionary.Add(nonce, topic);
                return true;
            }
        }

        /// <summary>
        /// Parses a PubSub message from json
        /// </summary>
        /// <param name="json">the received unaltered json text</param>
        /// <param name="message">the resulting argument instance</param>
        /// <returns>the type of the returned argument instance</returns>
        public PubSubMessageType Parse(string json, out PubSubMessageEventArgs message)
        {
            var type = JObject.Parse(json).SelectToken("type")?.ToString();

            switch (type?.ToLowerInvariant())
            {
                case "response":
                    return ParseResponseType(json, out message);
                case "message":
                    return ParseMessageType(json, out message);
                case "pong":
                    message = null;
                    return PubSubMessageType.Pong;
                case "reconnect":
                    message = null;
                    return PubSubMessageType.Reconnect;
                default:
                    message = new UnknownMessageEventArgs(PubSubMessageType.UnknownType, json, string.Empty);
                    return PubSubMessageType.UnknownType;
            }
        }

        /// <summary>
        /// Parses pubsub messages of type <code>response</code>
        /// </summary>
        /// <param name="json">the full json text</param>
        /// <param name="message">the parsed message</param>
        /// <returns>the message type</returns>
        private PubSubMessageType ParseResponseType(string json, out PubSubMessageEventArgs message)
        {
            var jobj = JObject.Parse(json);

            message = new ResponseMessageEventArgs(
                jobj.SelectToken("error")?.ToString() ?? string.Empty,
                ResolveNonce(jobj.SelectToken("nonce")?.ToString() ?? string.Empty)
            );

            return PubSubMessageType.Response;
        }

        /// <summary>
        /// Parses pubsub messages of type <code>message</code>
        /// </summary>
        /// <param name="json">the full json</param>
        /// <param name="message">the parsed message</param>
        /// <returns>the message type</returns>
        private PubSubMessageType ParseMessageType(string json, out PubSubMessageEventArgs message)
        {
            var jt = JObject.Parse(json).SelectToken("data");

            var topic = jt.SelectToken("topic")?.ToString() ?? string.Empty;
            var encodedJsonMessage = jt.SelectToken("message").ToString();

            //topic and info are delimited by a .   
            var delim = topic.IndexOf(".", StringComparison.Ordinal);       

            switch (delim < 0 ? topic : topic.Substring(0, delim))
            {
                case "chat_moderator_actions": return ParseModeratorAction(encodedJsonMessage, topic, out message);
                case "channel-bitsevents": return ParseBits(encodedJsonMessage, topic, out message);
                case "video-playback":
                    return ParseVideoPlayback(encodedJsonMessage, topic, out message);
                default:
                    message = new UnknownMessageEventArgs(PubSubMessageType.UnknownMessage, json, topic);
                    return PubSubMessageType.UnknownMessage;
            }
        }
        
        private PubSubMessageType ParseModeratorAction(string json, string topic, out PubSubMessageEventArgs message)
        {
            var jt = JObject.Parse(json).SelectToken("data");

            var action = jt.SelectToken("moderation_action")?.ToString() ?? string.Empty;
            var issuer = jt.SelectToken("created_by").ToString();

            var args = jt.SelectToken("args").Select(token => token.ToString()).ToArray();

            switch (action.ToLowerInvariant())
            {
                case "timeout":
                    var timeoutReason = args.Length > 2 ? args[2] : string.Empty;
                    var duration = int.Parse(args[1]);
                    message = new TimeoutMessageEventArgs(issuer, args[0], duration, timeoutReason, topic);
                    return PubSubMessageType.Timeout;
                case "ban":
                    var banReason = args.Length > 1 ? args[1] : string.Empty;
                    message = new BanMessageEventArgs(issuer, args[0], banReason, topic);
                    return PubSubMessageType.Ban;
                case "unban":
                case "untimeout":
                    message = new UnbanMessageEventArgs(issuer, args[0], topic);
                    return PubSubMessageType.Unban;
                case "host":
                    message = new HostMessageEventArgs(args[0], issuer, topic);
                    return PubSubMessageType.Host;
                case "unhost":
                    message = new UnhostMessageEventArgs(issuer, topic);
                    return PubSubMessageType.Unhost;
                default:
                    message = new UnknownMessageEventArgs(PubSubMessageType.UnknownModeratorAction, json, topic);
                    return PubSubMessageType.UnknownModeratorAction;
            }
        }

        private PubSubMessageType ParseBits(string json, string topic, out PubSubMessageEventArgs message)
        {
            var jt = JObject.Parse(json);

            message = new BitsMessageEventArgs(
                userName: jt.SelectToken("user_name")?.ToString() ?? string.Empty,
                userId: int.Parse(jt.SelectToken("user_id")?.ToString() ?? "-1"),
                channelName: jt.SelectToken("channel_name")?.ToString() ?? string.Empty,
                channelId: long.Parse(jt.SelectToken("channel_id")?.ToString() ?? "-1"),
                time: Convert.ToDateTime(jt.SelectToken("time")?.ToString() ?? string.Empty),
                chatMessage: jt.SelectToken("chat_message")?.ToString() ?? string.Empty,
                bitsUsed: (jt.SelectToken("bits_used")?.ToObject<int>()).GetValueOrDefault(-1),
                totalBitsUsed: (jt.SelectToken("total_bits_used")?.ToObject<int>()).GetValueOrDefault(-1),
                context: jt.SelectToken("context")?.ToString() ?? string.Empty,
                topic: topic
            );
            return PubSubMessageType.Bits;
        }

        private PubSubMessageType ParseVideoPlayback(string json, string topic, out PubSubMessageEventArgs message)
        {
            var jt = JObject.Parse(json);

            var serverTime = jt.SelectToken("server_time")?.ToString() ?? string.Empty;

            switch (jt.SelectToken("type").ToString())
            {
                case "stream-up":
                    var delay = jt.SelectToken("play_delay").ToObject<int>();
                    message = new StreamStartMessageEventArgs(delay, serverTime, topic);
                    return PubSubMessageType.StreamStart;
                case "stream-down":
                    delay = jt.SelectToken("play_delay").ToObject<int>();
                    message = new StreamStopMessageEventArgs(delay, serverTime, topic);
                    return PubSubMessageType.StreamStop;
                case "viewcount":
                    var count = jt.SelectToken("viewers").ToObject<int>();
                    message = new ViewerCountMessageEventArgs(count, serverTime, topic);
                    return PubSubMessageType.ViewerCount;
                default:
                    message = new UnknownMessageEventArgs(PubSubMessageType.UnknownVideoPlayback, json, topic);
                    return PubSubMessageType.UnknownVideoPlayback;
            }
        }
    }
}
