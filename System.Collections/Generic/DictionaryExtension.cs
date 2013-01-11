namespace System.Collections.Generic
{
    using Linq;

    /// <summary>
    /// 	Extension methods for Dictionary.
    /// </summary>
    public static class DictionaryExtension
    {
        /// <summary>
        /// Sorts the specified dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> Sort<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (null == dictionary) throw new ArgumentNullException("dictionary");
            return new SortedDictionary<TKey, TValue>(dictionary);
        }

        /// <summary>
        /// Sorts the specified dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary to be sorted.</param>
        /// <param name="comparer">The comparer used to sort dictionary.</param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> Sort<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IComparer<TKey> comparer)
        {
            if (null == dictionary) throw new ArgumentNullException("dictionary");
            if (comparer == null) throw new ArgumentNullException("comparer");
            return new SortedDictionary<TKey, TValue>(dictionary, comparer);
        }

        /// <summary>
        /// Sorts the dictionary by value.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> SortByValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return (new SortedDictionary<TKey, TValue>(dictionary)).OrderBy(kvp => kvp.Value).ToDictionary(item => item.Key, item => item.Value);
        }

        /// <summary>
        /// Inverts the specified dictionary. (Creates a new dictionary with the values as key, and key as values)
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns></returns>
        public static IDictionary<TValue, TKey> Invert<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (null == dictionary) throw new ArgumentNullException("dictionary");
            return dictionary.ToDictionary(pair => pair.Value, pair => pair.Key);
        }

        /// <summary>
        /// Creates a (non-generic) Hashtable from the Dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns></returns>
        public static Hashtable ToHashTable<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            var table = new Hashtable();
            foreach (var item in dictionary)
                table.Add(item.Key, item.Value);
            return table;
        }

        /// <summary>
        /// Returns the value of the first entry found with one of the <paramref name="keys"/> received.
        /// <para>Returns <paramref name="defaultValue"/> if none of the keys exists in this collection </para>
        /// </summary>
        /// <param name="dictionary"> </param>
        /// <param name="defaultValue">Default value if none of the keys </param>
        /// <param name="keys"> keys to search for (in order) </param>
        public static TValue GetFirstValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue defaultValue, params TKey[] keys)
        {
            foreach (var key in keys.Where(dictionary.ContainsKey))
            {
                return dictionary[key];
            }
            return defaultValue;
        }

        /// <summary>
        /// Returns the value associated with the specified key, or a default value if no element is found.
        /// </summary>
        /// <typeparam name="TKey">The key data type</typeparam>
        /// <typeparam name="TValue">The value data type</typeparam>
        /// <param name="source">The source dictionary.</param>
        /// <param name="key">The key of interest.</param>
        /// <returns>The value associated with the specified key if the key is found, the default value for the value data type if the key is not found</returns>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
        {
            return source.GetOrDefault(key, default(TValue));
        }

        /// <summary>
        /// Returns the value associated with the specified key, or the specified default value if no element is found.
        /// </summary>
        /// <typeparam name="TKey">The key data type</typeparam>
        /// <typeparam name="TValue">The value data type</typeparam>
        /// <param name="source">The source dictionary.</param>
        /// <param name="key">The key of interest.</param>
        /// <param name="defaultValue">The default value to return if the key is not found.</param>
        /// <returns>The value associated with the specified key if the key is found, the specified default value if the key is not found</returns>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue defaultValue)
        {
            TValue value;
            return source.TryGetValue(key, out value) ? value : defaultValue;
        }

        /// <summary>
        /// Returns the value associated with the specified key, or throw the specified exception if no element is found.
        /// </summary>
        /// <typeparam name="TKey">The key data type</typeparam>
        /// <typeparam name="TValue">The value data type</typeparam>
        /// <param name="source">The source dictionary.</param>
        /// <param name="key">The key of interest.</param>
        /// <param name="exception">The exception to throw if the key is not found.</param>
        /// <returns>The value associated with the specified key if the key is found, the specified exception is thrown if the key is not found</returns>
        public static TValue GetOrThrow<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Exception exception)
        {
            TValue value;
            if (source.TryGetValue(key, out value))
            {
                return value;
            }

            throw exception;
        }

        /// <summary>
        /// Tests if the collection is empty.
        /// </summary>
        /// <param name="collection">The collection to test.</param>
        /// <returns>True if the collection is empty.</returns>
        public static bool IsEmpty(this IDictionary collection)
        {
            collection.ExceptionIfNull("The collection cannot be null.", "collection");

            return collection.Count == 0;
        }

        /// <summary>
        /// Tests if the IDictionary is empty.
        /// </summary>
        /// <typeparam name="TKey">The type of the key of 
        /// the IDictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values
        /// of the IDictionary.</typeparam>
        /// <param name="collection">The collection to test.</param>
        /// <returns>True if the collection is empty.</returns>
        public static bool IsEmpty<TKey, TValue>(this IDictionary<TKey, TValue> collection)
        {
            collection.ExceptionIfNull("The collection cannot be null.", "collection");
            return collection.Count == 0;
        }

        /// <summary>
        /// Appends an Item
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"> </param>
        /// <param name="value"></param>
        public static void Append(this IDictionary<String, Object> dictionary, String key, Object value)
        {                            //IDictionary<String, List<Object>>
            List<Object> listValues;
            if (dictionary.ContainsKey(key))
            {
                listValues = (List<Object>) dictionary[key];
            }
            else
            {
                listValues = new List<Object>(5);
                dictionary[key] = listValues;
            }
            listValues.Add(value);
        }

        /// <summary>
        /// Gets the List of values.
        /// When this is called, the List of values are cleared.
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"> </param>
        public static List<Object> GetValues(this IDictionary<String, Object> dictionary, String key)
        {
            var listValues = (List<Object>) dictionary[key];
            if (null != listValues) return listValues;
            dictionary.Remove(key);
            return null;
        }

    }
}