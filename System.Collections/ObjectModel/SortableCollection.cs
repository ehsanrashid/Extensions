namespace System.Collections.ObjectModel
{
    using Generic;

    public class SortableCollection<T> : Collection<T>
    {
        #region Constructors

        public SortableCollection()
            : base()
        {
        }
        public SortableCollection(IList<T> list)
            : base(list)
        {
        }
        #endregion
        
        public void Sort()
        {
            var list = this.Items as List<T>;
            if (list != null)
            {
                list.Sort();
            }
        }
    }
}
