using System.Linq;

namespace System.Collections.Generic
{
    public class PagedEnumerable<T> : IEnumerable<T>
    {
        public const int DefaultPageSize = 10;

        readonly IEnumerable<T> _enumerable;
        int _pageSize;

        /// <summary>
        ///   Gets or sets page size
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > 0) ? value : DefaultPageSize; }
        }

        public int Count
        {
            get { return _enumerable.Count(); }
        }

        /// <summary>
        ///   Gets total page
        /// </summary>
        public int PageCount
        {
            get { return (int) Math.Ceiling(Count/(decimal) PageSize); }
        }

        /// <summary>
        ///   Creates paging collection and sets page size
        /// </summary>
        public PagedEnumerable(IEnumerable<T> enumerable, int pageSize = DefaultPageSize)
        {
            if (null == enumerable) throw new ArgumentNullException("enumerable");
            PageSize = pageSize;
            _enumerable = enumerable.ToArray();
        }

        /// <summary>
        ///   Returns data by page number
        /// </summary>
        public IEnumerable<T> GetPage(int page)
        {
            if (page < 0 || page > PageCount)
                return new T[] {};
            var offset = (page - 1)*PageSize;
            return _enumerable.Skip(offset).Take(PageSize);
        }

        /// <summary>
        ///   Returns data by page number and page size
        /// </summary>
        public static IEnumerable<T> GetPage(IEnumerable<T> enumerable, int page, int pageSize = DefaultPageSize)
        {
            return new PagedCollection<T>(enumerable, pageSize).GetPage(page);
        }

        #region IEnumerable<T> Members
        /// <summary>
        ///   Returns an enumerator that iterates through collection
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return _enumerable.GetEnumerator();
        }
        #endregion

        #region IEnumerable Members
        /// <summary>
        ///   Returns an enumerator that iterates through collection
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}