using System;

namespace NeXt.Twitch.Chat.Client
{
    /// <summary>
    /// Represents a change to a specific channels connection status
    /// </summary>
    public class ChannelConnectionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="channel">the channel name</param>
        public ChannelConnectionChangedEventArgs(string channel)
        {
            Channel = channel;
        }

        /// <summary>
        /// The name of the channel
        /// </summary>
        public string Channel { get; }
    }
}
