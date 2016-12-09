using System;

namespace NeXt.Twitch.Chat
{
    /// <summary>
    /// Checks messages to uphold certain limits
    /// </summary>
    /// <seealso cref="DefaultMessageLimiter"/>
    public interface IMessageLimiter
    {
        /// <summary>
        /// Checks the throtteling limits of this limiter instance and if they are met
        /// updates the limit. 
        /// </summary>
        /// <returns>True if the message should be throttled, false otherwise</returns>
        bool Throttle();

        /// <summary>
        /// The amount of time remaining until another message can be sent through this Limiter
        /// </summary>
        TimeSpan Remaining { get; }
        
        /// <summary>
        /// Validates the message text, the message is droppped if it doesn't validate
        /// </summary>
        /// <param name="raw">the message to validate</param>
        /// <returns>true if the mesage validates, false otherwise</returns>
        bool Validate(string raw);
    }
}
