namespace System.Collections.Generic
{
    // Wraps a generic Comparison<T> delegate in an IComparer to make it easy
    // to use a lambda expression for methods that take an IComparer or IComparer<T>
    public class ComparisonComparer<T> : IComparer, IComparer<T>
    {
        private Comparison<T> _comparison;

        /// <summary>
        ///   Gets or sets the comparison used in this comparer.
        /// </summary>
        /// <value> The comparison used in this comparer. </value>
        public Comparison<T> Comparison
        {
            get { return _comparison; }
            set
            {
                if (null == value) throw new ArgumentNullException("value");
                _comparison = value;
            }
        }

        public ComparisonComparer(Comparison<T> comparison)
        {
            if (null == comparison) throw new ArgumentNullException("comparison");
            _comparison = comparison;
        }

        public static implicit operator ComparisonComparer<T>(Comparison<T> comparison)
        {
            return new ComparisonComparer<T>(comparison);
        }

        /// <summary>
        ///   Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x"> The first object to compare. </param>
        /// <param name="y"> The second object to compare. </param>
        /// <returns> Value Condition Less than zerox is less than y.Zerox equals y.Greater than zerox is greater than y. </returns>
        public int Compare(T x, T y)
        {
            return _comparison(x, y);
        }

        public int Compare(Object x, Object y)
        {
            return _comparison((T) x, (T) y);
        }
    }
}