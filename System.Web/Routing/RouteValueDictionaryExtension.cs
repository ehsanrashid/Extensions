namespace System.Web.Routing
{
    public static class RouteValueDictionaryExtension
    {

        ///<summary>
        /// Get the name of the route
        ///</summary>
        ///<param name="routeValues"></param>
        ///<returns></returns>
        /// <example>
        /// 	<code>
        ///			var route = routes.Map("rName", "url");
        ///			route.GetRouteName();
        ///		</code>
        /// </example>
        public static String GetRouteName(this RouteValueDictionary routeValues)
        {
            if (default(RouteValueDictionary) == routeValues) return default(String);
            var routeName = default(Object);
            routeValues.TryGetValue("__RouteName", out routeName);
            return routeName as String;
        }
    }
}
