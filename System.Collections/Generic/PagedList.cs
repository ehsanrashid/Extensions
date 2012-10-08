namespace System.Collections.Generic
{
    using Linq;
    using Interface;

    /// <summary>
    /// Represent a class to implements Paging functionality.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : List<T>, IPagedList
    {
        /// <summary>
        /// Gets a value that indicate current page index (Starts by Zero).
        /// </summary>
        public int PageIndex { get; private set; }

        /// <summary>
        /// Gets a value that indicate each page size.
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// Gets a value that indicate count of all rows in data source.
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see><cref>PagedList&amp;lt;T&amp;gt;</cref></see> class.
        /// </summary>
        /// <param name="source">The source list of elements containing all elements to be paged over.</param>
        /// <param name="pageIndex">The current page number (1 based).</param>
        /// <param name="pageSize">Size of a page (number of items per page).</param>
        public PaginatedList(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            PageIndex = Math.Min(Math.Max(1, pageIndex), TotalPages);
            PageSize = pageSize;
            TotalCount = source.Count();

            AddRange(source.Skip((PageIndex - 1) * PageSize).Take(PageSize));
        }

        /// <summary>
        /// Gets a value that indicate count of pages in data source.
        /// </summary>
        public int TotalPages
        {
            get { return (int) Math.Ceiling(TotalCount / (double) PageSize); }
        }

        /// <summary>
        /// Gets a value that indicate that does previous page exists or not.
        /// </summary>
        public bool HasPreviousPage
        {
            get { return (PageIndex > 1); }
        }

        /// <summary>
        /// Gets a value that indicate that does next page exists or not.
        /// </summary>
        public bool HasNextPage
        {
            get
            {
                return
                    //(PageIndex < TotalPages);
                    (PageIndex * PageSize) < TotalCount;
            }
        }
    }
}