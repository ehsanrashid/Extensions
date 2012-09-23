namespace System.Collections.ObjectModel
{
    using Generic;

    public class HistoryCollection<T> : Collection<T>
    {

        public Collection<T> History { get; private set; }

        #region Constructors

        public HistoryCollection()
            : base()
        {
            History = new Collection<T>();
        }
        public HistoryCollection(IList<T> list)
            : base(list)
        {
            History = new Collection<T>();
        }

        #endregion

        #region Overrides

        protected override void SetItem(Int32 index, T item)
        {
            base.SetItem(index, item);
            //Console.Out.WriteLine("New Value-->", item);
        }

        protected override void InsertItem(Int32 index, T item)
        {
            base.InsertItem(index, item);
            //Console.Out.WriteLine("Added--> " + item);
        }

        protected override void RemoveItem(Int32 index)
        {
            if (0 <= index && index < Count)
            {
                var item = this[index];
                //Console.Out.WriteLine("Removed--> " + item);
                History.Insert(0, item);
                base.RemoveItem(index);
            }
        }

        protected override void ClearItems()
        {
            foreach (var item in this)
            {
                History.Insert(0, item);
            }

            //Console.Out.WriteLine("All Items Removed");
            base.ClearItems();
        }

        #endregion

    }
}
