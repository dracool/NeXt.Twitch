using System;

namespace NeXt.Twitch.Chat.Connection
{
    public partial class TmiConnection
    {
        /// <summary>
        /// Sends a message to the queue
        /// </summary>
        /// <param name="raw">the message text</param>
        /// <param name="limiter">the message limiter to use with this message, can be null if no limits are in place</param>
        /// <param name="priority">the priority the message has</param>
        /// <example>
        /// A client that automatically replies to pings with an appropriate pong, 
        /// the pong message has critical priority and is not limited or validated
        /// <code>
        /// public class TmiPing : TmiConnection
        /// {
        ///     [IrcCommand("PING")]
        ///     protected virtual void OnPing(IrcMessage msg)
        ///     {
        ///         Send($"PONG {msg.RawPrefix}", null, MessagePriority.Critical);
        ///     }
        /// }
        /// </code>
        /// </example>
        protected void Send(string raw, IMessageLimiter limiter ,MessagePriority priority = MessagePriority.Default)
        {
            if (!limiter?.Validate(raw) ?? false)
            {
                MessageDropped?.Invoke(this, new MessageDroppedEventArgs(raw, MessageDropReason.Validation));
                return;
            }
            if (sendQueue.Count >= QueueSize)
            {
                MessageDropped?.Invoke(this, new MessageDroppedEventArgs(raw, MessageDropReason.QueueOverflow));
                return;
            }
            lock (sendSyncRoot)
            {
                sendQueue.Enqueue(new QueuedMessage(raw, priority, limiter));
                sender.Signal();
            }
        }
        
        private struct QueuedMessage : IComparable<QueuedMessage>
        {
            public QueuedMessage(string msg, MessagePriority priority, IMessageLimiter limit)
            {
                Message = msg;
                Priority = (int)priority;
                Limit = limit;
            }

            // ReSharper disable once FieldCanBeMadeReadOnly.Local
            public int Priority;
            // ReSharper disable once FieldCanBeMadeReadOnly.Local
            public string Message;
            // ReSharper disable once FieldCanBeMadeReadOnly.Local
            public IMessageLimiter Limit;

            public int CompareTo(QueuedMessage other)
            {
                return Priority - other.Priority;
            }
        }
    }
}