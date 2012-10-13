namespace System.Web.Mvc
{
    /// <summary>
    /// Sets local settings of an HTML wrapper that is used on a checkbox list
    /// </summary>
    public class HtmlWrapperInfo
    {
        public String WrapOpen = String.Empty;
        public String WrapRowbreak = String.Empty;
        public String WrapClose = String.Empty;
        public HtmlElementTag WrapElement = HtmlElementTag.None;
        public String AppendToElement = String.Empty;
        public int SeparatorMaxCount;
    }

}
