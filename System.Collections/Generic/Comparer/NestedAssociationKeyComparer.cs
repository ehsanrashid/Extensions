namespace System.Collections.Generic
{
    /// <summary>
    ///   An association comparer that wraps another implementation of an IComparer instance.
    /// </summary>
    /// <typeparam name="TKey"> </typeparam>
    /// <typeparam name="TValue"> </typeparam>
    public sealed class NestedAssociationKeyComparer<TKey, TValue> : IComparer<Association<TKey, TValue>>
    {
        private readonly IComparer<TKey> nestedComparer;

        /// <summary>
        ///   Initializes a new instance of the <see cref="NestedAssociationKeyComparer&lt;TKey, TValue&gt;" /> class.
        /// </summary>
        /// <param name="comparer"> The nested comparer to use. </param>
        public NestedAssociationKeyComparer(IComparer<TKey> comparer)
        {
            if (null == comparer) throw new ArgumentNullException("comparer");
            nestedComparer = comparer;
        }

        #region IComparer<Association<TKey,TValue>> Members
        /// <summary>
        ///   Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x"> The first object to compare. </param>
        /// <param name="y"> The second object to compare. </param>
        /// <returns> Value Condition Less than zerox is less than y.Zerox equals y.Greater than zerox is greater than y. </returns>
        public int Compare(Association<TKey, TValue> x, Association<TKey, TValue> y)
        {
            return nestedComparer.Compare(x.Key, y.Key);
        }
        #endregion
    }
}