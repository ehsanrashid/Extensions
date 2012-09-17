namespace System.Collections.Generic
{
    /// <summary>
    ///   The Association performs the same function as a KeyValuePair, but allows the Key and 
    ///   Value members to be written to.
    /// </summary>
    /// <typeparam name="TKey"> </typeparam>
    /// <typeparam name="TValue"> </typeparam>
    public sealed class Association<TKey, TValue>
    {
        /// <summary>
        ///   Gets the key.
        /// </summary>
        /// <value> The key. </value>
        public TKey Key { get; set; }

        /// <summary>
        ///   Gets the value.
        /// </summary>
        /// <value> The value. </value>
        public TValue Value { get; set; }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Association&lt;TKey, TValue&gt;" /> class.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <param name="value"> The value. </param>
        public Association(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        ///   Construct a KeyValuePair object from the current values.
        /// </summary>
        /// <returns> A key value pair representation of this Association. </returns>
        public KeyValuePair<TKey, TValue> ToKeyValuePair()
        {
            return new KeyValuePair<TKey, TValue>(Key, Value);
        }
    }
}