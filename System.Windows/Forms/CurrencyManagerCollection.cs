namespace System.Windows.Forms
{
    using System.Collections;

    /// <summary>
    /// Utility for handling a collection of currencymanagers.
    /// If we add all of the currency managers to this collection, 
    /// we can easily 'SuspendBinding' or 'ResumeBinding' on all 
    /// currency managers at the same time, 
    /// when we're changing the underlying lists, 
    /// through searching or clearing, or whatever
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