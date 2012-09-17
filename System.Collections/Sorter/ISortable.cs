namespace System.Collections.Sorter
{
    using Generic;

    /// <summary>
    /// An interface implemented that can be implemented by sortable collections.
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public interface ISortable<T>
	{
        /// <summary>
        /// Sorts using the specified sorter.
        /// </summary>
        /// <param name="sorter">The sorter to use in the sorting process.</param>
		void Sort(ISorter<T> sorter);

        /// <summary>
        /// Sorts using the specified sorter.
        /// </summary>
        /// <param name="sorter">The sorter to use in the sorting process.</param>
        /// <param name="comparison">The comparison.</param>
        void Sort(ISorter<T> sorter, Comparison<T> comparison);

        /// <summary>
        /// Sorts using the specified sorter.
        /// </summary>
        /// <param name="sorter">The sorter to use in the sorting process.</param>
        /// <param name="comparer">The comparer.</param>
        void Sort(ISorter<T> sorter, IComparer<T> comparer);
	}
}
