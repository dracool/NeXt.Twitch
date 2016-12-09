using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using NeXt.Twitch.Chat.Messages;

namespace NeXt.Twitch.Chat.Connection
{
    /// <summary>
    /// Class representing a basic Twitch messaging interface connection. It handles connecting,
    /// disconnecting and reconnecting (if an appropriate <see cref="Common.IReconnectStrategy"/> is set).
    /// It automatically parses messages and forwards them to an appropriate message handler marked with the
    /// <see cref="IrcCommandAttribute"/> in a subclass.
    /// </summary>
    /// <remarks>
    /// This class is not intended for direct use. Use <see cref="Client.TmiClient"/> instead.
    /// If you need more fine grained control over all messages you can subclass this class.
    /// </remarks>
    /// <example>
    /// Subclasses the <see cref="TmiConnection"/> class and uses the <see cref="IrcCommandAttribute"/> to print
    /// the message text of any <c>"001"</c> irc command to the console.
    /// <code>
    /// public class TmiBasic : TmiConnection
    /// {
    ///     [IrcCommand("001")]
    ///     private void On001(IrcMessage message)
    ///     {
    ///         Console.WriteLine(message.Raw);
    ///     }
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="Client.TmiClient"/>
    /// <seealso cref="Common.IReconnectStrategy"/>
    /// <seealso cref="IrcCommandAttribute"/>
    public partial class TmiConnection : IDisposable
    {
        //sync objects for locking
        private readonly object sendSyncRoot = new object();

        //queues for sending messages
        private volatile PriorityQueue<QueuedMessage> sendQueue;

        /// <summary>
        /// The server to connect to
        /// </summary>
        public DnsEndPoint Server { get; set; }

        /// <summary>
        /// helper class used to handle sending of messages
        /// </summary>
        private readonly SendClient sender;

        /// <summary>
        /// helper class used to handle receiving of messages
        /// </summary>
        private readonly ReceiveClient receiver;

        /// <summary>
        /// helper class to interact with the current status
        /// </summary>
        private readonly Status status;

        /// <summary>
        /// The used tcp client
        /// </summary>
        private TcpClient client;

        /// <summary>
        /// cancellation source used when disconnecting by request
        /// </summary>
        private CancellationTokenSource connectionToken;

        /// <summary>
        /// cancellation source used when disconnecting unexpectedly
        /// </summary>
        private CancellationTokenSource clientToken;

        /// <summary>
        /// the task used to reconnect
        /// </summary>
        private Task reconnectingTask;

        /// <summary>
        /// Timer used to automatically send pings
        /// </summary>
        private readonly System.Timers.Timer pingTimer;
        
        /// <summary>
        /// initializes an instance of <see cref="TmiConnection"/> 
        /// </summary>
        /// <exception cref="InvalidOperationException">if invalid command handlers are defined on the used subclass</exception>
        public TmiConnection()
        {
            receiver = new ReceiveClient(this);
            sender = new SendClient(this);
            status = new Status(this);
            Parser = new TmiMessageParser();
            pingTimer = new System.Timers.Timer
            {
                AutoReset = true,
                Enabled = false,
                Interval = 4*60*1000
            };
            pingTimer.Elapsed += PingTimerOnElapsed;
            RegisterCommands();
        }

        /// <summary>
        /// initializes an instance of <see cref="TmiConnection"/> 
        /// </summary>
        /// <param name="endPoint">the EndPoint to connect to</param>
        /// <exception cref="InvalidOperationException">if invalid command handlers are defined on the used subclass</exception>
        public TmiConnection(DnsEndPoint endPoint) : this()
        {
            if (endPoint == null) throw new ArgumentNullException(nameof(endPoint));
            Server = endPoint;
        }

        private void PingTimerOnElapsed(object s, ElapsedEventArgs elapsedEventArgs)
        {
            Send("PONG :tmi.twitch.tv", null, MessagePriority.Critical);
        }

