using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ThreadSafeCollections
{
    public class SynchronizedObservableCollection<T> : IList<T>,
                                                       IList, INotifyPropertyChanged, INotifyCollectionChanged
    {
        private readonly ObservableCollection<T> items;
        private readonly Object sync;

        #region Constructors

        public SynchronizedObservableCollection(Object syncRoot, IEnumerable<T> list)
        {
            sync = syncRoot ?? new Object();
            items = (list == null)
                        ? new ObservableCollection<T>()
                        :
                            new ObservableCollection<T>(new List<T>(list));

            items.CollectionChanged +=
                delegate(Object sender, NotifyCollectionChangedEventArgs e) { OnCollectionChanged(e); };
            INotifyPropertyChanged propertyChangedInterface = items;
            propertyChangedInterface.PropertyChanged +=
                delegate(Object sender, PropertyChangedEventArgs e) { OnPropertyChanged(e); };
        }

        public SynchronizedObservableCollection(object syncRoot) : this(syncRoot, null)
        {
        }

        public SynchronizedObservableCollection() : this(null, null)
        {
        }

        #endregion

        #region Methods

        #region IList Members

        void ICollection.CopyTo(Array array, int index)
        {
            lock (sync)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    array.SetValue(items[i], index + i);
                }
            }
        }

        int IList.Add(object value)
        {
            VerifyValueType(value);
            lock (sync)
            {
                Add((T) value);
                return (Count - 1);
            }
        }

        bool IList.Contains(object value)
        {
            VerifyValueType(value);
            return Contains((T) value);
        }

        int IList.IndexOf(object value)
        {
            VerifyValueType(value);
            return IndexOf((T) value);
        }

        void IList.Insert(int index, object value)
        {
            VerifyValueType(value);
            Insert(index, (T) value);
        }

        void IList.Remove(object value)
        {
            VerifyValueType(value);
            Remove((T) value);
        }

        #endregion

        #region IList<T> Members

        public void Add(T item)
        {
            lock (sync)
            {
                int index = items.Count;
                InsertItem(index, item);
            }
        }

        public void Clear()
        {
            lock (sync)
            {
                ClearItems();
            }
        }

        public bool Contains(T item)
        {
            lock (sync)
            {
                return items.Contains(item);
            }
        }

        public void CopyTo(T[] array, int index)
        {
            lock (sync)
            {
                items.CopyTo(array, index);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (sync)
            {
                return items.GetEnumerator();
            }
        }

        public int IndexOf(T item)
        {
            lock (sync)
            {
                return InternalIndexOf(item);
            }
        }

        public void Insert(int index, T item)
        {
            lock (sync)
            {
                if ((index < 0) || (index > items.Count))
                {
                    throw new ArgumentOutOfRangeException("index", index, "Value must be in range.");
                }
                InsertItem(index, item);
            }
        }

        public bool Remove(T item)
        {
            lock (sync)
            {
                int index = InternalIndexOf(item);
                if (index < 0)
                {
                    return false;
                }
                RemoveItem(index);
                return true;
            }
        }

        public void RemoveAt(int index)
        {
            lock (sync)
            {
                if ((index < 0) || (index >= items.Count))
                {
                    throw new ArgumentOutOfRangeException("index", index,
                                                          "Value must be in range.");
                }
                RemoveItem(index);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        #endregion

        protected virtual void ClearItems()
        {
            items.Clear();
        }

        protected virtual void InsertItem(int index, T item)
        {
            items.Insert(index, item);
        }

        private int InternalIndexOf(T item)
        {
            int count = items.Count;
            for (int i = 0; i < count; i++)
            {
                if (Equals(items[i], item))
                {
                    return i;
                }
            }
            return -1;
        }

        protected virtual void RemoveItem(int index)
        {
            items.RemoveAt(index);
        }

        protected virtual void SetItem(int index, T item)
        {
            items[index] = item;
        }

        private static void VerifyValueType(object value)
        {
            if (value == null)
            {
                if (typeof (T).IsValueType)
                {
                    throw new ArgumentException("Synchronized collection wrong type null.");
                }
            }
            else if (!(value is T))
            {
                throw new ArgumentException("Synchronized collection wrong type.");
            }
        }

        #endregion

        #region Properties

        protected ObservableCollection<T> Items
        {
            get { return items; }
        }

        public object SyncRoot
        {
            get { return sync; }
        }

        #region IList Members

        bool ICollection.IsSynchronized
        {
            get { return true; }
        }

        object ICollection.SyncRoot
        {
            get { return sync; }
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        bool IList.IsReadOnly
        {
            get { return false; }
        }

        object IList.this[int index]
        {
            get { return this[index]; }
            set
            {
                VerifyValueType(value);
                this[index] = (T) value;
            }
        }

        #endregion

        #region IList<T> Members

        public int Count
        {
            get
            {
                lock (sync)
                {
                    return items.Count;
                }
            }
        }

        public T this[int index]
        {
            get
            {
                lock (sync)
                {
                    return items[index];
                }
            }
            set
            {
                lock (sync)
                {
                    if ((index < 0) || (index >= items.Count))
                    {
                        throw new ArgumentOutOfRangeException("index", index,
                                                              "Value must be in range.");
                    }
                    SetItem(index, value);
                }
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #endregion

        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null) CollectionChanged(this, e);
        }
    }
}