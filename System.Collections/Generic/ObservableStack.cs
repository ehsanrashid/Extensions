using System.Collections.Specialized;
using System.ComponentModel;

namespace System.Collections.Generic
{
    public class ObservableStack<T> : Stack<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public ObservableStack() { }

        public ObservableStack(IEnumerable<T> collection)
        {
            foreach (var item in collection) base.Push(item);
        }

        public ObservableStack(List<T> list)
        {
            foreach (var item in list) base.Push(item);
        }

        #region INotifyCollectionChanged Members

        public virtual event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region INotifyPropertyChanged Members

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { PropertyChanged += value; }
            remove { PropertyChanged -= value; }
        }

        #endregion

        public new virtual void Clear()
        {
            base.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public new virtual T Pop()
        {
            var item = base.Pop();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            return item;
        }

        public new virtual void Push(T item)
        {
            base.Push(item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            RaiseCollectionChanged(e);
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e);
        }

        protected virtual event PropertyChangedEventHandler PropertyChanged;

        void RaiseCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (null != CollectionChanged) CollectionChanged(this, e);
        }

        void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (null != PropertyChanged) PropertyChanged(this, e);
        }
    }
}