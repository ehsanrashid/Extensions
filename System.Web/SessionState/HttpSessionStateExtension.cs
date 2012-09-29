namespace System.Web.SessionState
{

    /// <summary>
    ///   Extensions classes for the ASP.NET Session State class
    /// </summary>
    public static class HttpSessionStateExtension
    {
        /// <summary>
        ///   Returns a typed value from the ASP.NET session state or the provided default value
        /// </summary>
        /// <typeparam name = "T">The generic type to be returned</typeparam>
        /// <param name = "sessionState">The session state.</param>
        /// <param name = "key">The session state key.</param>
        /// <param name = "defaultValue">The default value to be returned.</param>
        /// <returns>The session state value.</returns>
        /// <example>
        ///   <code>
        ///     public List&lt;string&gt; StringValues {
        ///     get { return this.Session.Ensure&lt;List&lt;string&gt;&gt;("StringValues"); }
        ///     set { this.ViewState.Set("StringValues", value); }
        ///     }
        ///   </code>
        /// </example>
        public static T GetValue<T>(this HttpSessionState sessionState, string key, T defaultValue = default(T))
        {
            var value = sessionState[key];
            return (T) (value ?? defaultValue);
        }

        /// <summary>
        ///   Ensures a specific key to be either already in the ASP.NET session state or to be newly created
        /// </summary>
        /// <typeparam name = "T">The generic type to be returned</typeparam>
        /// <param name = "sessionState">The session state.</param>
        /// <param name = "key">The session state key.</param>
        /// <returns>The session state value.</returns>
        /// <example>
        ///   <code>
        ///     public List&lt;string&gt; StringValues {
        ///     get { return this.Session.Ensure&lt;List&lt;string&gt;&gt;("StringValues"); }
        ///     set { this.ViewState.Set("StringValues", value); }
        ///     }
        ///   </code>
        /// </example>
        public static T Ensure<T>(this HttpSessionState sessionState, string key) where T : class, new()
        {
            var value = sessionState.GetValue<T>(key);
            if (value == null)
            {
                value = new T();
                sessionState[key] = value;
            }
            return value;
        }

     
    }
}