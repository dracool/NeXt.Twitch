using System;
using System.Threading;

namespace NeXt.Twitch.PubSub
{
    public partial class TwitchPubSub
    {
        private volatile int disposeSignaled;

        private void DisposeManaged()
        {
            pingTimer.Stop();
            pingTimer.Elapsed -= PingTimerOnElapsed;
            pingTimer.Dispose();

            pongTimeoutTimer.Stop();
            pongTimeoutTimer.Elapsed -= PongTimeoutTimerOnElapsed;
            pongTimeoutTimer.Dispose();

            DisposeSocket();
        }

        private void DisposeSocket()
        {
            if (socket == null) return;
            socket.Opened -= SocketOnOpened;
            socket.Error -= SocketOnError;
            socket.MessageReceived -= SocketOnMessageReceived;
            socket.Closed -= SocketOnClosed;
            socket.Dispose();
            socket = null;
        }

        /// <summary>
        /// Implements the IDisposable interface
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        private void ThrowIfDisposed()
        {
            if (disposeSignaled != 0)
            {
                throw new ObjectDisposedException(nameof(TwitchPubSub));
            }
        }

        /// <summary>
        /// handles the internal disposable pattern
        /// </summary>
        /// <param name="disposeManaged"></param>
        protected virtual void Dispose(bool disposeManaged)
        {
#pragma warning disable 420
            if (Interlocked.Exchange(ref disposeSignaled, 1) != 0) return;
#pragma warning restore 420

            if (disposeManaged)
            {
                DisposeManaged();
            }
        }
    }
}