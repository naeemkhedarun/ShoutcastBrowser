using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ThreadSafeCollections
{
    public class ReadOnlySynchronizedObservableCollection<T> : ReadOnlyCollection<T>,
                                                               INotifyPropertyChanged, INotifyCollectionChanged
    {
        #region Constructor

        public ReadOnlySynchronizedObservableCollection(SynchronizedObservableCollection<T> list)
            : base(list)
        {
            list.PropertyChanged += delegate(Object sender, PropertyChangedEventArgs e) { OnPropertyChanged(e); };
            list.CollectionChanged +=
                delegate(Object sender, NotifyCollectionChangedEventArgs e) { OnCollectionChanged(e); };
        }

        #endregion

        #region Event Handling

        private NotifyCollectionChangedEventHandler collectionChanged;
        private PropertyChangedEventHandler propertyChanged;

        #region INotifyCollectionChanged Members

        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add { collectionChanged += value; }
            remove { collectionChanged -= value; }
        }

        #endregion

        #region INotifyPropertyChanged Members

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { propertyChanged += value; }
            remove { propertyChanged -= value; }
        }

        #endregion

        protected event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { collectionChanged += value; }
            remove { collectionChanged -= value; }
        }

        protected event PropertyChangedEventHandler PropertyChanged
        {
            add { propertyChanged += value; }
            remove { propertyChanged -= value; }
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (propertyChanged != null)
            {
                propertyChanged(this, e);
            }
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (collectionChanged != null)
            {
                collectionChanged(this, e);
            }
        }

        #endregion
    }
}