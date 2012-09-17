namespace System.Collections.Generic
{
    using Runtime.Serialization;
    using Visitors;

    /// <summary>
    ///   A custom Linked List datastructure, extending the default .NET LinkedList.
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    [Serializable]
    public sealed class VisitableLinkedList<T> : LinkedList<T>, IVisitableCollection<T>
    {
        #region Construction
        /// <summary>
        ///   Initializes a new instance of the <see cref="LinkedList&lt;T&gt;" /> class.
        /// </summary>
        public VisitableLinkedList()
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="LinkedList&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="collection"> The collection. </param>
        public VisitableLinkedList(IEnumerable<T> collection) : base(collection)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="LinkedList&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="info"> A <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> object containing the information required to serialize the <see
        ///    cref="T:System.Collections.Generic.LinkedList`1"></see> . </param>
        /// <param name="context"> A <see cref="T:System.Runtime.Serialization.StreamingContext"></see> object containing the source and destination of the serialized stream associated with the <see
        ///    cref="T:System.Collections.Generic.LinkedList`1"></see> . </param>
        private VisitableLinkedList(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        #endregion

        #region IVisitableCollection<T> Members
        /// <summary>
        ///   Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor"> The visitor. </param>
        public void Accept(IVisitor<T> visitor)
        {
            if (visitor == null)
                throw new ArgumentNullException("visitor");
            var enumerator = GetEnumerator();
            while (enumerator.MoveNext())
            {
                visitor.Visit(enumerator.Current);
                if (visitor.HasCompleted)
                    return;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this collection is empty.
        /// </summary>
        /// <value> <c>true</c> if this collection is empty; otherwise, <c>false</c> . </value>
        public bool IsEmpty
        {
            get { return Count == 0; }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance is of a fixed size.
        /// </summary>
        /// <value> <c>true</c> if this instance is fixed size; otherwise, <c>false</c> . </value>
        public bool IsFixedSize
        {
            get { return false; }
        }

        /// <summary>
        ///   Gets a value indicating whether this collection is full.
        /// </summary>
        /// <value> <c>true</c> if this collection is full; otherwise, <c>false</c> . </value>
        public bool IsFull
        {
            get { return false; }
        }
        #endregion

        #region IComparable<T> Members
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
                var l = obj as VisitableLinkedList<T>;
                return Count.CompareTo(l.Count);
            }
            else
                return GetType().FullName.CompareTo(obj.GetType().FullName);
        }
        #endregion
    }
}