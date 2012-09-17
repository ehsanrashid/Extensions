namespace System
{
    /// <summary>
    /// An internal implementation of the IConverter&lt;T&gt; interface
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    public class Converter<T> : IConverter<T>
    {
        /// <summary>
        /// 	Initializes a new instance of the <see cref = "Converter&lt;T&gt;" /> class.
        /// </summary>
        /// <param name = "value">The value.</param>
        public Converter(T value)
        {
            Value = value;
        }

        #region IConverter<T> Members

        /// <summary>
        /// 	Gets the internal value to be converted.
        /// </summary>
        /// <value>The value.</value>
        public T Value { get; protected set; }

        #endregion IConverter<T> Members
    }
}