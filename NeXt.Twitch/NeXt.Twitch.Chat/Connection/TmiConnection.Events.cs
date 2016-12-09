using System;
using NeXt.Twitch.Chat.Client;

namespace NeXt.Twitch.Chat.Connection
{
    public partial class TmiConnection
    {
        /// <summary>
        /// Fired when the client finished connecting.
        /// </summary>
        public event EventHandler Connected;

        /// <summary>
        /// Fired when the client finished disconnecting by request.
        /// </summary>
        public event EventHandler Disconnected;

        /// <summary>
        /// Fired when the client starts reconnecting.
        /// </summary>
        public event EventHandler Reconnecting;

        /// <summary>
        /// Fired when an unexpected disconnected happened and reconnecting failed.
        /// </summary>
        public event EventHandler ReconnectFailed;

        /// <summary>
        /// Fired when the connection status is changing.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        /// <summary>
        /// Fired when a message was dropped by the connection.
        /// </summary>
        public event EventHandler<MessageDroppedEventArgs> MessageDropped;

#pragma warning disable 67 //(event is never invoked) seems visual studio is confused if the invocation is in a different file using ?.
        /// <summary>
        /// Fired when a raw message was received by the connection.
        /// </summary>
        public event EventHandler<string> RawReceived;

        /// <summary>
        /// Fired when a message that could not be parsed was received.
        /// </summary>
        public event EventHandler<InvalidMessageEventArgs> Invalid;

        /// <summary>
        /// Fired when a message that was not handled was received.
        /// </summary>
        public event EventHandler<UnknownMessageEventArgs> Unknown;
#pragma warning restore 67

        /// <summary>
        /// Fired when an unexpected exception occurs in the read or write threads
        /// </summary>
        public event EventHandler<ExceptionEventArgs> Exception;

        /// <summary>
        /// Called when the client finished connecting.
        /// </summary>
        protected virtual void OnConnected()
        {
            Connected?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when the client finished disconnecting.
        /// </summary>
        protected virtual void OnDisconnected()
        {
            Disconnected?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when the client starts reconnecting.
        /// </summary>
        protected virtual void OnReconnecting()
        {
            Reconnecting?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when the client permanently stopped reconnecting.
        /// </summary>
        protected virtual void OnReconnectFailed()
        {
            ReconnectFailed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when the status changed.
        /// </summary>
        /// <param name="e">the event arguments.</param>
        protected virtual void OnStatusChanged(StatusChangedEventArgs e)
        {
            StatusChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Called when a message was dropped.
        /// </summary>
        /// <param name="e">the event arguments.</param>
        protected virtual void OnMessageDropped(MessageDroppedEventArgs e)
        {
            MessageDropped?.Invoke(this, e);
        }

        /// <summary>
        /// Called when an unexpected Exception was thrown in the read or write thread
        /// </summary>
        /// <param name="e">the exception that was thrown</param>
        protected virtual void OnException(ExceptionEventArgs e)
        {
            Exception?.Invoke(this, e);
        }
    }
}