using System;
using System.Timers;
using NeXt.Twitch.PubSub.EventArgs;
using WebSocket4Net;
using Newtonsoft.Json.Linq;
using NeXt.Twitch.Common;
using SuperSocket.ClientEngine;

namespace NeXt.Twitch.PubSub
{
    /// <summary>
    /// Connection with the Twitch PubSub system
    /// </summary>
    public partial class TwitchPubSub : IDisposable
    {
        /// <summary>
        /// Creates an instance of the <see cref="TwitchPubSub"/> class
        /// </summary>
        public TwitchPubSub()
        {
            parser = new MessageParser();

            pingTimer = new Timer
            {   
                //just under 2.5 minutes to account for ping or timing fluctiation
                //a short disconnect during ping time will also not kill the connection
                Interval = 2.48*60*1000,
                AutoReset = true,
                Enabled = false
            };
            pingTimer.Elapsed += PingTimerOnElapsed;
            
            pongTimeoutTimer = new Timer
            {
                //arbitrarily set at 30 seconds, could probably be much shorter
                Interval = 30 * 1000,
                AutoReset = false,
                Enabled = false
            };
            pongTimeoutTimer.Elapsed += PongTimeoutTimerOnElapsed;

            InitializeSocket();
        }

        private void InitializeSocket()
        {
            socket = new WebSocket("wss://pubsub-edge.twitch.tv")
            {
                EnableAutoSendPing = false,
            };
            socket.Opened += SocketOnOpened;
            socket.Error += SocketOnError;
            socket.MessageReceived += SocketOnMessageReceived;
            socket.Closed += SocketOnClosed;
        }
        
        private readonly Timer pongTimeoutTimer;
        private readonly Timer pingTimer;
        private readonly MessageParser parser;

        private WebSocket socket;
        private TwitchPubSubStatus status;

        /// <summary>
        /// The current status of this client
        /// </summary>
        public TwitchPubSubStatus Status
        {
            get
            {
                ThrowIfDisposed();
                return status;
            }
            private set
            {
                ThrowIfDisposed();
                if (value == status) return;
                StatusChanging?.Invoke(this, new StatusChangingEventArgs(status, value));
                status = value;
            }
        }

        /// <summary>
        /// The strategy used by this client to reconnect, null acts like <see cref="NoReconnectStrategy"/>
        /// </summary>
        public IReconnectStrategy ReconnectStrategy { get; set; }

        #region Internal event Handling
        private void PongTimeoutTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            pongTimeoutTimer.Stop();
            HandleUnexpectedDisconnect();
        }

        private void PingTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            SendPing();
            pongTimeoutTimer.Start();
            Ping?.Invoke(this, System.EventArgs.Empty);
        }

        private void SocketOnClosed(object sender, System.EventArgs e)
        {
            pingTimer.Stop();
            pongTimeoutTimer.Stop();

            if (Status != TwitchPubSubStatus.Disconnecting && HandleUnexpectedDisconnect()) return;

            Status = TwitchPubSubStatus.Disconnected;
            Disconnected?.Invoke(this, System.EventArgs.Empty);
        }

        private void SocketOnError(object sender, ErrorEventArgs errorEventArgs)
        {
            Status = TwitchPubSubStatus.Disconnected;
            Error?.Invoke(this, new ExceptionEventArgs(errorEventArgs.Exception));
        }

        private void SocketOnOpened(object sender, System.EventArgs eventArgs)
        {
            Status = TwitchPubSubStatus.Connected;
            pingTimer.Start();
            Connected?.Invoke(this, System.EventArgs.Empty);
        }

        private void SocketOnMessageReceived(object sender, MessageReceivedEventArgs messageReceivedEventArgs)
        {
            PubSubMessageEventArgs msg;
            var type = parser.Parse(messageReceivedEventArgs.Message, out msg);

            // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags (enum is not really using flags)
            if ((type & PubSubMessageType.Unknown) > 0)
            {
                Unknown?.Invoke(this, (UnknownMessageEventArgs)msg);
                return;
            }

            // ReSharper disable once SwitchStatementMissingSomeCases (handled by the if above)
            switch (type)
            {
                case PubSubMessageType.Response:
                    Response?.Invoke(this, (ResponseMessageEventArgs)msg);
                    break;
                case PubSubMessageType.Pong:
                    pongTimeoutTimer.Stop();
                    Pong?.Invoke(this, System.EventArgs.Empty);
                    break;
                case PubSubMessageType.Reconnect:
                    HandleReconnect();
                    break;
                case PubSubMessageType.Timeout:
                    Timeout?.Invoke(this, (TimeoutMessageEventArgs)msg);
                    break;
                case PubSubMessageType.Ban:
                    Ban?.Invoke(this, (BanMessageEventArgs)msg);
                    break;
                case PubSubMessageType.Unban:
                    Unban?.Invoke(this, (UnbanMessageEventArgs)msg);
                    break;
                case PubSubMessageType.Host:
                    Host?.Invoke(this, (HostMessageEventArgs)msg);
                    break;
                case PubSubMessageType.Bits:
                    Bits?.Invoke(this, (BitsMessageEventArgs)msg);
                    break;
                case PubSubMessageType.StreamStart:
                    StreamStart?.Invoke(this, (StreamStartMessageEventArgs)msg);
                    break;
                case PubSubMessageType.StreamStop:
                    StreamStop?.Invoke(this, (StreamStopMessageEventArgs)msg);
                    break;
                case PubSubMessageType.ViewerCount:
                    ViewerCount?.Invoke(this, (ViewerCountMessageEventArgs)msg);
                    break;
                default:
                    Error?.Invoke(this, new ExceptionEventArgs(new ArgumentOutOfRangeException()));
                    break;
            }
        } 
        #endregion

        /// <summary>
        /// Sends a twitch ping message to the server
        /// </summary>
        private void SendPing()
        {
            var data = new JObject(
                new JProperty("type", "PING")
            );
            pongTimeoutTimer.Start();
            socket.Send(data.ToString());
        }

        private bool HandleUnexpectedDisconnect()
        {
            Status = TwitchPubSubStatus.Reconnecting;
            return (ReconnectStrategy?.Execute(() =>
            {
                DisposeSocket();
                try
                {
                    InitializeSocket();
                    socket.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
            })).GetValueOrDefault(false);
        }
        
        /// <summary>
        /// Handles the reconnection process on twitch reconnect message
        /// </summary>
        private void HandleReconnect()
        {
            pingTimer.Stop();
            socket.Close();
            socket.Open();
        }

        /// <summary>
        /// Connects to the Twitch PubSub system
        /// <para>NOTE: If you do not listen to any topic within 15 seconds of the Connected event the connection will be closed</para>
        /// </summary>
        public void Connect()
        {
            ThrowIfDisposed();
            Status = TwitchPubSubStatus.Connecting;
            socket.Open();
        }

        /// <summary>
        /// Disconnects the TwitchPubSub client
        /// </summary>
        public void Disconnect()
        {
            ThrowIfDisposed();
            Status = TwitchPubSubStatus.Disconnecting;
            socket.Close();
        }

    }
}
