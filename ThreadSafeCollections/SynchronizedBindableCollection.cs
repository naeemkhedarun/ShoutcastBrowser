using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Threading;

namespace ThreadSafeCollections
{
    public class SynchronizedBindableCollection<T> : IList<T>, IList, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private readonly IList<T> list;

        #region Constructor

        public SynchronizedBindableCollection(IList<T> list)
        {
            if (list == null || list as INotifyCollectionChanged == null || list as INotifyPropertyChanged == null)
            {
                throw new ArgumentNullException("The list must support IList, INotifyCollectionChanged " +
                                                "and INotifyPropertyChanged.");
            }
            //Copy list over.
            this.list = new List<T>();
            foreach (T t in list)
            {
                list.Add(t);
            }


            INotifyCollectionChanged collectionChanged = list as INotifyCollectionChanged;
            if (collectionChanged != null)
                collectionChanged.CollectionChanged += CollectionChanged_OnCollectionChanged;
        }

        private void CollectionChanged_OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (object newItem in e.NewItems)
                    {
                        list.Add((T) newItem);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (object oldItem in e.OldItems)
                    {
                        list.Remove((T) oldItem);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    throw new ArgumentOutOfRangeException();
                case NotifyCollectionChangedAction.Move:
                    throw new ArgumentOutOfRangeException();
                case NotifyCollectionChangedAction.Reset:
                    list.Clear();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Normal,
                                                     new RaiseCollectionChangedEventHandler(RaiseCollectionChangedEvent),
                                                     e);
        }

        #endregion

        #region IList Members

        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection) list).CopyTo(array, index);
        }

        int ICollection.Count
        {
            get { return ((ICollection) list).Count; }
        }

        bool ICollection.IsSynchronized
        {
            get { return ((ICollection) list).IsSynchronized; }
        }

        object ICollection.SyncRoot
        {
            get { return ((ICollection) list).SyncRoot; }
        }

        int IList.Add(object value)
        {
            return ((IList) list).Add(value);
        }

        void IList.Clear()
        {
            ((IList) list).Clear();
        }

        bool IList.Contains(object value)
        {
            return ((IList) list).Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return ((IList) list).IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            ((IList) list).Insert(index, value);
        }

        bool IList.IsFixedSize
        {
            get { return ((IList) list).IsFixedSize; }
        }

        bool IList.IsReadOnly
        {
            get { return ((IList) list).IsReadOnly; }
        }

        void IList.Remove(object value)
        {
            ((IList) list).Remove(value);
        }

        void IList.RemoveAt(int index)
        {
            ((IList) list).RemoveAt(index);
        }

        object IList.this[int index]
        {
            get { return ((IList) list)[index]; }
            set { ((IList) list)[index] = value; }
        }

        #endregion

        #region IList<T> Members

        public void Add(T item)
        {
            list.Add(item);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return list.Count; }
        }

        public bool IsReadOnly
        {
            get { return list.IsReadOnly; }
        }

        public bool Remove(T item)
        {
            return list.Remove(item);
        }

        public int IndexOf(T item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        public T this[int index]
        {
            get { return list[index]; }
            set { list[index] = value; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) list).GetEnumerator();
        }

        #endregion

        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void RaiseCollectionChangedEvent(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, e);
            }
        }

        private void RaisePropertyChangedEvent(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #region Nested type: RaiseCollectionChangedEventHandler

        private delegate void RaiseCollectionChangedEventHandler(NotifyCollectionChangedEventArgs e);

        #endregion
    }
}