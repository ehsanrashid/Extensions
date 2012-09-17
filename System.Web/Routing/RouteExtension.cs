namespace System.Web.Routing
{

    public static class RouteExtension
    {
        ///<summary>
        /// Get the name of the route
        ///</summary>
        ///<param name="route"></param>
        ///<returns></returns>
        public static String GetRouteName(this Route route)
        {
            return default(Route) == route ? default(String) : route.DataTokens.GetRouteName();
        }

        ///<summary>
        /// Set the name of a route
        ///</summary>
        ///<param name="route">The route</param>
        ///<param name="routeName">the route name</param>
        ///<returns></returns>
        ///<exception cref="ArgumentNullException"></exception>
        /// <example>
        /// 	<code>
        ///		routes.MapRoute("rName", "{controller}/{action}").SetRouteName("rName");
        ///		</code>
        /// </example>
        public static Route SetRouteName(this Route route, String routeName)
        {
            if (default(Route) == route) throw new ArgumentNullException("route");
            if (default(RouteValueDictionary) == route.DataTokens) route.DataTokens = new RouteValueDictionary();

            route.DataTokens["__RouteName"] = routeName;
            return route;
        }

    }
}