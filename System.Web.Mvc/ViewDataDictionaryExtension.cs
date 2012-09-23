using System.Collections.Generic;

namespace System.Web.Mvc
{
    public static class ViewDataDictionaryExtension
    {

        //<label style="background-color: <%:  ViewData["JobStatusMessageColor"] %>"> <%: ViewData[key]%></label>


        /// <summary>
        /// Appends a value to ViewData, meant to be displayed on the very _next_ request.
        /// </summary>
        /// <param name="viewData"></param>
        /// <param name="key">key of the value</param>
        /// <param name="value">The value to be added to the collection</param>
        public static void Append(this ViewDataDictionary viewData, String key, Object value)
        {
            DictionaryExtension.Append(viewData, key, value);
        }

    }
}
