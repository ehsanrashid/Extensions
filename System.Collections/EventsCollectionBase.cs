namespace System.Collections
{
    /*
     * Introduction
     Due to the large number of records I deal with in my applications, the ability to suspend DataBindings is key to performance.
     I have often spent too much time making sure I am suspending DataBindings for all areas of my app.
     It is common to have a dozen or more active screens with a Binding Context.
     One common reason for suspending binding is to be able to make changes to multiple fields before validation occurs.
     Those cases can be handled on a 'one on one' basis as business logic dictates.
     There are a myriad of events that can fire off on bound controls when you're populating a dataset, both on grids and individual controls.
     I have found a significant performance increase when I suspend Bindings as I'm loading data, or
 
 
     * Usage
    
     As the events are firing off in your controls/forms that set up DataBindings, all you need to do is add the CurrencyManger to the collection. Some examples of this are shown below:
    
    */
    /* Suppose we have a 'mainClass' that has a CurrencyManagerCollection class called CurrencyManagers this will add the currency manager for the current form/control for the binding context of the 'TheDataView' */

    //mainClass.CurrencyManagers.Add(((CurrencyManager) this.BindingContext[TheDataView]));

    /* this will add the currency manager for the current form/control for the binding context of the 'TheDataTable' */

    //mainClass.CurrencyManagers.Add(((CurrencyManager) this.BindingContext[TheDataTable]));

    /* if you already have a CurrencyManager set up, you can just add it. */

    /* if your CurrencyManager was called 'currencyManger' then: */

    //mainClass.CurrencyManagers.Add(currencyManager);

    /* As you're preparing to load data into your DataSet (hopefully it will be a strongly-typed DataSet!), you can to the following: */

    //mainClass.CurrencyManagers.SuspendBindings() ;
    // your code here that populates the DataSet
    //mainClass.CurrencyManagers.ResumeBindings() ;

    /// <summary>
    /// Base class for managing strongly typed collections
    /// </summary>
    public abstract class EventsCollectionBase : CollectionBase
    {
        // Declare the event signatures
        public delegate void CollectionClear();
        public delegate void CollectionChange(int index, object value);

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
            if (default(CollectionClear) != Clearing) Clearing();
        }

        protected override void OnClearComplete()
        {
            if (default(CollectionClear) != Cleared) Cleared();
        }

        protected override void OnInsert(int index, object value)
        {
            if (default(CollectionChange) != Inserting) Inserting(index, value);
        }

        protected override void OnInsertComplete(int index, object value)
        {
            if (default(CollectionChange) != Inserted) Inserted(index, value);
        }

        protected override void OnRemove(int index, object value)
        {
            if (default(CollectionChange) != Removing) Removing(index, value);
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            if (default(CollectionChange) != Removed) Removed(index, value);
        }
    }
}