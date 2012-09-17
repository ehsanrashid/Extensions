namespace System.Web.Routing
{
    public static class RouteDataExtension
    {

        ///<summary>
        /// Get the name of the route
        ///</summary>
        ///<param name="routeData"></param>
        ///<returns></returns>
        public static String GetRouteName(this RouteData routeData)
        {
            return default(RouteData) == routeData ? default(String) : routeData.DataTokens.GetRouteName();
        }

    }
}
