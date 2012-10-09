namespace System.Web.Mvc
{
    using Collections.Generic.Interface;
    using Text;
    using Html;

    public class Pager<T>
    {
        readonly StringBuilder htmlText;
        readonly HtmlHelper htmlHelper;
        readonly String controller;
        readonly String action;
        readonly IPagedList<T> pagedList;

        public Pager(HtmlHelper htmlHelper, String controller, String action, IPagedList<T> pagedList)
        {
            htmlText = new StringBuilder();
            this.htmlHelper = htmlHelper;
            this.controller = controller;
            this.action = action;
            this.pagedList = pagedList;
        }

        public String WriteHtml()
        {
            htmlText.Append("<div class=\"pager\">");

            WriteLink(0, "<<");
            WriteLink(pagedList.PageIndex - 1, "<");

            for (int i = 0; i < pagedList.NoOfPages; i++)
            {
                WriteLink(i);
            }

            WriteLink(pagedList.PageIndex + 1, ">");
            WriteLink(pagedList.NoOfPages - 1, ">>");

            htmlText.Append("</div>");

            return htmlText.ToString();
        }

        private void WriteLink(int pageNumber)
        {
            WriteLink(pageNumber, (pageNumber + 1).ToString());
        }

        private void WriteLink(int pageNumber, String text)
        {
            if (pageNumber == pagedList.PageIndex || pageNumber < 0 || pageNumber > pagedList.NoOfPages - 1)
            {
                htmlText.AppendFormat("{0} ", text);
            }
            else
            {
                htmlText.AppendFormat("{0} ", htmlHelper.ActionLink(text, action, controller, new
                {
                    CurrentPage = pageNumber
                }));
            }
        }
    }
}
