namespace System.Web.Mvc
{
    public class PagerItem
    {
        public PagerItem(String text, int pageIndex, bool disabled, PagerItemType type)
        {
            Text = text;
            PageIndex = pageIndex;
            Disabled = disabled;
            Type = type;
        }

        public String Text { get; set; }
        public int PageIndex { get; set; }
        public bool Disabled { get; set; }
        public PagerItemType Type { get; set; }
    }
}