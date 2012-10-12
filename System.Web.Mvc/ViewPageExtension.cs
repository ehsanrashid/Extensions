
namespace System.Web.Mvc
{
    using Collections.Generic.Interface;
    using UI;

    /// <summary>
    /// The implementation creates a list of page numbers, with a link on each except for the current page.
    /// The link will be of the format "/{controller name}/{action name}/{page number}".
    /// 
    /// 1) In your controller, create an action which takes the page number as it's sole parameter. The action would then create a new instance of the PagedList, passing it the "full list" and the current page number from the parameter.
    /// public ActionResult Page(int id)
    /// {
    ///     List<Product> products = CatalogService.ListOpenProducts();
    ///     PagedList<Product> data = new PagedList<Product>(products, id, PAGE_SIZE);
    ///     return View(data);
    /// }
    /// 
    /// 2) Change / create a view which takes the PagedList<your row type> as it's model.
    /// 
    /// @model PagedList<Product>
    /// 
    /// 3) Place a call to the extension method to display the pager anywhere in your view 
    /// (multiple placement allowed – you can put one on top and one on the bottom etc). 
    /// Recall that the method ShowPagerControl() extends  ViewPage, 
    /// so the keyword this should show your intellisence for the method. 
    /// If you chose a more complex model (MVVM ViewModel containing more data than just the paged list) 
    /// then you would use Model.{paged list property name}. 
    /// The use of the view as a bag of random data conjured up by string names should IMHO be universally abandoned and eliminated.
    /// 
    /// @this.Pager<Product>(Model, "Bids", "Page");
    /// 
    /// </summary>
    public static class ViewPageExtension
    {

        public static String GetDefaultPageTitle<T>(this ViewPage<T> viewPage)
            where T : class
        {
            return String.Empty;
        }


        /// <summary>
        /// Shows a pager control - Creates a list of links that jump to each page
        /// </summary>
        /// <param name="page">The ViewPage instance this method executes on.</param>
        /// <param name="pagedList">A PagedList instance containing the data for the paged control</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action on the controller.</param>
        public static void Pager<T>(this ViewPage page, IPagedList<T> pagedList, String controllerName, String actionName)
        {
            var writer = new HtmlTextWriter(page.Response.Output);
            for (var pageNum = 1; pageNum <= pagedList.NoOfPages; ++pageNum)
            {
                if (pageNum != pagedList.PageIndex)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "/" + controllerName + "/" + actionName + "/" + pageNum);
                    writer.AddAttribute(HtmlTextWriterAttribute.Alt, "Page " + pageNum);
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                }

                writer.AddAttribute(HtmlTextWriterAttribute.Class,
                                    pageNum == pagedList.PageIndex
                                        ? "pageLinkCurrent"
                                        : "pageLink");

                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.Write(pageNum);
                writer.RenderEndTag();

                if (pageNum != pagedList.PageIndex)
                {
                    writer.RenderEndTag();
                }
                writer.Write("&nbsp;");
            }

            writer.Write(String.Concat("(", pagedList.NoOfItems, " items in all)"));
        }


    }
}