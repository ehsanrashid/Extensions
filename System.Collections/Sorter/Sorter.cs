namespace System.Collections.Sorter
{
    using Generic;

    /// <summary>
    ///   The base class used for all Sorters in the library.
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    public abstract class Sorter<T> : ISorter<T>
    {
        #region ISorter<T> Members
        /// <summary>
        ///   Sorts the specified list.
        /// </summary>
        /// <param name="list"> The list. </param>
        /// <param name="comparer"> The comparer to use in comparing items. </param>
        public abstract void Sort(IList<T> list, IComparer<T> comparer);

        /// <summary>
        ///   Sorts the specified list.
        /// </summary>
        /// <param name="list"> The list. </param>
        /// <param name="comparison"> The comparison to use. </param>
        public void Sort(IList<T> list, Comparison<T> comparison)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (comparison == null)
                throw new ArgumentNullException("comparison");
            Sort(list, new ComparisonComparer<T>(comparison));
        }

        /// <summary>
        ///   Sorts the specified list.
        /// </summary>
        /// <param name="list"> The list. </param>
        public void Sort(IList<T> list)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            Sort(list, Comparer<T>.Default);
        }

        /// <summary>
        ///   Sorts the specified list.
        /// </summary>
        /// <param name="list"> The list. </param>
        /// <param name="order"> The order in which to sort the list. </param>
        public void Sort(IList<T> list, SortOrder order)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            switch (order)
            {
                case SortOrder.Ascending:
                    Sort(list, Comparer<T>.Default);
                    break;
                case SortOrder.Descending:
                    Sort(list, new InvertedComparer<T>(Comparer<T>.Default));
                    break;
            }
        }
        #endregion

        #region Protected Members
        /// <summary>
        ///   Swaps items in the specified list.
        /// </summary>
        /// <param name="list"> The list. </param>
        /// <param name="pos1"> The position of the first item. </param>
        /// <param name="pos2"> The position of the last item. </param>
        protected void Swap(IList<T> list, int pos1, int pos2)
        {
            if (pos1 == pos2) return;
            var item = list[pos1];
            list[pos1] = list[pos2];
            list[pos2] = item;
        }

        protected static void FindMinMaxValues(IList<int> list, int beg, int end, IComparer<int> comparer,
                                               out int minValue, out int maxValue)
        {
            minValue = list[beg];
            maxValue = list[beg];
            for (var index = beg + 1; index <= end; ++index)
            {
                var item = list[index];
                if (comparer.Compare(item, minValue) < 0)
                    minValue = item;
                if (comparer.Compare(item, maxValue) > 0)
                    maxValue = item;
            }
        }

        protected static void CountValues(IList<int> list, int[] counts, int beg, int end, int minValue)
        {
            for (var index = beg; index <= end; ++index)
                ++counts[list[index] - minValue];
        }

        protected static void Fill(IList<T> list, int beg, int end, T value)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (0 > beg || beg > end) throw new ArgumentOutOfRangeException("beg");
            if (end > list.Count) throw new ArgumentOutOfRangeException("end");
            for (var index = beg; index < end; ++index)
                list[index] = value;
        }
        #endregion
    }
}