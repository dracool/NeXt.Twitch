using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NeXt.Twitch.API.Data
{
    //TODO: fix these classes to use json attributes without explicit constructor

    /// <summary>
    /// Class representing Post object found in FeedResponse
    /// </summary>
    //[JsonObject(MemberSerialization.OptIn)]
    //[SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]
    //[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    public class Post
    {
        /// <summary>Post object constructor</summary>
        public Post(JToken json)
        {
            Id = json.SelectToken("id")?.ToString();
            CreatedAt = json.SelectToken("created_at")?.ToString();
            if (json.SelectToken("deleted") != null)
                Deleted = json.SelectToken("deleted").ToString().ToLower() == "true";
            Body = json.SelectToken("body")?.ToString();
            Emotes = new List<Emote>();
            if (json.SelectToken("emotes") != null)
                foreach (var emote in json.SelectToken("emotes"))
                    Emotes.Add(new Emote(emote));
            Reactions = new List<Reaction>();
            if (json.SelectToken("reactions") != null)
                foreach (var reaction in json.SelectToken("reactions"))
                    Reactions.Add(new Reaction(reaction));
            if (json.SelectToken("user") != null)
                User = json.SelectToken("user").ToObject<User>();
            Comments = new List<Comment>();
            var comments = json.SelectToken("comments");
            if (comments.SelectToken("_total") != null)
                CommentsTotal = int.Parse(comments.SelectToken("_total").ToString());
            CommentsCursor = comments.SelectToken("_cursor")?.ToString();
            if (comments.SelectToken("comments") != null)
                foreach (var comment in comments.SelectToken("comments"))
                    Comments.Add(new Comment(comment));
            Permissions = new List<KeyValuePair<string, bool>>();
            if (json.SelectToken("permissions") != null)
            {
                var permissions = json.SelectToken("permissions");
                if (permissions.SelectToken("can_reply") != null)
                    Permissions.Add(new KeyValuePair<string, bool>("can_reply", permissions.SelectToken("can_reply").ToString() == "true"));
                if (permissions.SelectToken("can_moderate") != null)
                    Permissions.Add(new KeyValuePair<string, bool>("can_moderate", permissions.SelectToken("can_moderate").ToString() == "true"));
                if (permissions.SelectToken("can_delete") != null)
                    Permissions.Add(new KeyValuePair<string, bool>("can_delete", permissions.SelectToken("can_delete").ToString() == "true"));
            }
        }

        /// <summary>
        /// Property representing Id of post.
        /// </summary>
        public string Id { get; protected set; }

        /// <summary>
        /// Property representing date time string of post creation.
        /// </summary>
        public string CreatedAt { get; protected set; }

        /// <summary>
        /// Property representing whether or not post was deleted.
        /// </summary>
        public bool Deleted { get; protected set; }

        /// <summary>
        /// Property representing the body of the post.
        /// </summary>
        public string Body { get; protected set; }

        /// <summary>
        /// Property representing list of Emote objects.
        /// </summary>
        public List<Emote> Emotes { get; protected set; }

        /// <summary>
        /// Property representing list of reaction objects.
        /// </summary>
        public List<Reaction> Reactions { get; protected set; }

        /// <summary>
        /// Property representing User object.
        /// </summary>
        public User User { get; protected set; }

        /// <summary>Property representing total number of comments.</summary>
        public int CommentsTotal { get; protected set; }

        /// <summary>Proeprty representing comments cursor used for pagination.</summary>
        public string CommentsCursor { get; protected set; }

        /// <summary>Property representing list of Comment objects.</summary>
        public List<Comment> Comments { get; protected set; }

        /// <summary>Property representing the list of permissions of the post fetch.</summary>
        public List<KeyValuePair<string, bool>> Permissions { get; protected set; }

        /// <summary>Class representing an emote found in a comment or post.</summary>
        public class Emote
        {
            /// <summary>Emote object construcotr.</summary>
            public Emote(JToken json)
            {
                if (json.SelectToken("id") != null)
                    Id = int.Parse(json.SelectToken("id").ToString());
                if (json.SelectToken("start") != null)
                    Start = int.Parse(json.SelectToken("start").ToString());
                if (json.SelectToken("end") != null)
                    End = int.Parse(json.SelectToken("end").ToString());
                if (json.SelectToken("set") != null)
                    Set = int.Parse(json.SelectToken("set").ToString());
            }

            /// <summary>Property representing Id of emote.</summary>
            public int Id { get; protected set; }

            /// <summary>Property representing start index of emote.</summary>
            public int Start { get; protected set; }

            /// <summary>Property representing end index of emote.</summary>
            public int End { get; protected set; }

            /// <summary>Property representing the emote set id.</summary>
            public int Set { get; protected set; }
        }

        /// <summary>Class representing a reaction as towards a post.</summary>
        public class Reaction
        {
            /// <summary>Reaction object constructor.</summary>
            public Reaction(JToken json)
            {
                json = json.First;
                Emote = json.SelectToken("emote")?.ToString();
                if (json.SelectToken("count") != null)
                    Count = int.Parse(json.SelectToken("count").ToString());
            }

            /// <summary>Property representing the emote of the reaciton.</summary>
            public string Emote { get; protected set; }

            /// <summary>Property representing the number of people using reaction.</summary>
            public int Count { get; protected set; }
        }

        /// <summary>
        /// Class representing comments found on a post.
        /// </summary>
        [JsonObject(MemberSerialization.OptIn)]
        public sealed class Comment
        {
            /// <summary>Comment object constructor.</summary>
            public Comment(JToken json)
            {
                if (json.SelectToken("id") != null)
                    Id = int.Parse(json.SelectToken("id").ToString());
                CreatedAt = json.SelectToken("created_at")?.ToString();
                if (json.SelectToken("deleted") != null)
                    Deleted = json.SelectToken("deleted").ToString().ToLower() == "true";
                Body = json.SelectToken("body")?.ToString();
                Permissions = new List<KeyValuePair<string, bool>>();
                if (json.SelectToken("permissions") != null)
                {
                    var permissions = json.SelectToken("permissions");
                    if (permissions.SelectToken("can_delete") != null)
                        Permissions.Add(new KeyValuePair<string, bool>("can_delete", permissions.SelectToken("can_delete").ToString().ToLower() == "true"));
                }
                Emotes = new List<Emote>();
                if (json.SelectToken("emotes") != null)
                    foreach (var emote in json.SelectToken("emotes"))
                        Emotes.Add(new Emote(emote));
                Reactions = new List<Reaction>();
                if (json.SelectToken("reactions") != null)
                    foreach (var reaction in json.SelectToken("reactions"))
                        Reactions.Add(new Reaction(reaction));
                if (json.SelectToken("user") != null)
                    User = json.SelectToken("user").ToObject<User>();
            }

            /// <summary>
            /// Property representing the Id of the comment.
            /// </summary>
            [JsonProperty("id")]
            public int Id { get; private set; }

            /// <summary>
            /// Property representing the date time of the comment creation.
            /// </summary>
            [JsonProperty("created_at")]
            public string CreatedAt { get; private set; }

            /// <summary>
            /// Property representing whether or not the comment was deleted.
            /// </summary>
            [JsonProperty("deleted")]
            public bool Deleted { get; private set; }

            /// <summary>
            /// Property representing the body of the comment.
            /// </summary>
            [JsonProperty("body")]
            public string Body { get; private set; }

            /// <summary>
            /// Property representing the list of permissions assigned to comment fetch.
            /// </summary>
            [JsonProperty("")]
            public List<KeyValuePair<string, bool>> Permissions { get; private set; }

            /// <summary>
            /// Property representing the list of Emote objects.
            /// </summary>
            [JsonProperty("")]
            public List<Emote> Emotes { get; private set; }

            /// <summary>
            /// Property representing the list of Reaction objects.
            /// </summary>
            [JsonProperty("")]
            public List<Reaction> Reactions { get; private set; }

            /// <summary>
            /// Property representing the commenter.
            /// </summary>
            [JsonProperty("")]
            public User User { get; private set; }
        }
    }
}
