namespace System.Web.Mvc
{
    using Collections.Generic;
    using Html;
    using Text;
    using Linq;
    using Linq.Expressions;
    using Routing;
    using Ajax;

    ///<summary>
    /// A bunch of HTML helper extensions
    ///</summary>
    public static class HtmlHelperExtension
    {
        #region Controls

        ///<summary>
        /// Returns an HTML image element for the given image options
        ///</summary>
        ///<param name="htmlHelper"></param>
        ///<param name="imgSrc">Image source</param>
        ///<param name="alt">Image alt text</param>
        ///<param name="actionName">Link action name</param>
        ///<param name="controllerName">Link controller name</param>
        ///<param name="routeValues">Link route values</param>
        ///<param name="htmlAttributes">Link html attributes</param>
        ///<param name="imgHtmlAttributes">Image html attributes</param>
        ///<returns>MvcHtmlString</returns>
        public static MvcHtmlString ImageLink(this HtmlHelper htmlHelper, String imgSrc = null, String alt = null,
                                              String actionName = null, String controllerName = null,
                                              Object routeValues = null, Object htmlAttributes = null,
                                              Object imgHtmlAttributes = null)
        {
            var urlHelper = ((Controller) htmlHelper.ViewContext.Controller).Url;

            var url = urlHelper.Action(actionName, controllerName, routeValues);
            var tbLink = new TagBuilder("a");
            tbLink.MergeAttribute("href", url);

            var tbImg = new TagBuilder("img");
            tbImg.MergeAttribute("src", imgSrc);
            tbImg.MergeAttributes(new RouteValueDictionary(imgHtmlAttributes), true);

            tbLink.InnerHtml = tbImg.ToString(TagRenderMode.SelfClosing);
            tbLink.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

            return MvcHtmlString.Create(tbLink.ToString());
        }

        ///<summary>
        /// Returns an HTML image element for the given source and alt text.
        ///</summary>
        ///<param name="htmlHelper"></param>
        ///<param name="src"></param>
        ///<param name="alt"></param>
        ///<param name="htmlAttributes"></param>
        ///<returns>MvcHtmlString</returns>
        public static MvcHtmlString Image(this HtmlHelper htmlHelper, String src, String alt = null, Object htmlAttributes = null)
        {
            var tbImg = new TagBuilder("img");
            tbImg.Attributes.Add("src", htmlHelper.Encode(src));
            tbImg.Attributes.Add("alt", htmlHelper.Encode(alt));
            tbImg.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);
            return MvcHtmlString.Create(tbImg.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Returns an HTML label element for the given target and text.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="target"></param>
        /// <param name="text"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString Label(this HtmlHelper htmlHelper, String target, String text, Object htmlAttributes = null)
        {
            var tbLbl = new TagBuilder("label");
            tbLbl.MergeAttribute("for", target);
            tbLbl.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);
            tbLbl.SetInnerText(text);

            return MvcHtmlString.Create(tbLbl.ToString());
        }

        private static readonly String[] HtmlTags = new[]
                                                    {
                                                        "script", "div", "p", "a", "h1", "h2", "h3", "h4", "h5", "h6",
                                                        "center", "table", "form"
                                                    };

        public static MvcHtmlString Tag(this HtmlHelper htmlHelper,
                                        String tag = null, String src = null, String href = null,
                                        String type = null, String id = null, String name = null,
                                        String style = null, String @class = null, String attribs = null)
        {
            var sb = new StringBuilder();
            sb.Append("<");

            if (tag.IsNotNullOrEmpty()) tag = "div";

            sb.Append(tag);
            AppendOptionalAttrib(htmlHelper, sb, "id", id, false, validateScriptableIdent: true);
            AppendOptionalAttrib(htmlHelper, sb, "name", name, false, validateScriptableIdent: true);
            AppendOptionalAttrib(htmlHelper, sb, "type", type);
            AppendOptionalAttrib(htmlHelper, sb, "src", src, false, true);
            AppendOptionalAttrib(htmlHelper, sb, "href", href, false, true);
            AppendOptionalAttrib(htmlHelper, sb, "style", style);
            AppendOptionalAttrib(htmlHelper, sb, "class", @class, false, validateClass: true);

            if (attribs.IsNotNullOrEmpty()) sb.Append(" " + attribs);

            if (HtmlTags.Contains(tag.ToLower())) sb.Append("></" + tag + ">");
            else sb.Append(" />");
            return MvcHtmlString.Create(sb.ToString());
        }

