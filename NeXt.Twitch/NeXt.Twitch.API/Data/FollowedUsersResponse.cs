using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace NeXt.Twitch.API.Data
{
    /// <summary>
    /// Represents API response from user's followed list.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]//used by Json.Net
    [SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]//Instantiated by Json.Net
    public sealed class FollowedUsersResponse
    {
        [JsonConstructor]
        private FollowedUsersResponse() { }

        /// <summary>
        /// All follows returned in the api request.
        /// </summary>
        [JsonProperty("follows")]
        public IReadOnlyList<Follow> Follows { get; private set; }

        /// <summary>
        /// Total follow count.
        /// </summary>
        [JsonProperty("_total")]
        public int TotalFollowCount { get; private set; }
    }
}
