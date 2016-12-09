using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeXt.Twitch.PubSub.EventArgs
{
    /// <summary>
    /// Represents a status change in the <see cref="TwitchPubSub"/> client
    /// </summary>
    public class StatusChangingEventArgs : System.EventArgs
    { 
        /// <summary>
        /// Initializes an instance of the StatusChangedEventArgs
        /// </summary>
        /// <param name="oldStatus">the old status</param>
        /// <param name="newStatus">the new status</param>
        public StatusChangingEventArgs(TwitchPubSubStatus oldStatus, TwitchPubSubStatus newStatus)
        {
            Old = oldStatus;
            New = newStatus;
        }

        /// <summary>
        /// The previous status
        /// </summary>
        public TwitchPubSubStatus Old { get; }

        /// <summary>
        /// The new status
        /// </summary>
        public TwitchPubSubStatus New { get; }
    }
}