        private static void AppendOptionalAttrib(HtmlHelper htmlHelper,
                                                 StringBuilder sb, String attribName, String attribValue,
                                                 bool? encode = null,
                                                 bool? resolveAbsUrl = null, bool? validateScriptableIdent = null,
                                                 bool? validateClass = null)
        {
            if (attribValue.IsNullOrEmpty()) return;
            if (attribName.IsNullOrEmpty()) throw new ArgumentException("attribName is required.", "attribName");

            var attribNameLcase = attribName.ToLower();

            if (!resolveAbsUrl.HasValue) resolveAbsUrl = attribNameLcase == "src" || attribNameLcase == "href";
            if (!validateScriptableIdent.HasValue) validateScriptableIdent = attribNameLcase == "id" || attribNameLcase == "name";
            if (!validateClass.HasValue) validateClass = attribNameLcase == "class";
            if (!encode.HasValue) encode = !validateScriptableIdent.Value && !resolveAbsUrl.Value && !validateClass.Value;
            sb.Append(String.Concat(" ", attribName, "=\""));

            if (validateScriptableIdent.Value && !IsScriptableIdValue(attribValue)) throw new FormatException("Attrib value has invalid characters: " + attribNameLcase + "=" + attribValue);
            if (validateClass.Value && !IsValidClassValue(attribValue)) throw new FormatException("Attrib value has invalid characters: " + attribNameLcase + "=" + attribValue);
            if (resolveAbsUrl.Value)
                try
                {
                    sb.Append(VirtualPathUtility.ToAbsolute(attribValue));
                }
                catch (ArgumentException e)
                {
                    if (e.Message.Contains("is not allowed here"))
                        throw new ArgumentException(e.Message + " (Try prefixing the app root, i.e. \"~/\".)",
                                                    e.ParamName);
                    throw;
                }
            else if (encode.Value) sb.Append(htmlHelper.Encode(attribValue));
            else sb.Append(attribValue);
            sb.Append("\"");
        }

        private static bool IsValidClassValue(String value)
        {
            const String numeric = "1234567890";
            const String alphanumeric = "abcdefghijklmnopqrstuvwxyz_- " + numeric;

            if (value.IsNullOrEmpty()) return false;

            value = value.ToLower();
            return value.All(c => alphanumeric.Contains(c));
        }

        private static bool IsScriptableIdValue(String value)
        {
            const String numeric = "1234567890";
            const String alphanumeric = "abcdefghijklmnopqrstuvwxyz_" + numeric;

            if (value.IsNullOrEmpty()) return false;

            value = value.ToLower();
            return !numeric.Contains(value[0]) && value.All(c => alphanumeric.Contains(c));
        }

        public static MvcHtmlString CurrentAction(this HtmlHelper htmlHelper)
        {
            return MvcHtmlString.Create(htmlHelper.ViewContext.RouteData.Values["action"].ToString());
        }

        public static MvcHtmlString CurrentController(this HtmlHelper htmlHelper)
        {
            return MvcHtmlString.Create(htmlHelper.ViewContext.RouteData.Values["controller"].ToString());
        }

        //public static MvcForm BeginForm(this HtmlHelper htmlHelper, Object routeValues, FormMethod method, Object htmlAttributes)
        //{
        //  return htmlHelper.BeginForm(Html.CurrentAction, Html.CurrentController, routeValues, method, htmlAttributes);
        //}

        public static MvcHtmlString Render(this HtmlHelper htmlHelper, String content)
        {
            return MvcHtmlString.Create(content);
        }


        public static MvcHtmlString DropDownListNull(this HtmlHelper html, String name, SelectList selectList, Object htmlAttributes)
        {
            return (null == selectList || !selectList.Any())
                       ? MvcHtmlString.Empty
                       : html.DropDownList(name, selectList, htmlAttributes);
        }

        #endregion

        // --------------------------------------------------------------------

