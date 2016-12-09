using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NeXt.Twitch.Chat.Connection
{
    public partial class TmiConnection
    {
        private sealed class SendClient : IDisposable
        {
            private readonly AutoResetEvent signal;
            private readonly TmiConnection client;

            public SendClient(TmiConnection owner)
            {
                client = owner;
                signal = new AutoResetEvent(false);
            }

            public void Start(CancellationToken token, TextWriter writer)
            {
                ThrowIfDisposed();
                Task = Task.Factory.StartNew(() => Run(token, writer), TaskCreationOptions.LongRunning);
            }
            
            private Task task;

            public Task Task
            {
                get
                {
                    ThrowIfDisposed();
                    return task;
                }
                private set { task = value; }
            }
            
            public void Signal()
            {
                ThrowIfDisposed();
                signal.Set();
            }

            /// <summary>
            /// Safely dequeues an item from the queue if there are any
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
            private bool TryRead(out QueuedMessage message)
            {
                if (client.sendQueue.Count > 0)
                {
                    lock (client.sendSyncRoot)
                    {
                        return client.sendQueue.TryDequeue(out message);
                    }
                }
                message = default(QueuedMessage);
                return false;
            }
            
            private void Run(CancellationToken token, TextWriter writer)
            {
                try
                {
                    var isSignalled = false;
                    while (!token.IsCancellationRequested)
                    {
                        while (client.sendQueue.Count > 0)
                        {
                            if (token.IsCancellationRequested) break;

                            QueuedMessage message;
                            if (!TryRead(out message)) break;

                            if (message.Limit?.Throttle() ?? false)
                            {
                                message.Priority--;
                                var rem = message.Limit.Remaining;
                                if (rem > TimeSpan.Zero)
                                {
                                    // ReSharper disable once MethodSupportsCancellation
                                    Task.Delay(rem, token)
                                        .ContinueWith(t =>
                                        {
                                            if(t.IsFaulted || t.IsCanceled || token.IsCancellationRequested) return;
                                            lock (client.sendSyncRoot)
                                            {
                                                client.sendQueue.Enqueue(message);
                                            }
                                            Signal();
                                        });
                                }
                                else
                                {
                                    lock (client.sendSyncRoot)
                                    {
                                        client.sendQueue.Enqueue(message);
                                    }
                                }
                            }
                            else
                            {
                                writer.WriteLine(message.Message);
                            }
                        }
                        writer.Flush();
                        
                        while (!token.IsCancellationRequested && !isSignalled)
                        {
                            if (signal.WaitOne(100)) isSignalled = true;
                        }
                        isSignalled = false;
                    }
                }
                catch (Exception e)
                {
                    client.OnException(new ExceptionEventArgs(e));
                }
                finally
                {
                    if (!token.IsCancellationRequested)
                    {
                        // ReSharper disable once MethodSupportsCancellation
                        Task.ContinueWith(t => client.HandleUnexpectedDisconnect());
                    }
                }
            }

            private volatile int disposeSignaled;

            private void DisposeManaged()
            {
                signal?.Dispose();
            }

            public void Dispose()
            {
                Dispose(true);
            }

            private void ThrowIfDisposed()
            {
                if (disposeSignaled != 0)
                {
                    throw new ObjectDisposedException(nameof(SendClient));
                }
            }

            private void Dispose(bool disposeManaged)
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
}