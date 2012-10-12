namespace System.Web.Mvc
{
    using Collections;
    using Collections.Generic;
    using Text;
    using Html;
    using Linq;

    /// <summary>
    /// <code>
    /// Here is a fluent pager in action with ASP.NET MVC (the code for the pager I've written is listed at the bottom of the article).
    /// First let's look at how you might use a fluent pager in a controller 
    /// ("fluent" just means the class allows method chaining to modify the way the pager behaves):
    /// 
    /// public class HomeController : Controller
    /// {
    ///     public ActionResult Index(int? page)
    ///     {
    ///          ViewBag.Pageable = Pageable.Items(100).PerPage(10).Move(page ?? 1).Segment(5).Center();
    ///          return View();
    ///     }
    /// }
    /// 
    /// The razor view markup to render the pager is:
    /// @{
    ///    Pageable pageable = ViewBag.Pageable;
    ///    if (pager.HasPrevPage)
    ///    {
    ///      @Html.ActionLink("<<", "Index", new { page = pageable.FirstPageIndex });
    ///      @Html.ActionLink("<", "Index", new { page = pageable.PreviousPageIndex });
    ///    }
    ///    foreach (int page in ViewBag.Pageable)
    ///    {
    ///      @Html.ActionLink(page.ToString(), "Index", new { page = page });
    ///    }
    ///    if (pager.HasNextPage)
    ///    {
    ///      @Html.ActionLink(">", "Index", new { page = pageable.NextPageIndex });
    ///      @Html.ActionLink(">>", "Index", new { page = pageable.LastPageIndex });
    ///    }
    ///  }
    /// 
    /// 
    /// Pageable.Items(100).PerPage(10) 
    /// ensures that there are only 10 items per page, which gives 10 page links (by default the pager assumes you're on the first page so we get a move to next page arrow (>) and a move to last page arrow (>>) due to how the view was implemented):
    /// 
    /// 1 2 3 4 5 6 7 8 9 10 > >>
    /// 
    /// Pageable.Items(100).PerPage(10).Move(5) 
    /// moves the current page index to 5 so that a move to previous page arrow (<) and first page arrow (<<) become active:
    /// 
    /// << < 1 2 3 4 5 6 7 8 9 10 > >>
    /// 
    /// Pageable.Items(100).PerPage(10).Move(5).Segment(5) 
    /// makes sure only 5 of the possible 10 pages are visible in the pager (ending on the current page index, or ending as close as possible to the current page index):
    /// 
    /// << < 1 2 3 4 5 > >>
    /// 
    /// Finally, Pageable.Items(100).PerPage(10).Move(5).Segment(5).Center() 
    /// ensures the 5 visible pages are centred around the current page index rather than ending on the current page index:
    /// 
    /// << < 3 4 5 6 7 8 > >>
    /// </code>
    /// 
    /// </summary>

    public sealed class Pageable : IEnumerable<int>
    {
        int _noOfPages;
        int _skipPages;
        int _takePages;
        int _pageIndex;
        int _noOfItems;
        int _itemsPerPage;

        Pageable()
        { }

        Pageable(Pageable pager)
        {
            _noOfItems = pager._noOfItems;
            _pageIndex = pager._pageIndex;
            _noOfPages = pager._noOfPages;
            _takePages = pager._takePages;
            _skipPages = pager._skipPages;
            _itemsPerPage = pager._itemsPerPage;
        }

        /// <summary>
        /// Creates a pager for the given number of items.
        /// </summary>
        public static Pageable Items(int noOfItems)
        {
            return new Pageable
            {
                _noOfItems = noOfItems,
                _pageIndex = 1,
                _noOfPages = 1,
                _skipPages = 0,
                _takePages = 1,
                _itemsPerPage = noOfItems
            };
        }

        /// <summary>
        /// Specifies the number of items per page.
        /// </summary>
        public Pageable PerPage(int itemsPerPage)
        {
            int noOfPages = (_noOfItems + itemsPerPage - 1) / itemsPerPage;

            return new Pageable(this)
                    {
                        _noOfPages = noOfPages,
                        _skipPages = 0,
                        _takePages = noOfPages - _pageIndex + 1,
                        _itemsPerPage = itemsPerPage
                    };
        }

        /// <summary>
        /// Moves the pager to the given page index
        /// </summary>
        public Pageable Move(int pageIndex)
        {
            return new Pageable(this)
                    {
                        _pageIndex = pageIndex
                    };
        }

        /// <summary>
        /// Segments the pager so that it will display a maximum number of pages.
        /// </summary>
        public Pageable Segment(int maximum)
        {
            int count = Math.Min(_noOfPages, maximum);

            return new Pageable(this)
                    {
                        _takePages = count,
                        _skipPages = Math.Min(_skipPages, _noOfPages - count),
                    };
        }

        /// <summary>
        /// Centers the segment around the current page
        /// </summary>
        public Pageable Center()
        {
            int radius = ((_takePages + 1) / 2);

            return new Pageable(this)
                    {
                        _skipPages = Math.Min(Math.Max(_pageIndex - radius, 0), _noOfPages - _takePages)
                    };
        }

        public IEnumerator<int> GetEnumerator()
        {
            return Enumerable.Range(_skipPages + 1, _takePages).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool IsPaged { get { return _noOfItems > _itemsPerPage; } }

        public int NumberOfPages { get { return _noOfPages; } }

        public bool IsUnpaged { get { return _noOfPages == 1; } }

        public int CurrentPageIndex { get { return _pageIndex; } }

        public int NextPageIndex { get { return _pageIndex + 1; } }

        public int LastPageIndex { get { return _noOfPages; } }

        public int FirstPageIndex { get { return 1; } }

        public bool HasNextPage { get { return _pageIndex < _noOfPages && _noOfPages > 1; } }

        public bool HasPrevPage { get { return _pageIndex > 1 && _noOfPages > 1; } }

        public int PrevPageIndex { get { return _pageIndex - 1; } }

        public bool IsSegmented { get { return _skipPages > 0 || _skipPages + 1 + _takePages < _noOfPages; } }

        public bool IsEmpty { get { return _noOfPages < 1; } }

        public bool IsFirstSegment { get { return _skipPages == 0; } }

        public bool IsLastSegment { get { return _skipPages + 1 + _takePages >= _noOfPages; } }
    }

}