        #region CheckBoxList

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="items"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        /// <code>
        /// ${Html.CheckBoxList(“Product.Categories”,
        ///     ViewData.Model.Categories.ToDictionary(c => c.Name, c => c.Id.ToString()),
        ///     ViewData.Model.Product.Categories.Select(c => c.Id.ToString()))}
        /// </code>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, String name, IEnumerable<SelectListItem> items, IDictionary<String, Object> htmlAttributes)
        {
            var sb = new StringBuilder();
            foreach (var item in items)
            {
                sb.Append("<div class=\"fields\"><label>");
                var tagInput = new TagBuilder("input");
                tagInput.MergeAttribute("type", "checkbox");
                tagInput.MergeAttribute("name", name);
                tagInput.MergeAttribute("value", item.Value);

                // Check to see if it's checked
                if (item.Selected) tagInput.MergeAttribute("checked", "checked");//, true);
                // Add any attributes
                if (null != htmlAttributes) tagInput.MergeAttributes(htmlAttributes);

                tagInput.SetInnerText(item.Text);
                sb.Append(tagInput.ToString(TagRenderMode.SelfClosing));
                sb.Append("&nbsp; " + item.Text + "</label></div>");
            }
            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, String name, IEnumerable<SelectListItem> items)
        {
            return CheckBoxList(htmlHelper, name, items, null);
        }

        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, String name, IDictionary<String, String> items, IEnumerable<String> selectedValues, IDictionary<String, Object> htmlAttributes)
        {
            var selectListItems = from item in items
                                  select new SelectListItem
                                  {
                                      Text = item.Key,
                                      Value = item.Value,
                                      Selected = (null != selectedValues && selectedValues.Contains(item.Value))
                                  };

            return CheckBoxList(htmlHelper, name, selectListItems, htmlAttributes);
        }

        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, String name, IDictionary<String, String> items, IEnumerable<String> selectedValues)
        {
            return CheckBoxList(htmlHelper, name, items, selectedValues, null);
        }

        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, String name, IDictionary<String, String> items, IDictionary<String, Object> htmlAttributes)
        {
            return CheckBoxList(htmlHelper, name, items, null, htmlAttributes);
        }

        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, String name, IDictionary<String, String> items)
        {
            return CheckBoxList(htmlHelper, name, items, null, null);
        }

        public static MvcHtmlString CheckBoxList(this HtmlHelper helper, String name, Dictionary<Int32, String> items, bool isVertical, String cssClass)
        {
            var sb = new StringBuilder();
            sb.Append(String.Format("<div >"));
            foreach (var item in items)
            {
                sb.Append(helper.CheckBox(item.Value, true, new { @class = cssClass, value = item.Key }));
                sb.Append(helper.Label("RadioButtonItems", item.Value));
                sb.Append("&nbsp;");
                if (isVertical) sb.Append("<br>");
            }
            sb.Append("</div> ");
            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, List<SelectListItem> listSelectItem, String modelCollectionName)
        {
            var sb = new StringBuilder();

            if (null != listSelectItem)
            {
                var i = 0;

                foreach (var item in listSelectItem)
                {
                    var collectionNameIndex = String.Format("{0}[{1}]", modelCollectionName, i);

                    var tagHiddenName = new TagBuilder("input");
                    tagHiddenName.Attributes.Add(new KeyValuePair<String, String>("type", "hidden"));
                    tagHiddenName.Attributes.Add(new KeyValuePair<String, String>("name", String.Format("{0}.{1}", collectionNameIndex, "Text")));
                    tagHiddenName.Attributes.Add(new KeyValuePair<String, String>("value", item.Text));

                    var tagHiddenValue = new TagBuilder("input");
                    tagHiddenValue.Attributes.Add(new KeyValuePair<String, String>("type", "hidden"));
                    tagHiddenValue.Attributes.Add(new KeyValuePair<String, String>("name", String.Format("{0}.{1}", collectionNameIndex, "Value")));
                    tagHiddenValue.Attributes.Add(new KeyValuePair<String, String>("value", item.Value));

                    var checkBoxTag = htmlHelper.CheckBox(String.Format("{0}.{1}", collectionNameIndex, "Selected"), item.Selected);

                    var tagLabel = new TagBuilder("label");
                    tagLabel.Attributes.Add(new KeyValuePair<String, String>("for", String.Format("{0}.{1}", collectionNameIndex, "Name")));
                    tagLabel.SetInnerText(item.Text);

                    sb.Append(tagHiddenName);
                    sb.Append(tagHiddenValue);
                    sb.Append(checkBoxTag);
                    sb.Append(tagLabel);
                    sb.Append("<br/>");

                    ++i;
                }
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        //public static String[] CheckBoxList(this HtmlHelper htmlHelper, String htmlName, Object dataSource, String textField, String valueField, Object selectedValue, RouteValueDictionary htmlAttributes)
        //{
        //    var list = new List<String>();
        //    var dictionary = MvcControlDataBinder.SourceToDictionary(dataSource, valueField, textField);
        //    foreach (Object obj2 in dictionary.Keys)
        //    {
        //        String str = obj2.ToString();
        //        String text = (dictionary[obj2] == null) ? String.Empty : dictionary[obj2].ToString();
        //        bool isChecked = selectedValue.Equals(str);
        //        String item = CheckBoxBuilder.CheckBox(htmlName, text, str, isChecked, htmlAttributes);
        //        list.Add(item);
        //    }
        //    return list.ToArray();
        //}
        #endregion

        #region CheckBoxFor

        /// <summary>
        /// Checks the box for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="htmlHelper">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="checkedValue">The checked value.</param>
        /// <returns>Checkbox</returns>
        public static MvcHtmlString CheckBoxFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, Object htmlAttributes, String checkedValue)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            //var htmlFieldName = ExpressionHelper.GetExpressionText(expression);

            var tagInput = new TagBuilder("input");
            tagInput.Attributes.Add("type", "checkbox");
            tagInput.Attributes.Add("name", metadata.PropertyName);
            tagInput.Attributes.Add("value", checkedValue.IsNullOrEmpty() ? metadata.Model.ToString() : checkedValue);

            if (metadata.Model.ToString() == checkedValue) tagInput.MergeAttribute("checked", "checked");

            if (null != htmlAttributes) tagInput.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            return MvcHtmlString.Create(tagInput.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Checks the box for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="htmlHelper">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>Checkbox</returns>
        public static MvcHtmlString CheckBoxFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, Object htmlAttributes)
        {
            return CheckBoxFor(htmlHelper, expression, htmlAttributes, String.Empty);
        }

        /// <summary>
        /// Checks the box for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="htmlHelper">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>Checkbox</returns>
        public static MvcHtmlString CheckBoxFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression)
        {
            return CheckBoxFor(htmlHelper, expression, new RouteDirection());
        }

        #endregion

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty[]>> expression, MultiSelectList multiSelectList, Object htmlAttributes = null)
        {
            //Derive property name for checkbox name
            var body = expression.Body as MemberExpression;
            var propertyName = body.Member.Name;

            //Get currently select values from the ViewData model
            var list = expression.Compile().Invoke(htmlHelper.ViewData.Model);

            //Convert selected value list to a List<String> for easy manipulation
            var selectedValues = new List<String>();

            if (null != list) selectedValues = new List<TProperty>(list).ConvertAll(item => item.ToString());

            //Create div
            var tagDiv = new TagBuilder("div");
            tagDiv.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);
            //Add checkboxes
            foreach (var item in multiSelectList)
            {
                tagDiv.InnerHtml += String.Format("<div><input type=\"checkbox\" name=\"{0}\" id=\"{0}_{1}\" value=\"{1}\" {2} /><label for=\"{0}_{1}\">{3}</label></div>",
                                                    propertyName, item.Value, selectedValues.Contains(item.Value) ? "checked=\"checked\"" : "", item.Text);
            }
            return MvcHtmlString.Create(tagDiv.ToString());
        }


        //public static IHtmlString Raw(this HtmlHelper htmlHelper, String value)
        //{
        //    return new HtmlString(value);
        //}

        //public static IHtmlString Raw(this HtmlHelper htmlHelper, Object value)
        //{
        //    return new HtmlString(value == null ? null : value.ToString());
        //}


        #region Pager
        #region Html Pager
        public static String Pager(this HtmlHelper htmlHelper, int noOfPages, int pageIndex, String actionName, String controllerName, PagerOptions pagerOptions, String routeName, object routeValues, object htmlAttributes)
        {
            var builder = new PagerBuilder
                (
                    htmlHelper,
                    actionName,
                    controllerName,
                    noOfPages,
                    pageIndex,
                    pagerOptions,
                    routeName,
                    new RouteValueDictionary(routeValues),
                    new RouteValueDictionary(htmlAttributes)
                );
            return builder.RenderPager();
        }

        public static String Pager(this HtmlHelper htmlHelper, int noOfPages, int pageIndex, String actionName, String controllerName, PagerOptions pagerOptions, String routeName, RouteValueDictionary routeValues, IDictionary<String, object> htmlAttributes)
        {
            var builder = new PagerBuilder
                (
                    htmlHelper,
                    actionName,
                    controllerName,
                    noOfPages,
                    pageIndex,
                    pagerOptions,
                    routeName,
                    routeValues,
                    htmlAttributes
                );
            return builder.RenderPager();
        }

        // ---

        static String Pager(HtmlHelper htmlHelper, PagerOptions pagerOptions, IDictionary<String, object> htmlAttributes)
        {
            return new PagerBuilder(htmlHelper, null, pagerOptions, htmlAttributes).RenderPager();
        }

        // ---

        public static String Pager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(htmlHelper, null, null)
                    : Pager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, null, null, null, null);
        }

        public static String Pager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, PagerOptions pagerOptions)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(htmlHelper, pagerOptions, null)
                    : Pager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, pagerOptions, null, null, null);
        }

        public static String Pager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, PagerOptions pagerOptions, object htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(htmlHelper, pagerOptions, new RouteValueDictionary(htmlAttributes))
                    : Pager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, pagerOptions, null, null, htmlAttributes);
        }

        public static String Pager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, PagerOptions pagerOptions, IDictionary<String, object> htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(htmlHelper, pagerOptions, htmlAttributes)
                    : Pager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, pagerOptions, null, null, htmlAttributes);
        }


        public static String Pager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, String actionName, String controllerName, PagerOptions pagerOptions, object htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(htmlHelper, 0, 1, actionName, controllerName, pagerOptions, null, null, htmlAttributes)
                    : Pager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, actionName, controllerName, pagerOptions, null, null, htmlAttributes);
        }

        public static String Pager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, String actionName, String controllerName, PagerOptions pagerOptions, IDictionary<String, object> htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(htmlHelper, 0, 1, actionName, controllerName, pagerOptions, null, null, htmlAttributes)
                    : Pager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, actionName, controllerName, pagerOptions, null, null, htmlAttributes);
        }


        public static String Pager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, PagerOptions pagerOptions, String routeName, object routeValues)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(htmlHelper, pagerOptions, null)
                    : Pager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, pagerOptions, routeName, routeValues, null);
        }

        public static String Pager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, PagerOptions pagerOptions, String routeName, RouteValueDictionary routeValues)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(htmlHelper, pagerOptions, null)
                    : Pager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, pagerOptions, routeName, routeValues, null);
        }

        public static String Pager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, PagerOptions pagerOptions, String routeName, object routeValues, object htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(htmlHelper, pagerOptions, new RouteValueDictionary(htmlAttributes))
                    : Pager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, pagerOptions, routeName, routeValues, htmlAttributes);
        }

        public static String Pager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, PagerOptions pagerOptions, String routeName, RouteValueDictionary routeValues, IDictionary<String, object> htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(htmlHelper, pagerOptions, htmlAttributes)
                    : Pager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, pagerOptions, routeName, routeValues, htmlAttributes);
        }

        public static String Pager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, String routeName, object routeValues, object htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(htmlHelper, null, new RouteValueDictionary(htmlAttributes))
                    : Pager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, null, routeName, routeValues, htmlAttributes);
        }

        public static String Pager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, String routeName, RouteValueDictionary routeValues, IDictionary<String, object> htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(htmlHelper, null, htmlAttributes)
                    : Pager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, null, routeName, routeValues, htmlAttributes);
        }

        #endregion

        #region jQuery Ajax Pager
        public static String AjaxPager(this HtmlHelper htmlHelper, int totalPageCount, int pageIndex, String actionName, String controllerName, String routeName, PagerOptions pagerOptions, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            if (null == pagerOptions) pagerOptions = new PagerOptions();
            pagerOptions.UseJqueryAjax = true;
            var builder = new PagerBuilder
                (
                    htmlHelper,
                    actionName,
                    controllerName,
                    totalPageCount,
                    pageIndex,
                    pagerOptions,
                    routeName,
                    new RouteValueDictionary(routeValues),
                    ajaxOptions,
                    new RouteValueDictionary(htmlAttributes)
                );
            return builder.RenderPager();
        }

        public static String AjaxPager(this HtmlHelper htmlHelper, int totalPageCount, int pageIndex, String actionName, String controllerName, String routeName, PagerOptions pagerOptions, RouteValueDictionary routeValues, AjaxOptions ajaxOptions, IDictionary<String, object> htmlAttributes)
        {
            if (null == pagerOptions) pagerOptions = new PagerOptions();
            pagerOptions.UseJqueryAjax = true;
            var builder = new PagerBuilder
                (
                    htmlHelper,
                    actionName,
                    controllerName,
                    totalPageCount,
                    pageIndex,
                    pagerOptions,
                    routeName,
                    routeValues,
                    ajaxOptions,
                    htmlAttributes
                );
            return builder.RenderPager();
        }

        // ---

        static String AjaxPager(HtmlHelper htmlHelper, PagerOptions pagerOptions, IDictionary<String, object> htmlAttributes)
        {
            return new PagerBuilder(htmlHelper, null, pagerOptions, htmlAttributes).RenderPager();
        }

        // ---

        public static String AjaxPager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, AjaxOptions ajaxOptions)
        {
            return (default(PagedList<T>) == pagedList)
                    ? AjaxPager(htmlHelper, null, null)
                    : AjaxPager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, null, null, null, ajaxOptions, null);
        }

        public static String AjaxPager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, String routeName, AjaxOptions ajaxOptions)
        {
            return (default(PagedList<T>) == pagedList)
                    ? AjaxPager(htmlHelper, null, null)
                    : AjaxPager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, routeName, null, null, ajaxOptions, null);
        }

        public static String AjaxPager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, PagerOptions pagerOptions, AjaxOptions ajaxOptions)
        {
            return (default(PagedList<T>) == pagedList)
                    ? AjaxPager(htmlHelper, pagerOptions, null)
                    : AjaxPager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, null, pagerOptions, null, ajaxOptions, null);
        }

        public static String AjaxPager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, PagerOptions pagerOptions, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? AjaxPager(htmlHelper, pagerOptions, new RouteValueDictionary(htmlAttributes))
                    : AjaxPager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, null, pagerOptions, null, ajaxOptions, htmlAttributes);
        }

        public static String AjaxPager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, PagerOptions pagerOptions, AjaxOptions ajaxOptions, IDictionary<String, object> htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? AjaxPager(htmlHelper, pagerOptions, htmlAttributes)
                    : AjaxPager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, null, pagerOptions, null, ajaxOptions, htmlAttributes);
        }

        public static String AjaxPager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, String routeName, object routeValues, PagerOptions pagerOptions, AjaxOptions ajaxOptions)
        {
            return (default(PagedList<T>) == pagedList)
                    ? AjaxPager(htmlHelper, pagerOptions, null)
                    : AjaxPager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, routeName, pagerOptions, routeValues, ajaxOptions, null);
        }

        public static String AjaxPager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, String routeName, object routeValues, PagerOptions pagerOptions, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? AjaxPager(htmlHelper, pagerOptions, new RouteValueDictionary(htmlAttributes))
                    : AjaxPager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, routeName, pagerOptions, routeValues, ajaxOptions, htmlAttributes);
        }

        public static String AjaxPager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, String routeName, RouteValueDictionary routeValues, PagerOptions pagerOptions, AjaxOptions ajaxOptions, IDictionary<String, object> htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? AjaxPager(htmlHelper, pagerOptions, htmlAttributes)
                    : AjaxPager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, routeName, pagerOptions, routeValues, ajaxOptions, htmlAttributes);
        }

        public static String AjaxPager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, String actionName, String controllerName, PagerOptions pagerOptions, AjaxOptions ajaxOptions)
        {
            return (default(PagedList<T>) == pagedList)
                    ? AjaxPager(htmlHelper, pagerOptions, null)
                    : AjaxPager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, actionName, controllerName, null, pagerOptions, null, ajaxOptions, null);
        }

        #endregion
        #endregion

    }
}