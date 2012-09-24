namespace System.Windows.Forms
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
    
     As the events are firing off in your controls/forms that set up DataBindings, 
     * all you need to do is add the CurrencyManger to the collection. Some examples of this are shown below:
    
    */
    /* Suppose we have a 'mainClass' that has a CurrencyManagerCollection class called CurrencyManagers 
     * this will add the currency manager for the current form/control for the binding context of the 'TheDataView' */

    //mainClass.CurrencyManagers.Add(((CurrencyManager) this.BindingContext[TheDataView]));

    /* this will add the currency manager for the current form/control for the binding context of the 'TheDataTable' */

    //mainClass.CurrencyManagers.Add(((CurrencyManager) this.BindingContext[TheDataTable]));

    /* if you already have a CurrencyManager set up, you can just add it. */

    /* if your CurrencyManager was called 'currencyManger' then: */

    //mainClass.CurrencyManagers.Add(currencyManager);

    /* As you're preparing to load data into your DataSet (hopefully it will be a strongly-typed DataSet!), 
     * you can to the following: */

    //mainClass.CurrencyManagers.SuspendBindings() ;
    // your code here that populates the DataSet
    //mainClass.CurrencyManagers.ResumeBindings() ;

    using Collections;

    /// <summary>
    /// Utility for handling a collection of currencymanagers.
    /// If we add all of the currency managers to this collection, 
    /// we can easily 'SuspendBinding' or 'ResumeBinding' on all currency managers at the same time, 
    /// when we're changing the underlying lists, through searching or clearing, or whatever
    /// </summary>
    public class CurrencyManagerCollection : EventsCollectionBase
    {
        /// <summary>
        /// Suspend bindings for all currency managers in collection
        /// </summary>
        public void SuspendBindings()
        {
            foreach (CurrencyManager curManager in this) curManager.SuspendBinding();
        }

        /// <summary>
        /// Resume bindings for all currency managers in collection
        /// </summary>
        public void ResumeBindings()
        {
            foreach (CurrencyManager curManager in this) curManager.ResumeBinding();
        }

        public int Add(CurrencyManager curManager)
        {
            //make sure we're not trying to add the same object
            return Contains(curManager) ? 0 : List.Add(curManager);
        }

        public void Remove(CurrencyManager curManager)
        {
            List.Remove(curManager);
        }

        public void Insert(int index, CurrencyManager curManager)
        {
            List.Insert(index, curManager);
        }

        public bool Contains(CurrencyManager curManager)
        {
            return List.Contains(curManager);
        }

        public CurrencyManager this[int index]
        {
            get { return (List[index] as CurrencyManager); }
        }
    }
}