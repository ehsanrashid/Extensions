namespace System.Web.Mvc
{
    using UI;
    using IO;
    using Reflection;
    using Caching;
    using Text;

    public class ActionOutputCacheAttribute : ActionFilterAttribute
    {
        // This hack is optional; I'll explain it later in the blog post
        static MethodInfo _switchWriterMethod = typeof(HttpResponse).GetMethod("SwitchWriter", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

        public ActionOutputCacheAttribute(int cacheDuration)
        {
            _cacheDuration = cacheDuration;
        }

        int _cacheDuration;
        TextWriter _originalWriter;
        String _cacheKey;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _cacheKey = ComputeCacheKey(filterContext);
            var cachedOutput = (String) filterContext.HttpContext.Cache[_cacheKey];
            if (cachedOutput != null)
                filterContext.Result = new ContentResult { Content = cachedOutput };
            else
                _originalWriter = (TextWriter) _switchWriterMethod.Invoke(HttpContext.Current.Response, new object[] { new HtmlTextWriter(new StringWriter()) });
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (_originalWriter != null) // Must complete the caching
            {
                var cacheWriter = (HtmlTextWriter) _switchWriterMethod.Invoke(HttpContext.Current.Response, new object[] { _originalWriter });
                var textWritten = ((StringWriter) cacheWriter.InnerWriter).ToString();
                filterContext.HttpContext.Response.Write(textWritten);

                filterContext.HttpContext.Cache.Add(_cacheKey, textWritten, null, DateTime.Now.AddSeconds(_cacheDuration), Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
            }
        }

        String ComputeCacheKey(ActionExecutingContext filterContext)
        {
            var sbKey = new StringBuilder();
            foreach (var pair in filterContext.RouteData.Values)
                sbKey.AppendFormat("rd{0}_{1}_", pair.Key.GetHashCode(), pair.Value.GetHashCode());
            foreach (var pair in filterContext.ActionParameters)
                sbKey.AppendFormat("ap{0}_{1}_", pair.Key.GetHashCode(), pair.Value.GetHashCode());
            return sbKey.ToString();
        }
    }

}
