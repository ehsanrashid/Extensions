namespace System.Collections.ObjectModel
{
    using Generic;

    public class SortableCollection<T> : Collection<T>
    {
        public SortableCollection()
        {
        }

        public SortableCollection(IList<T> list)
            : base(list)
        {
        }
        
        public void Sort()
        {
            var list = Items as List<T>;
            if (list != null)
            {
                list.Sort();
            }
        }
    }
}
