using System;
using System.Threading;

namespace NeXt.Twitch.Chat.Connection
{
    public partial class TmiConnection
    {
        private volatile int disposeSignaled;

        private void DisposeManaged()
        {
            clientToken?.Dispose();
            clientToken = null;

            connectionToken?.Dispose();
            clientToken = null;

            sender?.Dispose();
            
            client?.Close();
            client = null;
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
                throw new ObjectDisposedException(nameof(TmiConnection));
            }
        }

        /// <summary>
        /// The dispose pattern internal disposer
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
