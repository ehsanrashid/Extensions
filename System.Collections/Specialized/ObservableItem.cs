namespace System.Collections.Specialized
{
    using Generic;
    using ComponentModel;

    public abstract class ObservableItem : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if (null != handler) handler(this, e);
        }

        protected void SetProperty<T>(String propertyName, ref T field, T value)
        {
            SetProperty(propertyName, ref field, value, null);
        }

        protected void SetProperty<T>(String propertyName, ref T field, T value, IEqualityComparer<T> comparer)
        {
            if (null == comparer) comparer = EqualityComparer<T>.Default;
            if (comparer.Equals(field, value)) return;
            field = value;
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }



    public class Person : ObservableItem
    {
        private String m_firstName;
        private String m_lastName;

        public String FirstName
        {
            get { return m_firstName; }
            set { SetProperty("FirstName", ref m_firstName, value, StringComparer.Ordinal); }
        }

        public String LastName
        {
            get { return m_lastName; }
            set { SetProperty("LastName", ref m_lastName, value, StringComparer.Ordinal); }
        }
    }
}