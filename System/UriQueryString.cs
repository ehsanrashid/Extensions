namespace System
{
    using Collections.Generic;
    using Text;
    using Web;

    /// <summary>
    /// </summary>
    public class UriQueryString
    {
        /// <summary>
        /// </summary>
        readonly Dictionary<String, String> _values = new Dictionary<String, String>();

        /// <summary>
        ///   Adds the specified key.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <param name="value"> The value. </param>
        public void Add(String key, String value)
        {
            _values.Add(key, HttpUtility.UrlEncode(value));
        }

        /// <summary>
        ///   Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </summary>
        /// <param name="baseUrl"> The base URL. </param>
        /// <returns> A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" /> . </returns>
        public virtual String ToString(String baseUrl)
        {
            var sb = new StringBuilder();
            foreach (var pair in _values)
            {
                if (sb.Length > 0) sb.Append("&");
                sb.AppendFormat("{0}={1}", pair.Key, pair.Value);
            }
            return baseUrl.IsNotNullOrEmpty() ? String.Concat(baseUrl, "?", sb.ToString()) : sb.ToString();
        }

        /// <summary>
        ///   Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="baseUrl"> The base URL. </param>
        /// <returns> A <see cref="System.String" /> that represents this instance. </returns>
        public virtual String ToString(Uri baseUrl)
        {
            return ToString(baseUrl.ToString());
        }

        /// <summary>
        ///   Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns> A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" /> . </returns>
        public override String ToString()
        {
            return ToString(default(String));
        }
    }
}