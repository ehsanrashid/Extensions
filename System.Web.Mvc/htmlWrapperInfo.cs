namespace System.Web.Mvc
{
    /// <summary>
    /// Sets local settings of an HTML wrapper that is used on a checkbox list
    /// </summary>
    internal class HtmlWrapperInfo
    {
        public string wrap_open = String.Empty;
        public string wrap_rowbreak = String.Empty;
        public string wrap_close = String.Empty;
        public HtmlElementTag wrap_element = HtmlElementTag.None;
        public string append_to_element = String.Empty;
        public int separator_max_counter;
    }
}
