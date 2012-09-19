namespace System.Web.Mvc
{
    using IO;

    public static class ViewExtension
    {
        ///<summary>
        /// Get the name of the view
        ///</summary>
        ///<param name="view">Current view</param>
        ///<returns>View name</returns>
        ///<exception cref="InvalidOperationException"></exception>
        public static String GetWebFormViewName(this IView view)
        {
            var webFormView = view as WebFormView;
            if (default(WebFormView) == webFormView) throw (new InvalidOperationException("This view is not a WebFormView"));

            var viewUrl = webFormView.ViewPath;
            var viewFileName = viewUrl.Substring(viewUrl.LastIndexOf('/'));
            var viewFileNameWithoutExtension = Path.GetFileNameWithoutExtension(viewFileName);
            return (viewFileNameWithoutExtension);
        }
    }
}