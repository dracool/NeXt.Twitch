using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using NeXt.Twitch.Chat.Wpf.Annotations;

namespace NeXt.Twitch.Chat.Wpf
{
    public class NotifyCollectionBase<T> : NotifyObjectBase, IReadOnlyList<T>, INotifyCollectionChanged
    {
        private readonly List<T> list;

        protected NotifyCollectionBase()
        {
            list = new List<T>();
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) list).GetEnumerator();
        }

        protected void AddRange(IList<T> items)
        {
            var c = Count;
            list.AddRange(items);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items, c));
            Notify(nameof(Count));
        }

        /// <inheritdoc />
        protected void Add(T item)
        {
            list.Add(item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            Notify(nameof(Count));
        }

        /// <inheritdoc />
        protected void Clear()
        {
            list.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            Notify(nameof(Count));
        }

        /// <inheritdoc />
        public bool Contains(T item) => list.Contains(item);

        /// <inheritdoc />
        public void CopyTo([NotNull] T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0 || arrayIndex >= array.Length) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            list.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        protected bool Remove(T item)
        {
            var b = list.Remove(item);
            if (b)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
                Notify(nameof(Count));
            }
            return b;
        }

        /// <inheritdoc />
        public int Count => list.Count;

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        /// <inheritdoc />
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count) throw new ArgumentOutOfRangeException(nameof(index));
                return list[index];
            }
            protected set
            {
                if (index < 0 || index >= Count) throw new ArgumentOutOfRangeException(nameof(index));
                var item = list[index];
                if (EqualityComparer<T>.Default.Equals(item, value)) return;
                list[index] = value;
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, item, index));
            }
        }
    }
}
