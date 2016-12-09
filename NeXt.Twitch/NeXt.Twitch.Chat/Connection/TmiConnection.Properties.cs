using System;
using NeXt.Twitch.Common;

namespace NeXt.Twitch.Chat.Connection
{
    public partial class TmiConnection
    {
        private int queueSize = 16;
        private IReconnectStrategy reconnectStrategy;

        /// <summary>
        /// The maximum amount of items concurrently in queue
        /// </summary>
        public int QueueSize
        {
            get
            {
                ThrowIfDisposed();
                return queueSize;
            }
            set
            {
                ThrowIfDisposed();
                if (value <= 0) throw new InvalidOperationException("Queue size most not be <= 0");
                queueSize = value;
            }
        }

        /// <summary>
        /// The reconnect strategy used by the connection
        /// </summary>
        public IReconnectStrategy ReconnectStrategy
        {
            get
            {
                ThrowIfDisposed();
                return reconnectStrategy;
            }
            set
            {
                ThrowIfDisposed();
                reconnectStrategy = value;
            }
        }
    }
}