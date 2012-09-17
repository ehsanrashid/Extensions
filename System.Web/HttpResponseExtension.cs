namespace System.Web
{

    /// <summary>
    ///   Extension methods for the HttpResponse class
    /// </summary>
    public static class HttpResponseExtension
    {
        /// <summary>
        ///   Reloads the current page / handler by performing a redirect to the current url
        /// </summary>
        /// <param name = "response">The HttpResponse to perform on.</param>
        public static void Reload(this HttpResponse response)
        {
            response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
        }

        /// <summary>
        ///   Performs a response redirect and allows the url to be populated with String format parameters.
        /// </summary>
        /// <param name = "response">The HttpResponse to perform on.</param>
        /// <param name = "urlFormat">The URL including String.Format placeholders.</param>
        /// <param name = "values">The values to the populated.</param>
        public static void Redirect(this HttpResponse response, String urlFormat, params object[] values)
        {
            response.Redirect(urlFormat, true, values);
        }

        /// <summary>
        ///   Performs a response redirect and allows the url to be populated with String format parameters.
        /// </summary>
        /// <param name = "response">The HttpResponse to perform on.</param>
        /// <param name = "urlFormat">The URL including String.Format placeholders.</param>
        /// <param name = "endResponse">If set to <c>true</c> the response will be terminated.</param>
        /// <param name = "values">The values to the populated.</param>
        public static void Redirect(this HttpResponse response, String urlFormat, bool endResponse, params object[] values)
        {
            var url = String.Format(urlFormat, values);
            response.Redirect(url, endResponse);
        }

        /// <summary>
        ///   Performs a response redirect and allows the url to be populated with a query String.
        /// </summary>
        /// <param name = "response">The HttpResponse to perform on.</param>
        /// <param name = "url">The URL.</param>
        /// <param name = "queryString">The query String.</param>
        public static void Redirect(this HttpResponse response, String url, UriQueryString queryString)
        {
            response.Redirect(url, queryString, true);
        }

        /// <summary>
        ///   Performs a response redirect and allows the url to be populated with a query String.
        /// </summary>
        /// <param name = "response">The HttpResponse to perform on.</param>
        /// <param name = "url">The URL.</param>
        /// <param name = "queryString">The Query String.</param>
        /// <param name = "endResponse">If set to <c>true</c> the response will be terminated.</param>
        public static void Redirect(this HttpResponse response, String url, UriQueryString queryString, bool endResponse)
        {
            url = queryString.ToString(url);
            response.Redirect(url, endResponse);
        }

        /// <summary>
        ///   Returns a 404 to the client and ends the response.
        /// </summary>
        /// <param name = "response">The HttpResponse to perform on.</param>
        public static void SetFileNotFound(this HttpResponse response)
        {
            response.SetFileNotFound(true);
        }

        /// <summary>
        ///   Returns a 404 to the client and optionally ends the response.
        /// </summary>
        /// <param name = "response">The HttpResponse to perform on.</param>
        /// <param name = "endResponse">If set to <c>true</c> the response will be terminated.</param>
        public static void SetFileNotFound(this HttpResponse response, bool endResponse)
        {
            response.SetStatus(404, "Not Found", endResponse);
        }

        /// <summary>
        ///   Returns a 500 to the client and ends the response.
        /// </summary>
        /// <param name = "response">The HttpResponse to perform on.</param>
        public static void SetInternalServerError(this HttpResponse response)
        {
            response.SetInternalServerError(true);
        }

        /// <summary>
        ///   Returns a 500 to the client and optionally ends the response.
        /// </summary>
        /// <param name = "response">The HttpResponse to perform on.</param>
        /// <param name = "endResponse">If set to <c>true</c> the response will be terminated.</param>
        public static void SetInternalServerError(this HttpResponse response, bool endResponse)
        {
            response.SetStatus(500, "Internal Server Error", endResponse);
        }

        /// <summary>
        ///   Set the specified HTTP status code and description and optionally ends the response.
        /// </summary>
        /// <param name = "response">The HttpResponse to perform on.</param>
        /// <param name = "code">The status code.</param>
        /// <param name = "description">The status description.</param>
        /// <param name = "endResponse">If set to <c>true</c> the response will be terminated.</param>
        public static void SetStatus(this HttpResponse response, int code, String description, bool endResponse)
        {
            response.StatusCode = code;
            response.StatusDescription = description;

            if (endResponse) response.End();
        }
    }
}