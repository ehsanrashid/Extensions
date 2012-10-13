namespace System.Web.Mvc
{
    using Collections.Generic;
    using Collections.Generic.Interface;

    using Html;
    using Text;
    using Linq;
    using Linq.Expressions;
    using Routing;
    using Ajax;
    using IO;
    using UI;

    ///<summary>
    /// A bunch of HTML helper extensions
    ///</summary>
    public static class HtmlHelperExtension
    {
        public static MvcHtmlString Render(this HtmlHelper htmlHelper, String content)
        {
            return MvcHtmlString.Create(content);
        }

        public static MvcHtmlString CurrentAction(this HtmlHelper htmlHelper)
        {
            return MvcHtmlString.Create(htmlHelper.ViewContext.RouteData.Values["action"].ToString());
        }

        public static MvcHtmlString CurrentController(this HtmlHelper htmlHelper)
        {
            return MvcHtmlString.Create(htmlHelper.ViewContext.RouteData.Values["controller"].ToString());
        }

        public static MvcForm BeginForm(this HtmlHelper htmlHelper, Object routeValues, FormMethod method, Object htmlAttributes)
        {
            return htmlHelper.BeginForm(htmlHelper.CurrentAction().ToString(), htmlHelper.CurrentController().ToString(), routeValues, method, htmlAttributes);
        }

        public static MvcHtmlString Truncate(this HtmlHelper helper, String input, int length)
        {
            return MvcHtmlString.Create((input.Length <= length)
                                            ? input
                                            : input.Substring(0, length) + "...");
        }

        #region Label
        /// <summary>
        /// Returns an HTML label element for the given target and text.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="target"></param>
        /// <param name="text"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString LabelFor(this HtmlHelper htmlHelper, String target, String text, IDictionary<String, Object> htmlAttributes = null)
        {
            var tbLbl = new TagBuilder("label");
            tbLbl.MergeAttribute("for", target);
            tbLbl.MergeAttributes(htmlAttributes, true);
            tbLbl.SetInnerText(text);
            return MvcHtmlString.Create(tbLbl.ToString());
        }
        public static MvcHtmlString LabelFor(this HtmlHelper htmlHelper, String target, String text, Object htmlAttributes = null)
        {
            return LabelFor(htmlHelper, target, text, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        #endregion

        #region Image

        ///<summary>
        /// Returns an HTML image element for the given source and alt text.
        ///</summary>
        public static MvcHtmlString Image(this HtmlHelper htmlHelper, String src, String alt = null, IDictionary<String, Object> htmlAttributes = null)
        {
            var stringWriter = new StringWriter();
            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                if (src.IsNotNullOrEmpty()) writer.AddAttribute(HtmlTextWriterAttribute.Src, htmlHelper.Encode(src));
                if (alt.IsNotNullOrEmpty()) writer.AddAttribute(HtmlTextWriterAttribute.Alt, htmlHelper.Encode(alt));
                if (null != htmlAttributes)
                    foreach (var attrib in htmlAttributes)
                        writer.AddAttribute(attrib.Key, attrib.Value.ToString());
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
            }
            return MvcHtmlString.Create(stringWriter.ToString());
        }

        public static MvcHtmlString Image(this HtmlHelper htmlHelper, String src, String alt = null, Object htmlAttributes = null)
        {
            return Image(htmlHelper, src, alt, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString Image(this HtmlHelper htmlHelper, String actionName = null, String controllerName = null, Object routeValues = null, String alt = null, IDictionary<String, Object> htmlAttributes = null)
        {
            var urlHelper = ((Controller) htmlHelper.ViewContext.Controller).Url;
            var url = urlHelper.Action(actionName, controllerName, routeValues);

            return Image(htmlHelper, url, alt, htmlAttributes);
        }

        public static MvcHtmlString Image(this HtmlHelper htmlHelper, String actionName = null, String controllerName = null, Object routeValues = null, String alt = null, Object htmlAttributes = null)
        {
            return Image(htmlHelper, actionName, controllerName, routeValues, alt, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        ///<summary>
        /// Returns an HTML image element for the given image options
        ///</summary>
        ///<param name="htmlHelper"></param>
        ///<param name="actionName">Link action name</param>
        ///<param name="controllerName">Link controller name</param>
        ///<param name="routeValues">Link route values</param>
        ///<param name="imgSrc">Image source</param>
        ///<param name="imgAlt">Image alt text</param>
        ///<param name="aHtmlAttributes">Link html attributes</param>
        ///<param name="imgHtmlAttributes">Image html attributes</param>
        ///<returns>MvcHtmlString</returns>
        public static MvcHtmlString ImageLink(this HtmlHelper htmlHelper, String actionName = null, String controllerName = null, RouteValueDictionary routeValues = null, String imgSrc = null, String imgAlt = null, IDictionary<String, Object> aHtmlAttributes = null, IDictionary<String, Object> imgHtmlAttributes = null)
        {
            var urlHelper = ((Controller) htmlHelper.ViewContext.Controller).Url;
            var url = urlHelper.Action(actionName, controllerName, routeValues);

            var stringWriter = new StringWriter();
            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                if (url.IsNotNullOrEmpty()) writer.AddAttribute(HtmlTextWriterAttribute.Href, url);
                if (null != aHtmlAttributes)
                    foreach (var attrib in aHtmlAttributes)
                        writer.AddAttribute(attrib.Key, attrib.Value.ToString());
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write(htmlHelper.Image(imgSrc, imgAlt, imgHtmlAttributes));
                writer.RenderEndTag();
            }
            return MvcHtmlString.Create(stringWriter.ToString());
        }
        public static MvcHtmlString ImageLink(this HtmlHelper htmlHelper, String actionName = null, String controllerName = null, Object routeValues = null, String imgSrc = null, String imgAlt = null, IDictionary<String, Object> aHtmlAttributes = null, IDictionary<String, Object> imgHtmlAttributes = null)
        {
            return ImageLink(htmlHelper, actionName, controllerName, new RouteValueDictionary(routeValues), imgSrc, imgAlt, aHtmlAttributes, imgHtmlAttributes);
        }

        public static MvcHtmlString ImageLink(this HtmlHelper htmlHelper, String actionName = null, String controllerName = null, Object routeValues = null, String imgSrc = null, String imgAlt = null, Object aHtmlAttributes = null, Object imgHtmlAttributes = null)
        {
            return ImageLink(htmlHelper, actionName, controllerName, routeValues, imgSrc, imgAlt, HtmlHelper.AnonymousObjectToHtmlAttributes(aHtmlAttributes), HtmlHelper.AnonymousObjectToHtmlAttributes(imgHtmlAttributes));
        }

        #endregion

        #region Input

        #region DropDownList

        public static MvcHtmlString DropDownListNull(this HtmlHelper htmlHelper, String name, IEnumerable<SelectListItem> selectList, String optionLabel)
        {
            return (null == selectList || !selectList.Any())
                       ? MvcHtmlString.Empty
                       : htmlHelper.DropDownList(name, selectList, optionLabel);
        }

        public static MvcHtmlString DropDownListNull(this HtmlHelper htmlHelper, String name, IEnumerable<SelectListItem> selectList, Object htmlAttributes)
        {
            return (null == selectList || !selectList.Any())
                       ? MvcHtmlString.Empty
                       : htmlHelper.DropDownList(name, selectList, htmlAttributes);
        }

        public static MvcHtmlString DropDownListNull(this HtmlHelper htmlHelper, String name, IEnumerable<SelectListItem> selectList, IDictionary<String, Object> htmlAttributes)
        {
            return (null == selectList || !selectList.Any())
                       ? MvcHtmlString.Empty
                       : htmlHelper.DropDownList(name, selectList, htmlAttributes);
        }

        public static MvcHtmlString DropDownListNull(this HtmlHelper htmlHelper, String name, IEnumerable<SelectListItem> selectList, String optionLabel, Object htmlAttributes)
        {
            return (null == selectList || !selectList.Any())
                       ? MvcHtmlString.Empty
                       : htmlHelper.DropDownList(name, selectList, optionLabel, htmlAttributes);
        }

        public static MvcHtmlString DropDownListNull(this HtmlHelper htmlHelper, String name, IEnumerable<SelectListItem> selectList, String optionLabel, IDictionary<String, Object> htmlAttributes)
        {
            return (null == selectList || !selectList.Any())
                       ? MvcHtmlString.Empty
                       : htmlHelper.DropDownList(name, selectList, optionLabel, htmlAttributes);
        }
        #endregion

        #region CheckBox

        //----
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, String name, Dictionary<int, String> selectList, bool isVertical, String cssClass)
        {
            var sb = new StringBuilder();
            sb.Append(String.Format("<div >"));
            foreach (var item in selectList)
            {
                sb.Append(htmlHelper.CheckBox(item.Value, true, new { @class = cssClass, value = item.Key }));
                sb.Append(htmlHelper.Label("RadioButtonItems", item.Value));
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
        // ---

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
        public static MvcHtmlString CheckBoxFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, IDictionary<String, Object> htmlAttributes, String checkedValue = null)
        {
            if (null == expression) throw new ArgumentNullException("expression");
            var stringWriter = new StringWriter();
            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "checkBox");

                var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
                if (null != metadata.Model)
                {
                    String value = metadata.Model.ToString();
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, checkedValue.IsNullOrEmpty() ? value : checkedValue);
                    if (value == checkedValue) writer.AddAttribute(HtmlTextWriterAttribute.Checked, "checked");
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, checkedValue ?? String.Empty);
                }
                var name = ExpressionHelper.GetExpressionText(expression); // metadata.PropertyName
                writer.AddAttribute(HtmlTextWriterAttribute.Name, name);

                if (null != htmlAttributes)
                    foreach (var attrib in htmlAttributes)
                        writer.AddAttribute(attrib.Key, attrib.Value.ToString());
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
            }
            return MvcHtmlString.Create(stringWriter.ToString());
        }

        public static MvcHtmlString CheckBoxFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, Object htmlAttributes, String checkedValue = null)
        {
            return CheckBoxFor(htmlHelper, expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), checkedValue);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="selectList"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        /// <code>
        /// ${Html.CheckBoxList(“Product.Categories”,
        ///     ViewData.Model.Categories.ToDictionary(c => c.Name, c => c.Id.ToString()),
        ///     ViewData.Model.Product.Categories.Select(c => c.Id.ToString()))}
        /// </code>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, String name, IEnumerable<SelectListItem> selectList, IDictionary<String, Object> htmlAttributes)
        {
            /*
            var sb = new StringBuilder();
            foreach (var item in selectList)
            {
                sb.Append("<div class=\"checkBox\"><label>");
                var tagInput = new TagBuilder("input");
                tagInput.MergeAttribute("type", "checkbox");
                tagInput.MergeAttribute("name", name);
                tagInput.MergeAttribute("value", item.Value);
                if (item.Selected) tagInput.MergeAttribute("checked", "checked");//, true);
                if (null != htmlAttributes) tagInput.MergeAttributes(htmlAttributes);
                tagInput.SetInnerText(item.Text);
                sb.Append(tagInput.ToString(TagRenderMode.SelfClosing));
                sb.Append("&nbsp; " + item.Text + "</label></div>");
            }
            return MvcHtmlString.Create(sb.ToString());
            */

            var stringWriter = new StringWriter();
            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                foreach (var item in selectList)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "checkBox");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);

                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "checkBox");
                    writer.AddAttribute(HtmlTextWriterAttribute.Name, name);
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, item.Value);
                    if (item.Selected) writer.AddAttribute(HtmlTextWriterAttribute.Checked, "checked");
                    if (null != htmlAttributes)
                        foreach (var attrib in htmlAttributes as IDictionary<String, Object>)
                            writer.AddAttribute(attrib.Key, attrib.Value.ToString());
                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    //writer.Write("&nbsp;");
                    writer.WriteEncodedText(item.Text);
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                }
            }
            return MvcHtmlString.Create(stringWriter.ToString());
        }

        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, String name, IEnumerable<SelectListItem> selectList)
        {
            return CheckBoxList(htmlHelper, name, selectList, null);
        }

        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, String name, IDictionary<String, String> selectList, IEnumerable<String> selectedValues, IDictionary<String, Object> htmlAttributes)
        {
            var selectLists = from item in selectList
                              select new SelectListItem
                              {
                                  Text = item.Key,
                                  Value = item.Value,
                                  Selected = (null != selectedValues && selectedValues.Contains(item.Value))
                              };

            return CheckBoxList(htmlHelper, name, selectLists, htmlAttributes);
        }

        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, String name, IDictionary<String, String> selectList, IEnumerable<String> selectedValues)
        {
            return CheckBoxList(htmlHelper, name, selectList, selectedValues, null);
        }

        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, String name, IDictionary<String, String> selectList, IDictionary<String, Object> htmlAttributes)
        {
            return CheckBoxList(htmlHelper, name, selectList, null, htmlAttributes);
        }

        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, String name, IDictionary<String, String> selectList)
        {
            return CheckBoxList(htmlHelper, name, selectList, null, null);
        }

        #region ---

        /// <summary>
        /// Generates Model-based list of checkboxes (For...)
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <typeparam name="TProperty">ViewModel property</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listNameExpr">ViewModel Item type to serve as a name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValuesExpr">Data list of selected items (should be of same data type as a source list)</param>
        /// <param name="htmlAttributesExpr">Data list HTML tag attributes for each checkbox</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty, TItem, TValue, TKey>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> listNameExpr, Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr, Expression<Func<TItem, TValue>> valueExpr, Expression<Func<TItem, TKey>> textToDisplayExpr, Expression<Func<TModel, IEnumerable<TItem>>> selectedValuesExpr, Expression<Func<TItem, TKey>> htmlAttributesExpr = null)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(listNameExpr, htmlHelper.ViewData);
            return ListBuilder.CheckBoxList(htmlHelper, modelMetadata, listNameExpr.ToProperty(), sourceDataExpr, valueExpr, textToDisplayExpr, htmlAttributesExpr, selectedValuesExpr, null, null, null);
        }
        /// <summary>
        /// Generates Model-based list of checkboxes
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listName">Name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValuesExpr">Data list of selected items (should be of same data type as a source list)</param>
        /// <param name="htmlAttributesExpr">Data list HTML tag attributes for each checkbox</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxList<TModel, TItem, TValue, TKey>(this HtmlHelper<TModel> htmlHelper, string listName, Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr, Expression<Func<TItem, TValue>> valueExpr, Expression<Func<TItem, TKey>> textToDisplayExpr, Expression<Func<TModel, IEnumerable<TItem>>> selectedValuesExpr, Expression<Func<TItem, TKey>> htmlAttributesExpr = null)
        {
            return ListBuilder.CheckBoxList(htmlHelper, null, listName, sourceDataExpr, valueExpr, textToDisplayExpr, htmlAttributesExpr, selectedValuesExpr, null, null, null);
        }

        /// <summary>
        /// Generates Model-based list of checkboxes (For...)
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <typeparam name="TProperty">ViewModel property</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listNameExpr">ViewModel Item type to serve as a name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValuesExpr">Data list of selected items (should be of same data type as a source list)</param>
        /// <param name="position">Direction of the list (e.g. 'Position2.Horizontal' or 'Position2.Vertical')</param>
        /// <param name="htmlAttributesExpr">Data list HTML tag attributes for each checkbox</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty, TItem, TValue, TKey>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> listNameExpr, Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr, Expression<Func<TItem, TValue>> valueExpr, Expression<Func<TItem, TKey>> textToDisplayExpr, Expression<Func<TModel, IEnumerable<TItem>>> selectedValuesExpr, Position position, Expression<Func<TItem, TKey>> htmlAttributesExpr = null)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(listNameExpr, htmlHelper.ViewData);
            return ListBuilder.CheckBoxList(htmlHelper, modelMetadata, listNameExpr.ToProperty(), sourceDataExpr, valueExpr, textToDisplayExpr, htmlAttributesExpr, selectedValuesExpr, null, null, null, position);
        }
        /// <summary>
        /// Generates Model-based list of checkboxes
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listName">Name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValuesExpr">Data list of selected items (should be of same data type as a source list)</param>
        /// <param name="position">Direction of the list (e.g. 'Position2.Horizontal' or 'Position2.Vertical')</param>
        /// <param name="htmlAttributesExpr">Data list HTML tag attributes for each checkbox</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxList<TModel, TItem, TValue, TKey>(this HtmlHelper<TModel> htmlHelper, string listName, Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr, Expression<Func<TItem, TValue>> valueExpr, Expression<Func<TItem, TKey>> textToDisplayExpr, Expression<Func<TModel, IEnumerable<TItem>>> selectedValuesExpr, Position position, Expression<Func<TItem, TKey>> htmlAttributesExpr = null)
        {
            return ListBuilder.CheckBoxList(htmlHelper, null, listName, sourceDataExpr, valueExpr, textToDisplayExpr, htmlAttributesExpr, selectedValuesExpr, null, null, null, position);
        }

        /// <summary>
        /// Generates Model-based list of checkboxes (For...)
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <typeparam name="TProperty">ViewModel property</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listNameExpr">ViewModel Item type to serve as a name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValuesExpr">Data list of selected items (should be of same data type as a source list)</param>
        /// <param name="htmlAttributes">Each checkbox HTML tag attributes (e.g. 'new { class="somename" }')</param>
        /// <param name="htmlAttributesExpr">Data list HTML tag attributes for each checkbox</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty, TItem, TValue, TKey>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> listNameExpr, Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr, Expression<Func<TItem, TValue>> valueExpr, Expression<Func<TItem, TKey>> textToDisplayExpr, Expression<Func<TModel, IEnumerable<TItem>>> selectedValuesExpr, object htmlAttributes, Expression<Func<TItem, TKey>> htmlAttributesExpr = null)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(listNameExpr, htmlHelper.ViewData);
            return ListBuilder.CheckBoxList
              (htmlHelper, modelMetadata, listNameExpr.ToProperty(), sourceDataExpr, valueExpr,
               textToDisplayExpr, htmlAttributesExpr, selectedValuesExpr, htmlAttributes, null, null);
        }
        /// <summary>
        /// Generates Model-based list of checkboxes
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listName">Name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValuesExpr">Data list of selected items (should be of same data type as a source list)</param>
        /// <param name="htmlAttributes">Each checkbox HTML tag attributes (e.g. 'new { class="somename" }')</param>
        /// <param name="htmlAttributesExpr">Data list HTML tag attributes for each checkbox</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxList<TModel, TItem, TValue, TKey>(this HtmlHelper<TModel> htmlHelper, string listName, Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr, Expression<Func<TItem, TValue>> valueExpr, Expression<Func<TItem, TKey>> textToDisplayExpr, Expression<Func<TModel, IEnumerable<TItem>>> selectedValuesExpr, object htmlAttributes, Expression<Func<TItem, TKey>> htmlAttributesExpr = null)
        {
            return ListBuilder.CheckBoxList(htmlHelper, null, listName, sourceDataExpr, valueExpr, textToDisplayExpr, htmlAttributesExpr, selectedValuesExpr, htmlAttributes, null, null);
        }

        /// <summary>
        /// Generates Model-based list of checkboxes (For...)
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <typeparam name="TProperty">ViewModel property</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listNameExpr">ViewModel Item type to serve as a name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValuesExpr">Data list of selected items (should be of same data type as a source list)</param>
        /// <param name="htmlAttributes">Each checkbox HTML tag attributes (e.g. 'new { class="somename" }')</param>
        /// <param name="position">Direction of the list (e.g. 'Position2.Horizontal' or 'Position2.Vertical')</param>
        /// <param name="htmlAttributesExpr">Data list HTML tag attributes, to allow override of htmlAttributes for each checkbox</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty, TItem, TValue, TKey>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> listNameExpr, Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr, Expression<Func<TItem, TValue>> valueExpr, Expression<Func<TItem, TKey>> textToDisplayExpr, Expression<Func<TModel, IEnumerable<TItem>>> selectedValuesExpr, object htmlAttributes, Position position, Expression<Func<TItem, TKey>> htmlAttributesExpr = null)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(listNameExpr, htmlHelper.ViewData);
            return ListBuilder.CheckBoxList(htmlHelper, modelMetadata, listNameExpr.ToProperty(), sourceDataExpr, valueExpr, textToDisplayExpr, htmlAttributesExpr, selectedValuesExpr, htmlAttributes, null, null, position);
        }
        /// <summary>
        /// Generates Model-based list of checkboxes
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listName">Name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValuesExpr">Data list of selected items (should be of same data type as a source list)</param>
        /// <param name="htmlAttributes">Each checkbox HTML tag attributes (e.g. 'new { class="somename" }')</param>
        /// <param name="position">Direction of the list (e.g. 'Position2.Horizontal' or 'Position2.Vertical')</param>
        /// <param name="htmlAttributesExpr">Data list HTML tag attributes, to allow override of htmlAttributes for each checkbox</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxList<TModel, TItem, TValue, TKey>(this HtmlHelper<TModel> htmlHelper, string listName, Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr, Expression<Func<TItem, TValue>> valueExpr, Expression<Func<TItem, TKey>> textToDisplayExpr, Expression<Func<TModel, IEnumerable<TItem>>> selectedValuesExpr, object htmlAttributes, Position position, Expression<Func<TItem, TKey>> htmlAttributesExpr = null)
        {
            return ListBuilder.CheckBoxList
              (htmlHelper, null, listName, sourceDataExpr, valueExpr, textToDisplayExpr, htmlAttributesExpr,
               selectedValuesExpr, htmlAttributes, null, null, position);
        }

        /// <summary>
        /// Generates Model-based list of checkboxes (For...)
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <typeparam name="TProperty">ViewModel property</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listNameExpr">ViewModel Item type to serve as a name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValuesExpr">Data list of selected items (should be of same data type as a source list)</param>
        /// <param name="htmlAttributes">Each checkbox HTML tag attributes (e.g. 'new { class="somename" }')</param>
        /// <param name="disabledValues">String array of values to disable</param>
        /// <param name="position">Direction of the list (e.g. 'Position2.Horizontal' or 'Position2.Vertical')</param>
        /// <param name="htmlAttributesExpr">Data list HTML tag attributes, to allow override of htmlAttributes for each checkbox</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty, TItem, TValue, TKey>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> listNameExpr, Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr, Expression<Func<TItem, TValue>> valueExpr, Expression<Func<TItem, TKey>> textToDisplayExpr, Expression<Func<TModel, IEnumerable<TItem>>> selectedValuesExpr, object htmlAttributes, string[] disabledValues, Position position, Expression<Func<TItem, TKey>> htmlAttributesExpr = null)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(listNameExpr, htmlHelper.ViewData);
            return ListBuilder.CheckBoxList
              (htmlHelper, modelMetadata, listNameExpr.ToProperty(), sourceDataExpr, valueExpr, textToDisplayExpr,
               htmlAttributesExpr, selectedValuesExpr, htmlAttributes, null, disabledValues, position);
        }
        /// <summary>
        /// Generates Model-based list of checkboxes
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listName">Name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValuesExpr">Data list of selected items (should be of same data type as a source list)</param>
        /// <param name="htmlAttributes">Each checkbox HTML tag attributes (e.g. 'new { class="somename" }')</param>
        /// <param name="disabledValues">String array of values to disable</param>
        /// <param name="position">Direction of the list (e.g. 'Position2.Horizontal' or 'Position2.Vertical')</param>
        /// <param name="htmlAttributesExpr">Data list HTML tag attributes, to allow override of htmlAttributes for each checkbox</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxList<TModel, TItem, TValue, TKey>(this HtmlHelper<TModel> htmlHelper, string listName, Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr, Expression<Func<TItem, TValue>> valueExpr, Expression<Func<TItem, TKey>> textToDisplayExpr, Expression<Func<TModel, IEnumerable<TItem>>> selectedValuesExpr, object htmlAttributes, string[] disabledValues, Position position, Expression<Func<TItem, TKey>> htmlAttributesExpr = null)
        {
            return ListBuilder.CheckBoxList
              (htmlHelper, null, listName, sourceDataExpr, valueExpr, textToDisplayExpr, htmlAttributesExpr,
               selectedValuesExpr, htmlAttributes, null, disabledValues, position);
        }

        /// <summary>
        /// Generates Model-based list of checkboxes (For...)
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <typeparam name="TProperty">ViewModel property</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listNameExpr">ViewModel Item type to serve as a name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValuesExpr">Data list of selected items (should be of same data type as a source list)</param>
        /// <param name="wrapInfo">Settings for HTML wrapper of the list (e.g. 'new HtmlListInfo2(HtmlTag2.vertical_columns, 2, new { style="color:green;" })')</param>
        /// <param name="htmlAttributesExpr">Data list HTML tag attributes for each checkbox</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty, TItem, TValue, TKey>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> listNameExpr, Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr, Expression<Func<TItem, TValue>> valueExpr, Expression<Func<TItem, TKey>> textToDisplayExpr, Expression<Func<TModel, IEnumerable<TItem>>> selectedValuesExpr, HtmlListInfo wrapInfo, Expression<Func<TItem, TKey>> htmlAttributesExpr = null)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(listNameExpr, htmlHelper.ViewData);
            return ListBuilder.CheckBoxList
              (htmlHelper, modelMetadata, listNameExpr.ToProperty(), sourceDataExpr, valueExpr, textToDisplayExpr,
               htmlAttributesExpr, selectedValuesExpr, null, wrapInfo, null);
        }
        /// <summary>
        /// Generates Model-based list of checkboxes
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listName">Name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValuesExpr">Data list of selected items (should be of same data type as a source list)</param>
        /// <param name="wrapInfo">Settings for HTML wrapper of the list (e.g. 'new HtmlListInfo2(HtmlTag2.vertical_columns, 2, new { style="color:green;" })')</param>
        /// <param name="htmlAttributesExpr">Data list HTML tag attributes for each checkbox</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxList<TModel, TItem, TValue, TKey>(this HtmlHelper<TModel> htmlHelper, string listName, Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr, Expression<Func<TItem, TValue>> valueExpr, Expression<Func<TItem, TKey>> textToDisplayExpr, Expression<Func<TModel, IEnumerable<TItem>>> selectedValuesExpr, HtmlListInfo wrapInfo, Expression<Func<TItem, TKey>> htmlAttributesExpr = null)
        {
            return ListBuilder.CheckBoxList
              (htmlHelper, null, listName, sourceDataExpr, valueExpr, textToDisplayExpr, htmlAttributesExpr,
               selectedValuesExpr, null, wrapInfo, null);
        }

        /// <summary>
        /// Generates Model-based list of checkboxes (For...)
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <typeparam name="TProperty">ViewModel property</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listNameExpr">ViewModel Item type to serve as a name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValuesExpr">Data list of selected items (should be of same data type as a source list)</param>
        /// <param name="wrapInfo">Settings for HTML wrapper of the list (e.g. 'new HtmlListInfo2(HtmlTag2.vertical_columns, 2, new { style="color:green;" })')</param>
        /// <param name="disabledValues">String array of values to disable</param>
        /// <param name="htmlAttributesExpr">Data list HTML tag attributes for each checkbox</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty, TItem, TValue, TKey>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> listNameExpr, Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr, Expression<Func<TItem, TValue>> valueExpr, Expression<Func<TItem, TKey>> textToDisplayExpr, Expression<Func<TModel, IEnumerable<TItem>>> selectedValuesExpr, HtmlListInfo wrapInfo, string[] disabledValues, Expression<Func<TItem, TKey>> htmlAttributesExpr = null)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(listNameExpr, htmlHelper.ViewData);
            return ListBuilder.CheckBoxList
              (htmlHelper, modelMetadata, listNameExpr.ToProperty(), sourceDataExpr, valueExpr, textToDisplayExpr,
               htmlAttributesExpr, selectedValuesExpr, null, wrapInfo, disabledValues);
        }
        /// <summary>
        /// Generates Model-based list of checkboxes
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listName">Name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValuesExpr">Data list of selected items (should be of same data type as a source list)</param>
        /// <param name="wrapInfo">Settings for HTML wrapper of the list (e.g. 'new HtmlListInfo2(HtmlTag2.vertical_columns, 2, new { style="color:green;" })')</param>
        /// <param name="disabledValues">String array of values to disable</param>
        /// <param name="htmlAttributesExpr">Data list HTML tag attributes for each checkbox</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxList<TModel, TItem, TValue, TKey>(this HtmlHelper<TModel> htmlHelper, string listName, Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr, Expression<Func<TItem, TValue>> valueExpr, Expression<Func<TItem, TKey>> textToDisplayExpr, Expression<Func<TModel, IEnumerable<TItem>>> selectedValuesExpr, HtmlListInfo wrapInfo, string[] disabledValues, Expression<Func<TItem, TKey>> htmlAttributesExpr = null)
        {
            return ListBuilder.CheckBoxList
              (htmlHelper, null, listName, sourceDataExpr, valueExpr, textToDisplayExpr, htmlAttributesExpr,
               selectedValuesExpr, null, wrapInfo, disabledValues);
        }

        /// <summary>
        /// Generates Model-based list of checkboxes (For...)
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <typeparam name="TProperty">ViewModel property</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listNameExpr">ViewModel Item type to serve as a name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValuesExpr">Data list of selected items (should be of same data type as a source list)</param>
        /// <param name="htmlAttributes">Each checkbox HTML tag attributes (e.g. 'new { class="somename" }')</param>
        /// <param name="wrapInfo">Settings for HTML wrapper of the list (e.g. 'new HtmlListInfo2(HtmlTag2.vertical_columns, 2, new { style="color:green;" })')</param>
        /// <param name="disabledValues">String array of values to disable</param>
        /// <param name="htmlAttributesExpr">Data list HTML tag attributes, to allow override of htmlAttributes for each checkbox</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty, TItem, TValue, TKey>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> listNameExpr, Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr, Expression<Func<TItem, TValue>> valueExpr, Expression<Func<TItem, TKey>> textToDisplayExpr, Expression<Func<TModel, IEnumerable<TItem>>> selectedValuesExpr, object htmlAttributes, HtmlListInfo wrapInfo, string[] disabledValues, Expression<Func<TItem, TKey>> htmlAttributesExpr = null)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(listNameExpr, htmlHelper.ViewData);
            return ListBuilder.CheckBoxList
              (htmlHelper, modelMetadata, listNameExpr.ToProperty(), sourceDataExpr, valueExpr, textToDisplayExpr,
               htmlAttributesExpr, selectedValuesExpr, htmlAttributes, wrapInfo, disabledValues);
        }
        /// <summary>
        /// Generates Model-based list of checkboxes
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listName">Name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValuesExpr">Data list of selected items (should be of same data type as a source list)</param>
        /// <param name="htmlAttributes">Each checkbox HTML tag attributes (e.g. 'new { class="somename" }')</param>
        /// <param name="wrapInfo">Settings for HTML wrapper of the list (e.g. 'new HtmlListInfo2(HtmlTag2.vertical_columns, 2, new { style="color:green;" })')</param>
        /// <param name="disabledValues">String array of values to disable</param>
        /// <param name="htmlAttributesExpr">Data list HTML tag attributes, to allow override of htmlAttributes for each checkbox</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxList<TModel, TItem, TValue, TKey>(this HtmlHelper<TModel> htmlHelper, string listName, Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr, Expression<Func<TItem, TValue>> valueExpr, Expression<Func<TItem, TKey>> textToDisplayExpr, Expression<Func<TModel, IEnumerable<TItem>>> selectedValuesExpr, object htmlAttributes, HtmlListInfo wrapInfo, string[] disabledValues, Expression<Func<TItem, TKey>> htmlAttributesExpr = null)
        {
            return ListBuilder.CheckBoxList
              (htmlHelper, null, listName, sourceDataExpr, valueExpr, textToDisplayExpr, htmlAttributesExpr,
               selectedValuesExpr, htmlAttributes, wrapInfo, disabledValues);
        }

        #endregion

        #endregion
        #endregion

        #region Pager

        /// <summary>
        /// Shows a pager control - Creates a list of links that jump to each page
        /// </summary>
        /// <param name="htmlHelper">The ViewPage instance this method executes on.</param>
        /// <param name="pagedList">A PagedList instance containing the data for the paged control</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action on the controller.</param>
        public static MvcHtmlString Pager<T>(this HtmlHelper htmlHelper, IPagedList<T> pagedList, String controllerName, String actionName)
        {
            var stringWriter = new StringWriter();
            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                for (var pageIndex = 1; pageIndex <= pagedList.NoOfPages; ++pageIndex)
                {
                    if (pageIndex != pagedList.PageIndex)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "/" + controllerName + "/" + actionName + "/" + pageIndex);
                        writer.AddAttribute(HtmlTextWriterAttribute.Alt, "Page " + pageIndex);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                    }

                    writer.AddAttribute(HtmlTextWriterAttribute.Class,
                                        pageIndex == pagedList.PageIndex
                                            ? "pageLinkCurrent"
                                            : "pageLink");

                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    writer.Write(pageIndex);
                    writer.RenderEndTag();

                    if (pageIndex != pagedList.PageIndex)
                    {
                        writer.RenderEndTag();
                    }
                    writer.Write("&nbsp;");
                }

                writer.Write(String.Concat("(", pagedList.NoOfItems, " items in all)"));
            }
            return MvcHtmlString.Create(stringWriter.ToString());
        }

        #region Html Pager

        public static String Pager(this HtmlHelper htmlHelper, int noOfPages, int pageIndex, String actionName, String controllerName, PagerOptions pagerOptions, String routeName, Object routeValues, Object htmlAttributes)
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

        public static String Pager(this HtmlHelper htmlHelper, int noOfPages, int pageIndex, String actionName, String controllerName, PagerOptions pagerOptions, String routeName, RouteValueDictionary routeValues, IDictionary<String, Object> htmlAttributes)
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

        static String Pager(HtmlHelper htmlHelper, PagerOptions pagerOptions, IDictionary<String, Object> htmlAttributes)
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

        public static String Pager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, PagerOptions pagerOptions, Object htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(htmlHelper, pagerOptions, new RouteValueDictionary(htmlAttributes))
                    : Pager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, pagerOptions, null, null, htmlAttributes);
        }

        public static String Pager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, PagerOptions pagerOptions, IDictionary<String, Object> htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(htmlHelper, pagerOptions, htmlAttributes)
                    : Pager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, pagerOptions, null, null, htmlAttributes);
        }

        public static String Pager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, String actionName, String controllerName, PagerOptions pagerOptions, Object htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(htmlHelper, 0, 1, actionName, controllerName, pagerOptions, null, null, htmlAttributes)
                    : Pager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, actionName, controllerName, pagerOptions, null, null, htmlAttributes);
        }

        public static String Pager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, String actionName, String controllerName, PagerOptions pagerOptions, IDictionary<String, Object> htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(htmlHelper, 0, 1, actionName, controllerName, pagerOptions, null, null, htmlAttributes)
                    : Pager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, actionName, controllerName, pagerOptions, null, null, htmlAttributes);
        }

        public static String Pager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, PagerOptions pagerOptions, String routeName, Object routeValues)
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

        public static String Pager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, PagerOptions pagerOptions, String routeName, Object routeValues, Object htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(htmlHelper, pagerOptions, new RouteValueDictionary(htmlAttributes))
                    : Pager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, pagerOptions, routeName, routeValues, htmlAttributes);
        }

        public static String Pager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, PagerOptions pagerOptions, String routeName, RouteValueDictionary routeValues, IDictionary<String, Object> htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(htmlHelper, pagerOptions, htmlAttributes)
                    : Pager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, pagerOptions, routeName, routeValues, htmlAttributes);
        }

        public static String Pager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, String routeName, Object routeValues, Object htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(htmlHelper, null, new RouteValueDictionary(htmlAttributes))
                    : Pager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, null, routeName, routeValues, htmlAttributes);
        }

        public static String Pager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, String routeName, RouteValueDictionary routeValues, IDictionary<String, Object> htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? Pager(htmlHelper, null, htmlAttributes)
                    : Pager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, null, routeName, routeValues, htmlAttributes);
        }

        #endregion

        #region jQuery Ajax Pager
        public static String AjaxPager(this HtmlHelper htmlHelper, int totalPageCount, int pageIndex, String actionName, String controllerName, String routeName, PagerOptions pagerOptions, Object routeValues, AjaxOptions ajaxOptions, Object htmlAttributes)
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

        public static String AjaxPager(this HtmlHelper htmlHelper, int totalPageCount, int pageIndex, String actionName, String controllerName, String routeName, PagerOptions pagerOptions, RouteValueDictionary routeValues, AjaxOptions ajaxOptions, IDictionary<String, Object> htmlAttributes)
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

        static String AjaxPager(HtmlHelper htmlHelper, PagerOptions pagerOptions, IDictionary<String, Object> htmlAttributes)
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

        public static String AjaxPager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, PagerOptions pagerOptions, AjaxOptions ajaxOptions, Object htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? AjaxPager(htmlHelper, pagerOptions, new RouteValueDictionary(htmlAttributes))
                    : AjaxPager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, null, pagerOptions, null, ajaxOptions, htmlAttributes);
        }

        public static String AjaxPager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, PagerOptions pagerOptions, AjaxOptions ajaxOptions, IDictionary<String, Object> htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? AjaxPager(htmlHelper, pagerOptions, htmlAttributes)
                    : AjaxPager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, null, pagerOptions, null, ajaxOptions, htmlAttributes);
        }

        public static String AjaxPager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, String routeName, Object routeValues, PagerOptions pagerOptions, AjaxOptions ajaxOptions)
        {
            return (default(PagedList<T>) == pagedList)
                    ? AjaxPager(htmlHelper, pagerOptions, null)
                    : AjaxPager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, routeName, pagerOptions, routeValues, ajaxOptions, null);
        }

        public static String AjaxPager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, String routeName, Object routeValues, PagerOptions pagerOptions, AjaxOptions ajaxOptions, Object htmlAttributes)
        {
            return (default(PagedList<T>) == pagedList)
                    ? AjaxPager(htmlHelper, pagerOptions, new RouteValueDictionary(htmlAttributes))
                    : AjaxPager(htmlHelper, pagedList.NoOfPages, pagedList.PageIndex, null, null, routeName, pagerOptions, routeValues, ajaxOptions, htmlAttributes);
        }

        public static String AjaxPager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList, String routeName, RouteValueDictionary routeValues, PagerOptions pagerOptions, AjaxOptions ajaxOptions, IDictionary<String, Object> htmlAttributes)
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

        #region Controls

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
            {
                try
                {
                    sb.Append(VirtualPathUtility.ToAbsolute(attribValue));
                }
                catch (ArgumentException e)
                {
                    if (e.Message.Contains("is not allowed here"))
                        throw new ArgumentException(e.Message + " (Try prefixing the app root, i.e. \"~/\".)", e.ParamName);
                    throw;
                }
            }
            else if (encode.Value)
            {
                sb.Append(htmlHelper.Encode(attribValue));
            }
            else
            {
                sb.Append(attribValue);
            }
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



        #endregion

        // --------------------------------------------------------------------

        //public static IHtmlString Raw(this HtmlHelper htmlHelper, String value)
        //{
        //    return new HtmlString(value);
        //}

        //public static IHtmlString Raw(this HtmlHelper htmlHelper, Object value)
        //{
        //    return new HtmlString(value == null ? null : value.ToString());
        //}

    }
}