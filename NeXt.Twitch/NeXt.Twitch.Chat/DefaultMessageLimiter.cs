using System;

namespace NeXt.Twitch.Chat
{
    /// <summary>
    /// Default Message throttler for twitch chat
    /// </summary>
    /// <seealso cref="IMessageLimiter"/>
    public class DefaultMessageLimiter : IMessageLimiter
    {
        private readonly double rate;
        private readonly double per;
        private double allowance;
        private DateTime last;

        /// <summary>
        /// Initializes a message limiter
        /// </summary>
        /// <param name="messages">how many message can be sent in any given time frame</param>
        /// <param name="timeSpan">the time frame to use</param>
        public DefaultMessageLimiter(int messages, TimeSpan timeSpan)
        {
            if (messages <= 0) throw new ArgumentOutOfRangeException(nameof(messages));
            rate = messages;
            per = timeSpan.TotalSeconds;
            allowance = rate;
            last = DateTime.Now;
        }

        /// <inheritdoc/>
        public bool Throttle()
        {
            var current = DateTime.Now;
            var time = (current - last).TotalSeconds;
            last = current;
            
            allowance += time*(rate/per);
                
            if (allowance > rate)
            {
                allowance = rate;
            }

            if (allowance < 1) return true;
            allowance -= 1;
            return false;
        }

        /// <inheritdoc />
        public TimeSpan Remaining => TimeSpan.FromSeconds((1 - allowance) / (rate / per));

        /// <inheritdoc/>
        public bool Validate(string raw)
        {
            return !string.IsNullOrWhiteSpace(raw);
        }
    }
}
