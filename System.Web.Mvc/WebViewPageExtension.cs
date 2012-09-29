namespace System.Web.Mvc
{
   
    public static class WebViewPageExtension
    {
        static String Get(WebViewPage view, String key)
        {
            return view.ViewContext.Controller.ValueProvider.GetValue(key).RawValue.ToString();
        }

        public static String GetFormActionUrl(this WebViewPage view)
        {
            return String.Format("/{0}/{1}/{2}", view.GetController(), view.GetAction(), view.GetId());
        }

        public static String GetController(this WebViewPage view)
        {
            return Get(view, "controller");
        }

        public static String GetAction(this WebViewPage view)
        {
            return Get(view, "action");
        }

        public static String GetId(this WebViewPage view)
        {
            return Get(view, "id");
        }

        
    }
    
}