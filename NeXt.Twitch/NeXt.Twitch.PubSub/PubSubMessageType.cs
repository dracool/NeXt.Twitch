using NeXt.Twitch.PubSub.EventArgs;

namespace NeXt.Twitch.PubSub
{
    /// <summary>
    /// List of known message types
    /// </summary>
    public enum PubSubMessageType
    {
        //this specific setup for unknown values allows logical or bitwise comparison with
        //specific unknown flags and bitwise comparison with the general unknown flag

        /// <summary>
        /// The message is unknown
        /// <para>NOTE: ONLY COMPARE BITWISE TO THIS VALUE</para>
        /// </summary>
        Unknown                    = (0 << 0) + 1, // 00001
        
        /// <summary>
        /// the message is a moderator action but the action is unknown
        /// </summary>
        UnknownModeratorAction     = (1 << 1) + 1, // 00011

        /// <summary>
        /// The message is a video playback, but the specific type is unknown
        /// </summary>
        UnknownVideoPlayback       = (1 << 2) + 1, // 00101

        /// <summary>
        /// The message is of type <code>message</code> but the topic type is unknown
        /// </summary>
        UnknownMessage             = (1 << 3) + 1, // 01001

        /// <summary>
        /// The message had an unknown type
        /// </summary>
        UnknownType                = (1 << 4) + 1, // 10001

        /// <summary>
        /// The message is a response message of type <see cref="ResponseMessageEventArgs"/>
        /// </summary>
        Response = (1 << 5) * 1,

        /// <summary>
        /// The message is null
        /// </summary>
        Pong = (1 << 5) * 2,

        /// <summary>
        /// The message is null
        /// </summary>
        Reconnect = (1 << 5) * 3,

        /// <summary>
        /// The message is a timeout message of type <see cref="TimeoutMessageEventArgs"/>
        /// </summary>
        Timeout = (1 << 5) * 4,

        /// <summary>
        /// The message is a timeout message of type <see cref="BanMessageEventArgs"/>
        /// </summary>
        Ban = (1 << 5) * 5,

        /// <summary>
        /// The message is a timeout message of type <see cref="UnbanMessageEventArgs"/>
        /// </summary>
        Unban = (1 << 5) * 6,

        /// <summary>
        /// The message is a timeout message of type <see cref="HostMessageEventArgs"/>
        /// </summary>
        Host = (1 << 5) * 7,

        /// <summary>
        /// The message is a timeout message of type <see cref="BitsMessageEventArgs"/>
        /// </summary>
        Bits = (1 << 5) * 8,

        /// <summary>
        /// The message is a timeout message of type <see cref="StreamStartMessageEventArgs"/>
        /// </summary>
        StreamStart = (1 << 5) * 9,

        /// <summary>
        /// The message is a timeout message of type <see cref="StreamStopMessageEventArgs"/>
        /// </summary>
        StreamStop = (1 << 5) * 10,

        /// <summary>
        /// The message is a timeout message of type <see cref="ViewerCountMessageEventArgs"/>
        /// </summary>
        ViewerCount = (1 << 5) * 11,

        /// <summary>
        /// The message is a unhost message of  type <see cref="UnhostMessageEventArgs"/>
        /// </summary>
        Unhost = (1 << 5) * 12,

        //Whisper, //unsupported
    }
}
