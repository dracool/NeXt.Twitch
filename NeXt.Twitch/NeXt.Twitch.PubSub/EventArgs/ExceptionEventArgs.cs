using System;

namespace NeXt.Twitch.PubSub.EventArgs
{
    /// <summary>
    /// Represents an exception that was not handled in background code
    /// </summary>
    public class ExceptionEventArgs : System.EventArgs
    {
        /// <summary>
        /// Creates a new instance of <see cref="ExceptionEventArgs"/>
        /// </summary>
        /// <param name="e">The Exception</param>
        public ExceptionEventArgs(Exception e)
        {
            Exception = e;
        }

        /// <summary>
        /// The Exception that was not handled
        /// </summary>
        public Exception Exception { get; }
    }
}
