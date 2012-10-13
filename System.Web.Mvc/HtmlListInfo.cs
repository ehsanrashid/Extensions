namespace System.Web.Mvc
{
    /// <summary>
    /// Sets settings of an HTML wrapper that is used on a checkbox list
    /// </summary>
    public class HtmlListInfo
    {
        public HtmlListInfo(HtmlTag htmlTag, int columns = 0, object htmlAttributes = null, TextLayout textLayout = TextLayout.Default)
        {
            HtmlTag = htmlTag;
            Columns = columns;
            HtmlAttributes = htmlAttributes;
            TextLayout = textLayout;
        }

        public HtmlTag HtmlTag { get; set; }
        public int Columns { get; set; }
        public object HtmlAttributes { get; set; }
        public TextLayout TextLayout { get; set; }
    }
}