        /// <summary>
        /// Connects this client to the server
        /// </summary>
        /// <exception cref="InvalidOperationException">If the client is already connected or connecting</exception>
        public async Task ConnectAsync()
        {
            ThrowIfDisposed();
            if (!status.SetConditional( TwitchChatStatus.Connecting, old => 
                old == TwitchChatStatus.None  || old == TwitchChatStatus.Disconnected))
                throw new InvalidOperationException("Already connected");
            
            sendQueue = new PriorityQueue<QueuedMessage>();

            connectionToken = new CancellationTokenSource();

            await DoConnectUnsafeAsync();
            
            status.Set(TwitchChatStatus.Connected);
            OnConnected();
        }

        /// <summary>
        /// Connects to the server
        /// <para>WARNNING: This method does not do ANY safety checks, only call when in a known good state</para>
        /// </summary>
        private async Task DoConnectUnsafeAsync()
        {
            client = new TcpClient();
            
            await client.ConnectAsync(Server.Host, Server.Port);
            
            var stream = client.GetStream();
            clientToken = new CancellationTokenSource();
            
            var enc = new UTF8Encoding(false, false);
            var combinedSource = CancellationTokenSource.CreateLinkedTokenSource(connectionToken.Token, clientToken.Token);
            receiver.Start(combinedSource.Token, new StreamReader(stream, enc));
            sender.Start(combinedSource.Token, new StreamWriter(stream, enc));
            pingTimer.Start();
        }

        /// <summary>
        /// Disconnects from the client
        /// </summary>
        /// <exception cref="InvalidOperationException">If the client is not connected or reconnecting</exception>
        public async Task DisconnectAsync()
        {
            ThrowIfDisposed();
            TwitchChatStatus old;

            if(!status.ExchangeConditional(
                TwitchChatStatus.Disconnecting,
                out old,
                v => v == TwitchChatStatus.Connected || v == TwitchChatStatus.Reconnecting
                ))
                throw new InvalidOperationException("Not connected");

            connectionToken.Cancel();
            if (old == TwitchChatStatus.Reconnecting)
            {
                await reconnectingTask;
            }
            else
            {
                await DoDisconnectUnsafeAsync();
            }
            connectionToken.Dispose();
            connectionToken = null;
            status.Set(TwitchChatStatus.Disconnected);
            OnDisconnected();
        }
        
        /// <summary>
        /// Disconnects the client
        /// <para>WARNING: Does not do ANY sanity checks</para>
        /// </summary>
        private async Task DoDisconnectUnsafeAsync()
        {
            pingTimer.Stop();
            clientToken.Cancel();
            await sender.Task;

            client.Close();
            await receiver.Task;
            client = null;
            
            clientToken.Dispose();
            clientToken = null;
        }
        
        /// <summary>
        /// The internally executed action for reconnecting
        /// </summary>
        /// <param name="token">the cancellation token</param>
        private async Task<bool> DoReconnect(CancellationToken token)
        {
            await DoDisconnectUnsafeAsync();
            if (token.IsCancellationRequested) return false;
            try
            {
                await DoConnectUnsafeAsync();
            }
            catch (SocketException)
            {
                return false;
            }
            if (!token.IsCancellationRequested) return true;
            await DoDisconnectUnsafeAsync();
            return false;
        }
        
        /// <summary>
        /// Called by sender/receiver when they disconnect unexpectedly
        /// </summary>
        private void HandleUnexpectedDisconnect()
        {
            if (!status.Set(TwitchChatStatus.Reconnecting)) return;
            OnReconnecting();
            
            if (ReconnectStrategy == null)
            {
                return;
            }
            reconnectingTask = ReconnectStrategy
                .ExecuteAsync(DoReconnect, connectionToken.Token)
                .ContinueWith(t =>
                {
                    if (t.Result)
                    {
                        if (!status.SetConditional(TwitchChatStatus.Connected, s => s == TwitchChatStatus.Reconnecting))
                        {
                            DoDisconnectUnsafeAsync().Wait();
                        }
                        else
                        {
                            OnConnected();
                        }
                    }
                    else
                    {
                        if (connectionToken.IsCancellationRequested) return;
                        if (status.Set(TwitchChatStatus.Disconnected))
                        {
                            OnReconnectFailed();
                        }
                    }
                    reconnectingTask = null;
                });
        }

#if DEBUG
        public void SimulateReconnect()
        {
            HandleUnexpectedDisconnect();
        }
#endif
    }
}
