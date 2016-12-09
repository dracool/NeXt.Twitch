using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace NeXt.Twitch.Chat.Connection
{
    public partial class TmiConnection
    {
        private class ReceiveClient
        {
            // ReSharper disable once MemberHidesStaticFromOuterClass
            private readonly TmiConnection client;

            public ReceiveClient(TmiConnection owner)
            {
                client = owner;
            }

            /// <summary>
            /// The Task executing the receiving
            /// </summary>
            public Task Task { get; private set; }

            /// <summary>
            /// Starts the receiver
            /// </summary>
            /// <param name="token"></param>
            /// <param name="reader"></param>
            public void Start(CancellationToken token, TextReader reader)
            {
                Task = Task.Factory.StartNew(() => Run(token, reader), TaskCreationOptions.LongRunning);
            }
            
            /// <summary>
            /// runs the receiving end
            /// </summary>
            /// <param name="token"></param>
            /// <param name="reader"></param>
            private void Run(CancellationToken token, TextReader reader)
            {
                try
                {
                    while (!token.IsCancellationRequested)
                    {
                        var line = reader.ReadLine();
                        if (line == null) break;

                        // ReSharper disable once MethodSupportsCancellation (unncessary)
                        Task.Run(() => client.OnRawReceived(line));
                    }
                }
                catch (IOException e)
                {
                    if ((e.InnerException as SocketException)?.ErrorCode != 10004)
                        client.OnException(new ExceptionEventArgs(e));
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
        }
    }
}