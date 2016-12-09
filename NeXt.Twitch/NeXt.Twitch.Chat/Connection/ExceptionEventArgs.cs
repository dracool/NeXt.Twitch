using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeXt.Twitch.Chat.Connection
{
    /// <summary>
    /// Holds an Exception that was thrown in the read or write threads of the <see cref="TmiConnection"/>.
    /// </summary>
    public class ExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance of <see cref="ExceptionEventArgs"/>.
        /// </summary>
        /// <param name="e">The Exception</param>
        public ExceptionEventArgs(Exception e)
        {
            Exception = e;
        }

        /// <summary>
        /// The Exception that was caused
        /// </summary>
        public Exception Exception { get; }
    }
}
