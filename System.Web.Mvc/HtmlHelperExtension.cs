using System.Text;
using System.Linq;
using System.Web.Routing;

namespace System.Web.Mvc
{
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
        public static MvcHtmlString ImageLink(this HtmlHelper htmlHelper, String imgSrc = null, String alt = null, String actionName = null, String controllerName = null, object routeValues = null, object htmlAttributes = null, object imgHtmlAttributes = null)
        {
            var urlHelper = ((Controller) htmlHelper.ViewContext.Controller).Url;
            var imgTag = new TagBuilder("img");
            imgTag.MergeAttribute("src", imgSrc);
            imgTag.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

            var url = urlHelper.Action(actionName, controllerName, routeValues);
            var imglink = new TagBuilder("a");
            imglink.MergeAttribute("href", url);
            imglink.InnerHtml = imgTag.ToString(TagRenderMode.SelfClosing);
            imglink.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

            return MvcHtmlString.Create(imglink.ToString());
        }

        ///<summary>
        /// Returns an HTML image element for the given source and alt text.
        ///</summary>
        ///<param name="htmlHelper"></param>
        ///<param name="src"></param>
        ///<param name="alt"></param>
        ///<param name="htmlAttributes"></param>
        ///<returns>MvcHtmlString</returns>
        public static MvcHtmlString Image(this HtmlHelper htmlHelper, String src, String alt = null, object htmlAttributes = null)
        {
            var tb = new TagBuilder("img");
            tb.Attributes.Add("src", htmlHelper.Encode(src));
            tb.Attributes.Add("alt", htmlHelper.Encode(alt));
            tb.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);
            return MvcHtmlString.Create(tb.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Returns an HTML label element for the given target and text.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="target"></param>
        /// <param name="text"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString Label(this HtmlHelper htmlHelper, String target, String text, object htmlAttributes = null)
        {
            var tb = new TagBuilder("label");
            tb.MergeAttribute("for", target);
            tb.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);
            tb.SetInnerText(text);

            return MvcHtmlString.Create(tb.ToString());
        }

        public static MvcHtmlString Tag(this HtmlHelper htmlHelper,
                String tag = null,
                String src = null, String href = null,
                String type = null,
                String id = null, String name = null,
                String style = null, String @class = null,
                String attribs = null)
        {
            var sb = new StringBuilder();
            sb.Append("<");
            if (String.IsNullOrEmpty(tag)) tag = "div";
            sb.Append(tag);
            AppendOptionalAttrib(htmlHelper, sb, "id", id, false, validateScriptableIdent: true);
            AppendOptionalAttrib(htmlHelper, sb, "name", name, false, validateScriptableIdent: true);
            AppendOptionalAttrib(htmlHelper, sb, "type", type);
            AppendOptionalAttrib(htmlHelper, sb, "src", src, false, true);
            AppendOptionalAttrib(htmlHelper, sb, "href", href, false, true);
            AppendOptionalAttrib(htmlHelper, sb, "style", style);
            AppendOptionalAttrib(htmlHelper, sb, "class", @class, false, validateClass: true);
            if (!String.IsNullOrEmpty(attribs)) sb.Append(" " + attribs);
            if ((new[] { "script", "div", "p", "a", "h1", "h2", "h3", "h4", "h5", "h6", "center", "table", "form" }).Contains(tag.ToLower()))
                sb.Append("></" + tag + ">");
            else sb.Append(" />");
            return MvcHtmlString.Create(sb.ToString());
        }

        private static void AppendOptionalAttrib(HtmlHelper htmlHelper, StringBuilder sb,
                String attribName, String attribValue, bool? encode = null,
                bool? resolveAbsUrl = null, bool? validateScriptableIdent = null, bool? validateClass = null)
        {
            if (String.IsNullOrEmpty(attribValue)) return;
            if (String.IsNullOrEmpty(attribName)) throw new ArgumentException("attribName is required.", "attribName");
            var attribNameLcase = attribName.ToLower();
            if (!resolveAbsUrl.HasValue) resolveAbsUrl = attribNameLcase == "src" || attribNameLcase == "href";
            if (!validateScriptableIdent.HasValue)
                validateScriptableIdent = attribNameLcase == "id" || attribNameLcase == "name";
            if (!validateClass.HasValue)
                validateClass = attribNameLcase == "class";
            if (!encode.HasValue) encode = !validateScriptableIdent.Value && !resolveAbsUrl.Value && !validateClass.Value;
            sb.Append(" " + attribName + "=\"");
            if (validateScriptableIdent.Value && !IsScriptableIdValue(attribValue))
                throw new FormatException("Attrib value has invalid characters: " + attribNameLcase + "=" + attribValue);
            if (validateClass.Value && !IsValidClassValue(attribValue))
                throw new FormatException("Attrib value has invalid characters: " + attribNameLcase + "=" + attribValue);
            if (resolveAbsUrl.Value)
            {
                try
                {
                    sb.Append(VirtualPathUtility.ToAbsolute(attribValue));
                }
                catch (ArgumentException e)
                {
                    if (e.Message.Contains("is not allowed here"))
                    {
                        throw new ArgumentException(e.Message + " (Try prefixing the app root, i.e. \"~/\".)", e.ParamName);
                    }
                    throw;
                }
            }
            else if (encode.Value) 
                sb.Append(htmlHelper.Encode(attribValue));
            else 
                sb.Append(attribValue);
            sb.Append("\"");
        }

        private static bool IsValidClassValue(String value)
        {
            const String numeric = "1234567890";
            const String alphanumeric = "abcdefghijklmnopqrstuvwxyz_- " + numeric;
            
            if (String.IsNullOrEmpty(value)) return false;

            value = value.ToLower();
            return value.All(c => alphanumeric.Contains(c));
        }

        private static bool IsScriptableIdValue(String value)
        {
            const String numeric = "1234567890";
            const String alphanumeric = "abcdefghijklmnopqrstuvwxyz_" + numeric;
            
            if (String.IsNullOrEmpty(value)) return false;
            
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

        //public static MvcForm BeginForm(this HtmlHelper htmlHelper, object routeValues, FormMethod method, object htmlAttributes)
        //{
        //  return htmlHelper.BeginForm(Html.CurrentAction, Html.CurrentController, routeValues, method, htmlAttributes);
        //}

        public static MvcHtmlString Render(this HtmlHelper htmlHelper, String content)
        {
            return MvcHtmlString.Create(content);
        }
    
    }
}