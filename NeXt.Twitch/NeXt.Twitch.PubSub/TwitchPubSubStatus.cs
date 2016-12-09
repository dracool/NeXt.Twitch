using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeXt.Twitch.PubSub
{
    /// <summary>
    /// Describes the current status of the <see cref="TwitchPubSub"/> client
    /// </summary>
    public enum TwitchPubSubStatus
    {
        /// <summary>
        /// Initial value, only set when the client was not used
        /// </summary>
        None,

        /// <summary>
        /// The client is currently connecting
        /// </summary>
        Connecting,

        /// <summary>
        /// The client is currently connected
        /// </summary>
        Connected,

        /// <summary>
        /// The client is currently disconnecting
        /// </summary>
        Disconnecting,

        /// <summary>
        /// The client is currently  disconnected
        /// </summary>
        Disconnected,

        /// <summary>
        /// The client is currently handling an unexpected disconnect
        /// </summary>
        Reconnecting,
    }
}
