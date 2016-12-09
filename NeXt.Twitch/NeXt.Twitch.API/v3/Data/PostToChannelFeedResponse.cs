using Newtonsoft.Json.Linq;

namespace NeXt.Twitch.API.Data
{
    //TODO: make use of json attributes without explicit constructor
    /// <summary>
    /// Represents a response from the Twitch API
    /// </summary>
    public class PostToChannelFeedResponse
    {
        /// <summary>
        /// Parses a post to channel feed response
        /// </summary>
        /// <param name="jsonData">the json token</param>
        public PostToChannelFeedResponse(JToken jsonData)
        {
            if (jsonData.SelectToken("tweet") != null)
                TweetUrl = jsonData.SelectToken("tweet").ToString();
            Post = new Post(jsonData.SelectToken("post"));
        }

        /// <summary>
        /// The Url of the Tweet
        /// </summary>
        public string TweetUrl { get; protected set; }

        /// <summary>
        /// The post
        /// </summary>
        public Post Post { get; protected set; }
    }
}
