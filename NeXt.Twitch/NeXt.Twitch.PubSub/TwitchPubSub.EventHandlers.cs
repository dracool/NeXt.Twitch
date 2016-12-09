using System;
using NeXt.Twitch.PubSub.EventArgs;

namespace NeXt.Twitch.PubSub
{
    public partial class TwitchPubSub
    {
        /// <summary>
        /// Signals that the PubSub System was connected
        /// <para>NOTE: You must listen to a topic within 15 seconds of this event or you will be disconnected</para>
        /// </summary>
        public event EventHandler Connected;

        /// <summary>
        /// Signals that the PubSub System was disconnected, reconnect happens automatically
        /// </summary>
        public event EventHandler Disconnected;

        /// <summary>
        /// Fired when the  pubsub client automatically pings the server
        /// </summary>
        public event EventHandler Ping;

        /// <summary>
        /// Fired when the pubsub client receives a pong response from the server
        /// </summary>
        public event EventHandler Pong;

        /// <summary>
        /// Signals that the clients status changed
        /// </summary>
        public event EventHandler<StatusChangingEventArgs> StatusChanging;

        /// <summary>
        /// Signals that an exception was thrown by the websocket connection
        /// </summary>
        public event EventHandler<ExceptionEventArgs> Error;

        /// <summary>
        /// Occurs when an unknown message was received
        /// </summary>
        public event EventHandler<UnknownMessageEventArgs> Unknown;

        /// <summary>
        /// Occurs when a response to a listen request was received
        /// </summary>
        public event EventHandler<ResponseMessageEventArgs> Response;

        /// <summary>
        /// Occurs when a  timeout message is received
        /// </summary>
        public event EventHandler<TimeoutMessageEventArgs> Timeout;

        /// <summary>
        /// Occurs when a ban message is received
        /// </summary>
        public event EventHandler<BanMessageEventArgs> Ban;

        /// <summary>
        /// Occurs when an unban message is received
        /// </summary>
        public event EventHandler<UnbanMessageEventArgs> Unban;

        /// <summary>
        /// Occurs when a host message is received
        /// </summary>
        public event EventHandler<HostMessageEventArgs> Host;

        /// <summary>
        /// Occurs when a bits message is received
        /// </summary>
        public event EventHandler<BitsMessageEventArgs> Bits;

        /// <summary>
        /// Occurs when a stream start message is received
        /// </summary>
        public event EventHandler<StreamStartMessageEventArgs> StreamStart;

        /// <summary>
        /// Occurs when a stream stop message is received
        /// </summary>
        public event EventHandler<StreamStopMessageEventArgs> StreamStop;

        /// <summary>
        /// Occurs when a viewer count message is received
        /// </summary>
        public event EventHandler<ViewerCountMessageEventArgs> ViewerCount;
    }
}