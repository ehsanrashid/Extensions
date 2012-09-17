namespace System.Collections.Generic
{
    using Runtime.Serialization;
    using Visitors;

    /// <summary>
    ///   A custom hashtable extending the standard Generic Dictionary.
    /// </summary>
    /// <typeparam name="TKey"> </typeparam>
    /// <typeparam name="TValue"> </typeparam>
    [Serializable]
    public class VisitableHashtable<TKey, TValue> : Dictionary<TKey, TValue>, IVisitableDictionary<TKey, TValue>
    {
        #region Construction
        /// <summary>
        ///   Initializes a new instance of the <see cref="VisitableHashtable&lt;TKey, TValue&gt;" /> class.
        /// </summary>
        public VisitableHashtable()
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="VisitableHashtable&lt;TKey, TValue&gt;" /> class.
        /// </summary>
        /// <param name="dictionary"> The dictionary. </param>
        public VisitableHashtable(IDictionary<TKey, TValue> dictionary) : base(dictionary)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="VisitableHashtable&lt;TKey, TValue&gt;" /> class.
        /// </summary>
        /// <param name="comparer"> The comparer. </param>
        public VisitableHashtable(IEqualityComparer<TKey> comparer) : base(comparer)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="VisitableHashtable&lt;TKey, TValue&gt;" /> class.
        /// </summary>
        /// <param name="capacity"> The capacity. </param>
        public VisitableHashtable(int capacity) : base(capacity)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="VisitableHashtable&lt;TKey, TValue&gt;" /> class.
        /// </summary>
        /// <param name="dictionary"> The dictionary. </param>
        /// <param name="comparer"> The comparer. </param>
        public VisitableHashtable(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
            : base(dictionary, comparer)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="VisitableHashtable&lt;TKey, TValue&gt;" /> class.
        /// </summary>
        /// <param name="capacity"> The capacity. </param>
        /// <param name="comparer"> The comparer. </param>
        public VisitableHashtable(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="VisitableHashtable&lt;TKey, TValue&gt;" /> class.
        /// </summary>
        /// <param name="info"> A <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> object containing the information required to serialize the <see
        ///    cref="T:System.Collections.Generic.Dictionary`2"></see> . </param>
        /// <param name="context"> A <see cref="T:System.Runtime.Serialization.StreamingContext"></see> structure containing the source and destination of the serialized stream associated with the <see
        ///    cref="T:System.Collections.Generic.Dictionary`2"></see> . </param>
        protected VisitableHashtable(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        #endregion

        #region IVisitableCollection<KeyValuePair<TKey,TValue>> Members
        /// <summary>
        ///   Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor"> The visitor. </param>
        public void Accept(IVisitor<KeyValuePair<TKey, TValue>> visitor)
        {
            if (visitor == null)
                throw new ArgumentNullException("visitor");
            var enumerator = GetEnumerator();
            while (enumerator.MoveNext())
            {
                visitor.Visit(enumerator.Current);
                if (visitor.HasCompleted)
                    break;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this collection is empty.
        /// </summary>
        /// <value> <c>true</c> if this collection is empty; otherwise, <c>false</c> . </value>
        public bool IsEmpty
        {
            get { return (Count == 0); }
        }

        /// <summary>
        ///   Gets a value indicating whether this collection is full.
        /// </summary>
        /// <value> <c>true</c> if this collection is full; otherwise, <c>false</c> . </value>
        public bool IsFull
        {
            get { return false; }
        }

        /// <summary>
        ///   Gets a value indicating whether the <see cref="T:System.Collections.IDictionary"></see> collection has a fixed size.
        /// </summary>
        /// <value> </value>
        /// <returns> true if the <see cref="T:System.Collections.IDictionary"></see> collection has a fixed size; otherwise, false. </returns>
        public bool IsFixedSize
        {
            get { return false; }
        }
        #endregion

        #region IComparable Members
        /// <summary>
        ///   Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj"> An object to compare with this instance. </param>
        /// <returns> A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than obj. Zero This instance is equal to obj. Greater than zero This instance is greater than obj. </returns>
        /// <exception cref="T:System.ArgumentException">obj is not the same type as this instance.</exception>
        public int CompareTo(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (obj.GetType() == GetType())
            {
                var h = obj as VisitableHashtable<TKey, TValue>;
                return Count.CompareTo(h.Count);
            }
            else
                return GetType().FullName.CompareTo(obj.GetType().FullName);
        }
        #endregion
    }
}