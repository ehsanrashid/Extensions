namespace System.Collections.Generic
{
    using ObjectModel;
    using Linq;

    public class PagedCollection<T> : Collection<T>
    {
        public const int DefaultPageSize = 10;

        int _pageSize;

        /// <summary>
        ///   Gets or sets page size
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > 0) ? value : DefaultPageSize; }
        }

        public PagedCollection(int pageSize = DefaultPageSize)
        {
            _pageSize = pageSize;
        }

        public PagedCollection(IList<T> list, int pageSize = DefaultPageSize)
            : base(list)
        {
            _pageSize = pageSize;
        }

        public PagedCollection(IEnumerable<T> list, int pageSize = DefaultPageSize)
            : base(list.ToList())
        {
            _pageSize = pageSize;
        }

        public PagedCollection(PagedCollection<T> pageCol, int pageSize = DefaultPageSize)
            : base(pageCol.Items)
        {
            _pageSize = pageSize;
        }

        /// <summary>
        ///   Gets total page
        /// </summary>
        public int PageCount
        {
            get { return (int) Math.Ceiling((decimal) Count/PageSize); }
        }

        /// <summary>
        ///   Returns data by page number
        /// </summary>
        public IEnumerable<T> GetPage(int pageIndex)
        {
            if (pageIndex < 0 || pageIndex >= PageCount) return new T[] {};
            var offset = pageIndex*PageSize;
            return Items.Skip(offset).Take(PageSize);
        }

        /// <summary>
        ///   Returns data by page number and page size
        /// </summary>
        public static IEnumerable<T> GetPage(IEnumerable<T> collection, int pageIndex, int pageSize = DefaultPageSize)
        {
            return new PagedCollection<T>(collection, pageSize).GetPage(pageIndex);
        }
    }
}