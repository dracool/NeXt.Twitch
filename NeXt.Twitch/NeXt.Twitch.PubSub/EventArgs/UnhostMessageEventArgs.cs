using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeXt.Twitch.PubSub.EventArgs
{
    /// <summary>
    /// Represents a host message from the PubSub system
    /// </summary>
    public class UnhostMessageEventArgs : ModeratorActionMessageEventArgs
    {
        /// <summary>
        /// Initializes a HostMessage from the pubsub system
        /// </summary>
        /// <param name="moderator">the moderator</param>
        /// <param name="topic">the topic</param>
        public UnhostMessageEventArgs(string moderator, string topic) : base(
            moderator: moderator,
            topic: topic) { }
    }
}
