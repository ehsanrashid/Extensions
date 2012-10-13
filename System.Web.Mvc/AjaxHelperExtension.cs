namespace System.Web.Mvc
{
    using Collections.Generic;
    using Html;
    using Ajax;
    using Routing;
    

    public static class AjaxHelperExtension
    {


        #region Input

        public static MvcHtmlString ImageActionLink(this AjaxHelper ajaxHelper, string imageUrl, string altText, string actionName, object routeValues, AjaxOptions ajaxOptions)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", imageUrl);
            builder.MergeAttribute("alt", altText);
            var link = ajaxHelper.ActionLink("[replaceme]", actionName, routeValues, ajaxOptions).ToHtmlString();
            return MvcHtmlString.Create(link.Replace("[replaceme]", builder.ToString(TagRenderMode.SelfClosing)));
        }

        #endregion

        #region Pager
        #region Microsoft Ajax Pager
        public static String Pager(this AjaxHelper ajaxHelper, int noOfPages, int pageIndex, String actionName, String controllerName, String routeName, PagerOptions pagerOptions, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            var builder = new PagerBuilder
                (
                    ajaxHelper,
                    actionName,
                    controllerName,
                    noOfPages,
                    pageIndex,
                    pagerOptions,
                    routeName,
                    new RouteValueDictionary(routeValues),
                    ajaxOptions,
                    new RouteValueDictionary(htmlAttributes)
                );
            return builder.RenderPager();
        }

        public static String Pager(this AjaxHelper ajaxHelper, int noOfPages, int pageIndex, String actionName, String controllerName, String routeName, PagerOptions pagerOptions, RouteValueDictionary routeValues, AjaxOptions ajaxOptions, IDictionary<String, object> htmlAttributes)
        {
            var builder = new PagerBuilder
                (
                    ajaxHelper,
                    actionName,
                    controllerName,
                    noOfPages,
                    pageIndex,
                    pagerOptions,
                    routeName,
                    routeValues,
                    ajaxOptions,
                    htmlAttributes
                );
            return builder.RenderPager();
        }

        static String Pager(AjaxHelper ajaxHelper, PagerOptions pagerOptions, IDictionary<String, object> htmlAttributes)
        {
            return new PagerBuilder(null, ajaxHelper, pagerOptions, htmlAttributes).RenderPager();
        }

        // ---

        public static String Pager<T>(this AjaxHelper ajaxHelper, PagedList<T> pagedList, AjaxOptions ajaxOptions)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(ajaxHelper, null, null)
                    : Pager(ajaxHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, null, null, null, ajaxOptions, null);
        }

        public static String Pager<T>(this AjaxHelper ajaxHelper, PagedList<T> pagedList, PagerOptions pagerOptions, AjaxOptions ajaxOptions)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(ajaxHelper, pagerOptions, null)
                    : Pager(ajaxHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, null, pagerOptions, null, ajaxOptions, null);
        }

        public static String Pager<T>(this AjaxHelper ajaxHelper, PagedList<T> pagedList, PagerOptions pagerOptions, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(ajaxHelper, pagerOptions, new RouteValueDictionary(htmlAttributes))
                    : Pager(ajaxHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, null, pagerOptions, null, ajaxOptions, htmlAttributes);
        }

        public static String Pager<T>(this AjaxHelper ajaxHelper, PagedList<T> pagedList, PagerOptions pagerOptions, AjaxOptions ajaxOptions, IDictionary<String, object> htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(ajaxHelper, pagerOptions, htmlAttributes)
                    : Pager(ajaxHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, null, pagerOptions, null, ajaxOptions, htmlAttributes);
        }

        public static String Pager<T>(this AjaxHelper ajaxHelper, PagedList<T> pagedList, String routeName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(ajaxHelper, null, new RouteValueDictionary(htmlAttributes))
                    : Pager(ajaxHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, routeName, null, routeValues, ajaxOptions, htmlAttributes);
        }

        public static String Pager<T>(this AjaxHelper ajaxHelper, PagedList<T> pagedList, String routeName, RouteValueDictionary routeValues, AjaxOptions ajaxOptions, IDictionary<String, object> htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(ajaxHelper, null, htmlAttributes)
                    : Pager(ajaxHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, routeName, null, routeValues, ajaxOptions, htmlAttributes);
        }

        public static String Pager<T>(this AjaxHelper ajaxHelper, PagedList<T> pagedList, String routeName, object routeValues, PagerOptions pagerOptions, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(ajaxHelper, pagerOptions, new RouteValueDictionary(htmlAttributes))
                    : Pager(ajaxHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, routeName, pagerOptions, routeValues, ajaxOptions, htmlAttributes);
        }

        public static String Pager<T>(this AjaxHelper ajaxHelper, PagedList<T> pagedList, String routeName, RouteValueDictionary routeValues, PagerOptions pagerOptions, AjaxOptions ajaxOptions, IDictionary<String, object> htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(ajaxHelper, pagerOptions, htmlAttributes)
                    : Pager(ajaxHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, routeName, pagerOptions, routeValues, ajaxOptions, htmlAttributes);
        }
        #endregion
        #endregion


    }
}
