using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace NeXt.Twitch.API.Data
{
    /// <summary>
    /// Block object representing one blocked user.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    [SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]
    public sealed class Block
    {
        [JsonConstructor]
        private Block() { }

        /// <summary>
        /// String form of a datetime json object representing when the block was last updated.
        /// </summary>
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; private set; }

        /// <summary>
        /// User object of the user that has been blocked.
        /// </summary>
        [JsonProperty("user")]
        public User User { get; private set; }
    }
}
