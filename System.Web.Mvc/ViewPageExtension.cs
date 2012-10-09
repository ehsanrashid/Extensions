
namespace System.Web.Mvc
{
    using Collections.Generic.Interface;
    using UI;


    public static class ViewPageExtension
    {

        public static String GetDefaultPageTitle<T>(this ViewPage<T> viewPage)
            where T : class
        {
            return "";
        }


        /// <summary>
        /// Shows a pager control - Creates a list of links that jump to each page
        /// </summary>
        /// <param name="page">The ViewPage instance this method executes on.</param>
        /// <param name="pagedList">A PagedList instance containing the data for the paged control</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action on the controller.</param>
        public static void PagerControl<T>(this ViewPage page, IPagedList<T> pagedList, String controllerName, String actionName)
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