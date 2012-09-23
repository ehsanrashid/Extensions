using System.Collections.Generic;

namespace System.Web.Mvc
{
    public static class TempDataDictionaryExtension
    {
        /// <summary>
        /// Appends a value to TempData, meant to be displayed on the very _next_ request.
        /// </summary>
        /// <param name="tempData"></param>
        /// <param name="key">key of the value</param>
        /// <param name="value">The value to be added to the collection</param>
        public static void Append(this TempDataDictionary tempData, String key, Object value)
        {
            DictionaryExtension.Append(tempData, key, value);
        }

    }
}
