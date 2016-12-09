using System;

namespace NeXt.Twitch.Chat.Connection
{
    /// <summary>
    /// Represents an invalid message event
    /// </summary>
    public class InvalidMessageEventArgs : EventArgs
    {
        /// <summary>
        /// initializes a new instance
        /// </summary>
        /// <param name="raw">the raw text received</param>
        /// <param name="exception">the exception that was thrown</param>
        public InvalidMessageEventArgs(string raw, Exception exception)
        {
            Raw = raw;
            Exception = exception;
        }

        /// <summary>
        /// The Raw message
        /// </summary>
        public string Raw { get; }

        /// <summary>
        /// The exception that occured
        /// </summary>
        public Exception Exception { get; }
    }
}
