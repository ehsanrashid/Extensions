namespace System.Collections.Sorter
{
    using Generic;

    /// <summary>
    /// An interface for a Sorter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public interface ISorter<T>
	{
        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list to sort.</param>
		void Sort(IList<T> list);

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list to sort.</param>
        /// <param name="order">The order in which to sort the list.</param>
        void Sort(IList<T> list, SortOrder order);

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list to sort.</param>
        /// <param name="comparer">The comparer to use.</param>
        void Sort(IList<T> list, IComparer<T> comparer);

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list to sort.</param>
        /// <param name="comparison">The comparison to use.</param>
        void Sort(IList<T> list, Comparison<T> comparison);
	}
}
