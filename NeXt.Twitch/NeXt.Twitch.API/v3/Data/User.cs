using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace NeXt.Twitch.API.Data
{
    /// <summary>
    /// Class representing a User object returned from Twitch API.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]//used by Json.Net
    [SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]//Instantiated by Json.Net
    public sealed class User
    {
        [JsonConstructor]
        private User() { }

        /// <summary>
        /// Display name of user or null
        /// </summary>
        [JsonProperty("display_name")]
        public string DisplayName { get; private set; }

        /// <summary>
        /// Twitch Id of user.
        /// </summary>
        [JsonProperty("_id")]
        public int? Id { get; private set; }

        /// <summary>
        /// Username of user.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// Type of user assigned by Twitch.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; private set; }

        /// <summary>
        /// Bio of user.
        /// </summary>
        [JsonProperty("bio")]
        public string Bio { get; private set; }

        /// <summary>
        /// Date and time user was created at.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; private set; }

        /// <summary>
        /// Date and time user was last updated (logged in generally)
        /// </summary>
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; private set; }

        /// <summary>
        /// Url to logo of user.
        /// </summary>
        [JsonProperty("logo")]
        public string Logo { get; private set; }
    }
}
