namespace System.Web.Mvc
{
    using Collections.Generic;
    using Html;
    using Ajax;
    using Routing;
    using System.Text;

    public static class AjaxHelperExtension
    {

        public static MvcHtmlString TextBox(this AjaxHelper ajaxHelper, String name, AjaxOptions ajaxOptions, Object htmlAttributes)
        {
            var tag = new TagBuilder("input");
            tag.MergeAttribute("name", name);
            tag.MergeAttribute("type", "text");

            tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            tag.MergeAttributes((ajaxOptions ?? new AjaxOptions()).ToUnobtrusiveHtmlAttributes());

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        #region Input

        public static MvcHtmlString ImageActionLink(this AjaxHelper ajaxHelper, String imageUrl, String altText, String actionName, Object routeValues, AjaxOptions ajaxOptions)
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
        public static String Pager(this AjaxHelper ajaxHelper, int noOfPages, int pageIndex, String actionName, String controllerName, String routeName, PagerOptions pagerOptions, Object routeValues, AjaxOptions ajaxOptions, Object htmlAttributes)
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

        public static String Pager(this AjaxHelper ajaxHelper, int noOfPages, int pageIndex, String actionName, String controllerName, String routeName, PagerOptions pagerOptions, RouteValueDictionary routeValues, AjaxOptions ajaxOptions, IDictionary<String, Object> htmlAttributes)
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

        static String Pager(AjaxHelper ajaxHelper, PagerOptions pagerOptions, IDictionary<String, Object> htmlAttributes)
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

        public static String Pager<T>(this AjaxHelper ajaxHelper, PagedList<T> pagedList, PagerOptions pagerOptions, AjaxOptions ajaxOptions, Object htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(ajaxHelper, pagerOptions, new RouteValueDictionary(htmlAttributes))
                    : Pager(ajaxHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, null, pagerOptions, null, ajaxOptions, htmlAttributes);
        }

        public static String Pager<T>(this AjaxHelper ajaxHelper, PagedList<T> pagedList, PagerOptions pagerOptions, AjaxOptions ajaxOptions, IDictionary<String, Object> htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(ajaxHelper, pagerOptions, htmlAttributes)
                    : Pager(ajaxHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, null, pagerOptions, null, ajaxOptions, htmlAttributes);
        }

        public static String Pager<T>(this AjaxHelper ajaxHelper, PagedList<T> pagedList, String routeName, Object routeValues, AjaxOptions ajaxOptions, Object htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(ajaxHelper, null, new RouteValueDictionary(htmlAttributes))
                    : Pager(ajaxHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, routeName, null, routeValues, ajaxOptions, htmlAttributes);
        }

        public static String Pager<T>(this AjaxHelper ajaxHelper, PagedList<T> pagedList, String routeName, RouteValueDictionary routeValues, AjaxOptions ajaxOptions, IDictionary<String, Object> htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(ajaxHelper, null, htmlAttributes)
                    : Pager(ajaxHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, routeName, null, routeValues, ajaxOptions, htmlAttributes);
        }

        public static String Pager<T>(this AjaxHelper ajaxHelper, PagedList<T> pagedList, String routeName, Object routeValues, PagerOptions pagerOptions, AjaxOptions ajaxOptions, Object htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(ajaxHelper, pagerOptions, new RouteValueDictionary(htmlAttributes))
                    : Pager(ajaxHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, routeName, pagerOptions, routeValues, ajaxOptions, htmlAttributes);
        }

        public static String Pager<T>(this AjaxHelper ajaxHelper, PagedList<T> pagedList, String routeName, RouteValueDictionary routeValues, PagerOptions pagerOptions, AjaxOptions ajaxOptions, IDictionary<String, Object> htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(ajaxHelper, pagerOptions, htmlAttributes)
                    : Pager(ajaxHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, routeName, pagerOptions, routeValues, ajaxOptions, htmlAttributes);
        }
        #endregion
        #endregion

        #region Link
        
        public static MvcHtmlString DeleteLink<TModel>(this AjaxHelper<TModel> ajaxHelper, String linkText, String actionName, String controllerName = null, Object routeValues = null, IDictionary<String, Object> htmlAttributes = null)
        {
            return ajaxHelper.ActionLink(linkText, actionName, controllerName, routeValues,
                                new AjaxOptions
                                {
                                    Confirm = "Are you sure you want to delete this item?",
                                    HttpMethod = "DELETE",
                                    OnSuccess = "function() { window.location.reload(); }"
                                }, htmlAttributes);
        }

        public static MvcHtmlString DeleteLink<TModel>(this AjaxHelper<TModel> ajaxHelper, String linkText, String actionName, String controllerName = null, Object routeValues = null, Object htmlAttributes = null)
        {
            return DeleteLink(ajaxHelper, linkText, actionName, controllerName, routeValues, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        #endregion
    }
}
