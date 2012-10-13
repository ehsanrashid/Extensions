namespace System.Web.Mvc
{
    using Collections.Generic;
    using Collections.Generic.Interface;
    using Text;
    using Html;

    public class Pager<T>
    {
        readonly StringBuilder _htmlText;
        readonly HtmlHelper _htmlHelper;
        readonly String _controller;
        readonly String _action;
        readonly IPagedList<T> _pagedList;

        public Pager(HtmlHelper htmlHelper, String controller, String action, IPagedList<T> pagedList)
        {
            _htmlText = new StringBuilder();
            _htmlHelper = htmlHelper;
            _controller = controller;
            _action = action;
            _pagedList = pagedList;
        }

        public String WriteHtml()
        {
            _htmlText.Append("<div class=\"pager\">");

            WriteLink(0, "<<");
            WriteLink(_pagedList.PageIndex - 1, "<");

            for (var i = 0; i < _pagedList.NoOfPages; i++)
            {
                WriteLink(i);
            }

            WriteLink(_pagedList.PageIndex + 1, ">");
            WriteLink(_pagedList.NoOfPages - 1, ">>");

            _htmlText.Append("</div>");

            return _htmlText.ToString();
        }

        void WriteLink(int pageNumber)
        {
            WriteLink(pageNumber, (pageNumber + 1).ToString());
        }

        void WriteLink(int pageNumber, String text)
        {
            if (pageNumber == _pagedList.PageIndex || pageNumber < 0 || pageNumber > _pagedList.NoOfPages - 1)
            {
                _htmlText.AppendFormat("{0} ", text);
            }
            else
            {
                _htmlText.AppendFormat("{0} ", _htmlHelper.ActionLink(text, _action, _controller, new
                {
                    CurrentPage = pageNumber
                }));
            }
        }
    }

}
