namespace System.Collections
{
    /// <summary>
    /// Base class for managing strongly typed collections
    /// </summary>
    public abstract class EventsCollectionBase : CollectionBase
    {
        // Declare the event signatures
        public delegate void CollectionClear();
        public delegate void CollectionChange(int index, Object value);

        // Collection events
        public event CollectionClear Clearing;
        public event CollectionClear Cleared;
        public event CollectionChange Inserting;
        public event CollectionChange Inserted;
        public event CollectionChange Removing;
        public event CollectionChange Removed;

        // Overrides for generating events
        protected override void OnClear()
        {
            base.OnClear();
            if (default(CollectionClear) != Clearing) Clearing();
        }

        protected override void OnClearComplete()
        {
            base.OnClearComplete();
            if (default(CollectionClear) != Cleared) Cleared();
        }

        protected override void OnInsert(int index, Object value)
        {
            base.OnInsert(index, value);
            if (default(CollectionChange) != Inserting) Inserting(index, value);
        }

        protected override void OnInsertComplete(int index, Object value)
        {
            base.OnInsertComplete(index, value);
            if (default(CollectionChange) != Inserted) Inserted(index, value);
        }

        protected override void OnRemove(int index, Object value)
        {
            base.OnRemove(index, value);
            if (default(CollectionChange) != Removing) Removing(index, value);
        }

        protected override void OnRemoveComplete(int index, Object value)
        {
            base.OnRemoveComplete(index, value);
            if (default(CollectionChange) != Removed) Removed(index, value);
        }
    }
}