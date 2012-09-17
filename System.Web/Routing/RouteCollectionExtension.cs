using System.Web.Mvc;

namespace System.Web.Routing
{
    public static class RouteCollectionExtension
    {
        ///<summary>
        /// Create routes for which I can retrieve the route name
        ///</summary>
        ///<param name="routes"></param>
        ///<param name="name"></param>
        ///<param name="url"></param>
        ///<param name="defaults"></param>
        ///<param name="constraints"></param>
        ///<param name="namespaces"></param>
        ///<returns></returns>
        /// <example>
        /// 	<code>
        /// 		var route = routes.Map("rName", "url");
        ///			route.GetRouteName();
        ///			
        ///			//within a controller
        ///			String routeName = RouteData.GetRouteName();
        /// 	</code>
        /// </example>
        public static Route Map(this RouteCollection routes, String name,
                String url, object defaults = null, object constraints = null, String[] namespaces = null)
        {
            return routes.MapRoute(name, url, defaults, constraints, namespaces).SetRouteName(name);
        }
    }
}
