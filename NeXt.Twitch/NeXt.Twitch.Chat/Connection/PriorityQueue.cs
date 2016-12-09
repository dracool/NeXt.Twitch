using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace NeXt.Twitch.Chat.Connection
{
    /// <summary>
    /// Implements a priority queue ordered by the <see cref="IComparable{T}"/> implementation
    /// </summary>
    /// <typeparam name="T">the type to hold</typeparam>
    internal class PriorityQueue<T> where T : IComparable<T>
    {
        [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
        private static long _id = long.MinValue;

        //the default size for queues
        private const int DefaultCapacity = 16;

        //the equality comparer used for remove
        private readonly IEqualityComparer<T> equalityComparer;

        private QueueItem[] items;
        private int size;

        /// <summary>
        /// Initializes a priority queue instance
        /// </summary>
        public PriorityQueue()
            : this(DefaultCapacity) { }

        /// <summary>
        /// Initializes a priority queue instance
        /// </summary>
        /// <param name="capacity">the start capacity of the queue</param>
        public PriorityQueue(int capacity)
            : this(capacity, EqualityComparer<T>.Default) { }

        /// <summary>
        /// Initializes a priority queue instance
        /// </summary>
        /// <param name="comparer">the comparer to use when removing</param>
        public PriorityQueue(IEqualityComparer<T> comparer)
            : this(DefaultCapacity, comparer) { }

        /// <summary>
        /// Initializes a priority queue instance
        /// </summary>
        /// <param name="capacity">the start capacity of the queue</param>
        /// <param name="comparer">the comparer to use when removing</param>
        public PriorityQueue(int capacity, IEqualityComparer<T> comparer)
        {
            items = new QueueItem[capacity];
            equalityComparer = comparer;
            size = 0;
        }

        private bool IsHigherPriority(int left, int right)
        {
            return items[left].CompareTo(items[right]) < 0;
        }

        private void Percolate(int index)
        {
            while (true)
            {
                if (index >= size || index < 0)
                    return;
                var parent = (index - 1)/2;
                if (parent < 0 || parent == index)
                    return;

                if (IsHigherPriority(index, parent))
                {
                    var temp = items[index];
                    items[index] = items[parent];
                    items[parent] = temp;
                    index = parent;
                    continue;
                }
                break;
            }
        }

        private void Heapify(int index = 0)
        {
            while (true)
            {
                if (index >= size || index < 0)
                    return;

                var left = 2*index + 1;
                var right = 2*index + 2;
                var first = index;

                if (left < size && IsHigherPriority(left, first))
                    first = left;
                if (right < size && IsHigherPriority(right, first))
                    first = right;
                if (first != index)
                {
                    var temp = items[index];
                    items[index] = items[first];
                    items[first] = temp;
                    index = first;
                    continue;
                }
                break;
            }
        }

        private void RemoveAt(int index)
        {
            items[index] = items[--size];
            items[size] = default(QueueItem);
            Heapify();
            if (size < items.Length / 4)
            {
                var temp = items;
                items = new QueueItem[items.Length / 2];
                Array.Copy(temp, 0, items, 0, size);
            }
        }

        /// <summary>
        /// The current number of items in the queue
        /// </summary>
        [SuppressMessage("ReSharper", "ConvertToAutoPropertyWhenPossible")]
        public int Count => size;

        /// <summary>
        /// Peeks the highest priority item on the queue
        /// </summary>
        /// <param name="value">the returned value, default(T) if peeking failed</param>
        /// <returns>true if peeking succeeded, false otherwise</returns>
        public bool TryPeek(out T value)
        {
            if (size == 0)
            {
                value = default(T);
                return false;
            }
            value = items[0].Value;
            return true;
        }

        /// <summary>
        /// Peeks the highest priority item on the queue
        /// </summary>
        /// <returns>the value</returns>
        /// <exception cref="InvalidOperationException">if no items are in the queue</exception>
        public T Peek()
        {
            T val;
            if (!TryPeek(out val))
                throw new InvalidOperationException("Cannot peek on an empty heap");
            return val;
        }

        /// <summary>
        /// Dequeues the highest priority item on the queue
        /// </summary>
        /// <returns>the value</returns>
        /// <exception cref="InvalidOperationException">if no items are in the queue</exception>
        public T Dequeue()
        {
            var result = Peek();
            RemoveAt(0);
            return result;
        }

        /// <summary>
        /// Dequeues the highest priority item on the queue
        /// </summary>
        /// <param name="value">the returned value</param>
        /// <returns>true if dequeueing succeeded, false otherwise</returns>
        public bool TryDequeue(out T value)
        {
            if (!TryPeek(out value)) return false;
            RemoveAt(0);
            return true;
        }

        /// <summary>
        /// Enqueues an item in the queue
        /// </summary>
        /// <param name="item">the item to enqueue</param>
        public void Enqueue(T item)
        {
            if (size >= items.Length)
            {
                var temp = items;
                items = new QueueItem[items.Length * 2];
                Array.Copy(temp, items, temp.Length);
            }

            var index = size++;
            items[index] = new QueueItem { Value = item, Id = Interlocked.Increment(ref _id) };
            Percolate(index);
        }

        /// <summary>
        /// Removes the first matching item from the queue
        /// </summary>
        /// <param name="item">the item to remove</param>
        /// <returns>true if an item was removed</returns>
        public bool Remove(T item)
        {
            for (var i = 0; i < size; ++i)
            {
                if (equalityComparer.Equals(items[i].Value, item))
                {
                    RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        private struct QueueItem : IComparable<QueueItem>
        {
            public T Value;
            public long Id;

            public int CompareTo(QueueItem other)
            {
                var c = Value.CompareTo(other.Value);
                if (c == 0)
                    c = Id.CompareTo(other.Id);
                return c;
            }
        }
    }
}
