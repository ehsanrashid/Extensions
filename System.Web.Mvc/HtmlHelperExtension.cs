
namespace System.Web.Mvc
{
    using Collections.Generic;
    using Html;
    using Text;
    using Linq;
    using Linq.Expressions;
    using Routing;

    ///<summary>
    /// A bunch of HTML helper extensions
    ///</summary>
    public static class HtmlHelperExtension
    {
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
            tbImg.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

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
        public static MvcHtmlString Image(this HtmlHelper htmlHelper, String src, String alt = null,
                                          Object htmlAttributes = null)
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
        public static MvcHtmlString Label(this HtmlHelper htmlHelper, String target, String text,
                                          Object htmlAttributes = null)
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


        public static MvcHtmlString DropDownListNull(this HtmlHelper html, String name, SelectList selectList,
                                                     object htmlAttributes)
        {
            return (null == selectList || !selectList.Any())
                       ? MvcHtmlString.Empty
                       : html.DropDownList(name, selectList, htmlAttributes);
        }


        // --------------------------------------------------------------------

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
        public static MvcHtmlString CheckBoxFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
                                                                Expression<Func<TModel, TValue>> expression,
                                                                object htmlAttributes, String checkedValue)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            //var htmlFieldName = ExpressionHelper.GetExpressionText(expression);

            var tag = new TagBuilder("input");
            tag.Attributes.Add("type", "checkbox");
            tag.Attributes.Add("name", metadata.PropertyName);

            tag.Attributes.Add("value", checkedValue.IsNullOrEmpty() ? metadata.Model.ToString() : checkedValue);

            if (htmlAttributes != null)
            {
                tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            }

            if (metadata.Model.ToString() == checkedValue)
            {
                tag.Attributes.Add("checked", "checked");
            }
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.SelfClosing));
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
        public static MvcHtmlString CheckBoxFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
                                                                Expression<Func<TModel, TValue>> expression,
                                                                object htmlAttributes)
        {

            return CheckBoxFor(htmlHelper, expression, htmlAttributes, "");
        }

        /// <summary>
        /// Checks the box for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="htmlHelper">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>Checkbox</returns>
        public static MvcHtmlString CheckBoxFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
                                                                Expression<Func<TModel, TValue>> expression)
        {
            return CheckBoxFor(htmlHelper, expression, new RouteDirection());
        }

        public static String CheckBoxList(this HtmlHelper htmlHelper, String name, IEnumerable<SelectListItem> items, IDictionary<String, object> checkboxHtmlAttributes)
        {
            var output = new StringBuilder();
            foreach (var item in items)
            {
                output.Append("<div class=\"fields\"><label>");
                var checkboxList = new TagBuilder("input");
                checkboxList.MergeAttribute("type", "checkbox");
                checkboxList.MergeAttribute("name", name);
                checkboxList.MergeAttribute("value", item.Value);

                // Check to see if it's checked
                if (item.Selected) checkboxList.MergeAttribute("checked", "checked");

                // Add any attributes
                if (checkboxHtmlAttributes != null) checkboxList.MergeAttributes(checkboxHtmlAttributes);

                checkboxList.SetInnerText(item.Text);
                output.Append(checkboxList.ToString(TagRenderMode.SelfClosing));
                output.Append("&nbsp; " + item.Text + "</label></div>");
            }

            return output.ToString();
        }

        public static String CheckBoxList(this HtmlHelper htmlHelper, String name, IDictionary<String, String> items,
                                         IEnumerable<String> selectedValues,
                                         IDictionary<String, object> checkboxHtmlAttributes)
        {

            var selectListItems = from item in items
                                  select new SelectListItem
                                  {
                                      Text = item.Key,
                                      Value = item.Value,
                                      Selected = (selectedValues != null && selectedValues.Contains(item.Value))
                                  };

            return CheckBoxList(htmlHelper, name, selectListItems, checkboxHtmlAttributes);
        }

        public static String CheckBoxList(this HtmlHelper htmlHelper, String name, IEnumerable<SelectListItem> items)
        {
            return CheckBoxList(htmlHelper, name, items, null);
        }

        public static String CheckBoxList(this HtmlHelper htmlHelper, String name, IDictionary<String, String> items)
        {
            return CheckBoxList(htmlHelper, name, items, null, null);
        }

        public static String CheckBoxList(this HtmlHelper htmlHelper, String name, IDictionary<String, String> items, IDictionary<String, object> checkboxHtmlAttributes)
        {
            return CheckBoxList(htmlHelper, name, items, null, checkboxHtmlAttributes);
        }

        public static String CheckBoxList(this HtmlHelper htmlHelper, String name, IDictionary<String, String> items, IEnumerable<String> selectedValues)
        {
            return CheckBoxList(htmlHelper, name, items, selectedValues, null);
        }


        //public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty[]>> expression, MultiSelectList multiSelectList, object htmlAttributes = null)
        //{
        //    //Derive property name for checkbox name
        //    var body = expression.Body as MemberExpression;
        //    var propertyName = body.Member.Name;

        //    //Get currently select values from the ViewData model
        //    var list = expression.Compile().Invoke(htmlHelper.ViewData.Model);

        //    //Convert selected value list to a List<String> for easy manipulation
        //    var selectedValues = new List<String>();

        //    if (null != list)
        //    {
        //        selectedValues = new List<TProperty>(list).ConvertAll(item => item.ToString());
        //    }

        //    //Create div
        //    var divTag = new TagBuilder("div");
        //    divTag.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

        //    //Add checkboxes
        //    foreach (var item in multiSelectList)
        //    {
        //        divTag.InnerHtml += String.Format("<div><input type=\"checkbox\" name=\"{0}\" id=\"{0}_{1}\" " +
        //                                            "value=\"{1}\" {2} /><label for=\"{0}_{1}\">{3}</label></div>",
        //                                            propertyName,
        //                                            item.Value,
        //                                            selectedValues.Contains(item.Value) ? "checked=\"checked\"" : "",
        //                                            item.Text);
        //    }

        //    return MvcHtmlString.Create(divTag.ToString());
        //}


        //public static String[] CheckBoxList(this HtmlHelper htmlHelper, String htmlName, object dataSource, String textField, String valueField, object selectedValue, RouteValueDictionary htmlAttributes)
        //{
        //    var list = new List<String>();
        //    var dictionary = MvcControlDataBinder.SourceToDictionary(dataSource, valueField, textField);
        //    foreach (object obj2 in dictionary.Keys)
        //    {
        //        String str = obj2.ToString();
        //        String text = (dictionary[obj2] == null) ? String.Empty : dictionary[obj2].ToString();
        //        bool isChecked = selectedValue.Equals(str);
        //        String item = CheckBoxBuilder.CheckBox(htmlName, text, str, isChecked, htmlAttributes);
        //        list.Add(item);
        //    }
        //    return list.ToArray();
        //}

    }
}