namespace System.Collections.Generic
{
    /// <summary>
    /// A generic Singleton pattern implementation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Singleton<T> where T : new()
    {
        #region Globals

        static T instance = new T();

        #endregion

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance
        {
            get { return instance; }
            internal set { instance = value; }
        }
    }
}