namespace System.Web.Mvc
{
    using Collections.Generic;
    using Html;
    using Ajax;
    using Routing;
    using System.Text;
    using UI;
    using IO;

    public static class AjaxHelperExtension
    {
        #region DeleteLink

        public static MvcHtmlString DeleteLink<TModel>(this AjaxHelper<TModel> ajaxHelper, String linkText, String actionName, String controllerName = null, Object routeValues = null, IDictionary<String, Object> htmlAttributes = null)
        {
            var ajaxOptDelete = new AjaxOptions
                                {
                                    Confirm = "Are you sure you want to delete this item?",
                                    HttpMethod = "DELETE",
                                    OnSuccess = "function() { window.location.reload(); }"
                                };
            return ajaxHelper.ActionLink(linkText, actionName, controllerName, routeValues, ajaxOptDelete, htmlAttributes);
        }

        public static MvcHtmlString DeleteLink<TModel>(this AjaxHelper<TModel> ajaxHelper, String linkText, String actionName, String controllerName, Object routeValues, Object htmlAttributes)
        {
            return DeleteLink(ajaxHelper, linkText, actionName, controllerName, routeValues, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        #endregion

        #region Input

        public static MvcHtmlString TextBox(this AjaxHelper ajaxHelper, String name, AjaxOptions ajaxOptions, IDictionary<String, Object> htmlAttributes)
        {
            using (var stringWriter = new StringWriter())
            using (var htmlWriter = new HtmlTextWriter(stringWriter))
            {
                htmlWriter.AddAttribute(HtmlTextWriterAttribute.Name, name);
                htmlWriter.AddAttribute(HtmlTextWriterAttribute.Type, "Text");
                foreach (var attrib in (ajaxOptions ?? new AjaxOptions()).ToUnobtrusiveHtmlAttributes())
                    htmlWriter.AddAttribute(attrib.Key, attrib.Value.ToString());
                if (null != htmlAttributes)
                    foreach (var attrib in htmlAttributes)
                        htmlWriter.AddAttribute(attrib.Key, attrib.Value.ToString());
                htmlWriter.RenderBeginTag(HtmlTextWriterTag.Input);
                htmlWriter.RenderEndTag();
                return MvcHtmlString.Create(stringWriter.ToString());
            }
        }

        public static MvcHtmlString TextBox(this AjaxHelper ajaxHelper, String name, AjaxOptions ajaxOptions, Object htmlAttributes)
        {
            return TextBox(ajaxHelper, name, ajaxOptions, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }


        public static MvcHtmlString ImageActionLink(this AjaxHelper ajaxHelper, String imageUrl, String altText, String actionName, Object routeValues, AjaxOptions ajaxOptions)
        {
            using (var stringWriter = new StringWriter())
            using (var htmlWriter = new HtmlTextWriter(stringWriter))
            {
                var linkHtml = ajaxHelper.ActionLink("[LinkText]", actionName, routeValues, ajaxOptions).ToHtmlString();
                htmlWriter.AddAttribute(HtmlTextWriterAttribute.Src, imageUrl);
                htmlWriter.AddAttribute(HtmlTextWriterAttribute.Alt, altText);
                htmlWriter.RenderBeginTag(HtmlTextWriterTag.Img);
                htmlWriter.RenderEndTag();
                return MvcHtmlString.Create(linkHtml.Replace("[LinkText]", stringWriter.ToString()));
            }
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

    }
}
