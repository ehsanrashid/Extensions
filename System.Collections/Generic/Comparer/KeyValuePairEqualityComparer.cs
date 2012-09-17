namespace System.Collections.Generic
{
    /// <summary>
    ///   An Equality Comparer for comparing KeyValue pairs.
    /// </summary>
    /// <typeparam name="TKey"> </typeparam>
    /// <typeparam name="TValue"> </typeparam>
    public sealed class KeyValuePairEqualityComparer<TKey, TValue> : IEqualityComparer<KeyValuePair<TKey, TValue>>
        where TKey : IComparable
        where TValue : IComparable
    {
        #region IEqualityComparer<KeyValuePair<TKey,TValue>> Members
        /// <summary>
        ///   Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x"> The first object of type T to compare. </param>
        /// <param name="y"> The second object of type T to compare. </param>
        /// <returns> true if the specified objects are equal; otherwise, false. </returns>
        public bool Equals(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
        {
            return (x.Key.CompareTo(y.Key) == 0) && (x.Value.CompareTo(y.Value) == 0);
        }

        /// <summary>
        ///   Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj"> The <see cref="T:System.Object"></see> for which a hash code is to be returned. </param>
        /// <returns> A hash code for the specified object. </returns>
        /// <exception cref="T:System.ArgumentNullException">The type of obj is a reference type and obj is null.</exception>
        public int GetHashCode(KeyValuePair<TKey, TValue> obj)
        {
            return GetHashCode() ^ obj.GetHashCode();
        }
        #endregion
    }
}