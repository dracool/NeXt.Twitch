using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using NeXt.Twitch.PubSub.Topics;

namespace NeXt.Twitch.PubSub
{
    public partial class TwitchPubSub
    {
        /// <summary>
        /// The random instance
        /// </summary>
        private static readonly Random Random = new Random();

        /// <summary>
        /// Generates a random string with <paramref name="length"/> alphanumeric characters
        /// </summary>
        /// <param name="length">the length of the generated string</param>
        /// <returns>the generated string</returns>
        private static string GenerateNonce(int length = 12)
        {
            return new string(Enumerable
                .Repeat("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", length)
                .Select(s => s[Random.Next(s.Length)])
                .ToArray()
            );
        }

        /// <summary>
        /// Starts listening to a topic
        /// </summary>
        /// <param name="topic">the topic to start listening to</param>
        public void Listen(PubSubTopic topic)
        {
            ThrowIfDisposed();
            if (topic == null) throw new ArgumentNullException(nameof(topic));
            SetListenToTopic(true, topic);
        }

        /// <summary>
        /// Stops listening to a topic
        /// </summary>
        /// <param name="topic">the topic to stop listening to</param>
        public void Unlisten(PubSubTopic topic)
        {
            ThrowIfDisposed();
            if (topic == null) throw new ArgumentNullException(nameof(topic));
            SetListenToTopic(false, topic);
        }

        private void SetListenToTopic(bool listen, PubSubTopic topic)
        {
            string nonce;
            do
            {
                nonce = GenerateNonce();
            } while (!parser.AddNonce(nonce, topic.Topic));

            var jObj = new JObject(
                new JProperty("type", listen ? "LISTEN" : "UNLISTEN"),
                new JProperty("nonce", nonce),
                new JProperty("data",
                    new JObject(
                        new JProperty("topics",
                            new JArray(
                                new JValue(topic.Topic)
                            )
                        )
                    )
                )
            );

            if (topic.IsAuthorized)
            {
                if (string.IsNullOrEmpty(topic.OAuthToken))
                    throw new ArgumentException("Topic cannot have null or empty token if it needs authorization",
                        nameof(topic));
                ((JObject) jObj.SelectToken("data")).Add(new JProperty("auth_token", topic.OAuthToken));
            }

            socket.Send(jObj.ToString());
        }
    }
}