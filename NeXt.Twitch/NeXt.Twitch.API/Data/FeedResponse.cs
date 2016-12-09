using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace NeXt.Twitch.API.Data
{
    //TODO: make use of json attributes without explicit constructor

    /// <summary>
    /// Class representing the response from fetching a channel feed via Twitch API
    /// </summary>
    public class FeedResponse
    {
        /// <summary>
        /// FeedResponse object constructor.
        /// </summary>
        public FeedResponse(JToken json)
        {
            if (json.SelectToken("_total") != null)
                Total = int.Parse(json.SelectToken("_total").ToString());
            Cursor = json.SelectToken("_cursor")?.ToString();
            Topic = json.SelectToken("_topic")?.ToString();
            Posts = new List<Post>();
            if (json.SelectToken("posts") != null)
                Posts = json
                    .SelectToken("posts")
                    .Select(t => new Post(t))
                    .ToList()
                    .AsReadOnly();
        }

        /// <summary>
        /// Property representing total posts in a channel's feed.
        /// </summary>
        public int Total { get; protected set; }

        /// <summary>
        /// Property representing cursor value used for pagination.
        /// </summary>
        public string Cursor { get; protected set; }

        /// <summary>
        /// Property representing the topic (likely internal).
        /// </summary>
        public string Topic { get; protected set; }

        /// <summary>
        /// Property representing a list of Post objects.
        /// </summary>
        public IReadOnlyList<Post> Posts { get; protected set; }
    }
}
