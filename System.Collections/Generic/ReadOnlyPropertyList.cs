namespace System.Collections.Generic
{
    using Visitors;
    using ComponentModel;
    using Properties;

    /// <summary>
    ///   A Wrapper around a IList which exposes a specific property via a read only list.
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    /// <typeparam name="TProperty"> </typeparam>
    public sealed class ReadOnlyPropertyList<T, TProperty> : IList<TProperty>, IVisitableCollection<TProperty>
    {
        readonly IList<T> _list;
        readonly PropertyDescriptor _property;

        /// <summary>
        ///   Initializes a new instance of the <see cref="ReadOnlyPropertyList&lt;T, TProperty&gt;" /> class.
        /// </summary>
        /// <param name="list"> The list to wrap. </param>
        /// <param name="property"> The property on the type to use for conversion. </param>
        public ReadOnlyPropertyList(IList<T> list, PropertyDescriptor property)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (property == null)
                throw new ArgumentNullException("property");
            if (!property.PropertyType.IsAssignableFrom(typeof (TProperty)))
                throw new ArgumentException(Resources.PropertyHaveOtherType + typeof (TProperty).FullName,
                                            "property");
            _list = list;
            _property = property;
        }

        /// <summary>
        ///   Gets the item at the specified index.
        /// </summary>
        /// <value> </value>
        public TProperty this[int index]
        {
            get { return (TProperty) _property.GetValue(_list[index]); }
        }

        #region IList<TProperty> Members
        /// <summary>
        ///   Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"></see>.
        /// </summary>
        /// <param name="item"> The object to locate in the <see cref="T:System.Collections.Generic.IList`1"></see> . </param>
        /// <returns> The index of item if found in the list; otherwise, -1. </returns>
        public int IndexOf(TProperty item)
        {
            for (var i = 0; i < _list.Count; i++)
                if (_property.GetValue(_list[i]).Equals(item))
                    return i;
            return -1;
        }

        /// <summary>
        ///   Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"></see> at the specified index.
        /// </summary>
        /// <param name="index"> The zero-based index at which item should be inserted. </param>
        /// <param name="item"> The object to insert into the <see cref="T:System.Collections.Generic.IList`1"></see> . </param>
        /// <exception cref="T:System.NotSupportedException">The
        ///   <see cref="T:System.Collections.Generic.IList`1"></see>
        ///   is read-only.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the
        ///   <see cref="T:System.Collections.Generic.IList`1"></see>
        ///   .</exception>
        void IList<TProperty>.Insert(int index, TProperty item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///   Removes the <see cref="T:System.Collections.Generic.IList`1"></see> item at the specified index.
        /// </summary>
        /// <param name="index"> The zero-based index of the item to remove. </param>
        /// <exception cref="T:System.NotSupportedException">The
        ///   <see cref="T:System.Collections.Generic.IList`1"></see>
        ///   is read-only.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the
        ///   <see cref="T:System.Collections.Generic.IList`1"></see>
        ///   .</exception>
        void IList<TProperty>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///   Gets or sets the item at the specified index.
        /// </summary>
        /// <value> </value>
        TProperty IList<TProperty>.this[int index]
        {
            get { return this[index]; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        ///   Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item"> The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see> . </param>
        /// <exception cref="T:System.NotSupportedException">The
        ///   <see cref="T:System.Collections.Generic.ICollection`1"></see>
        ///   is read-only.</exception>
        void ICollection<TProperty>.Add(TProperty item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///   Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The
        ///   <see cref="T:System.Collections.Generic.ICollection`1"></see>
        ///   is read-only.</exception>
        void ICollection<TProperty>.Clear()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///   Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
        /// </summary>
        /// <param name="item"> The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see> . </param>
        /// <returns> true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see> ; otherwise, false. </returns>
        public bool Contains(TProperty item)
        {
            return IndexOf(item) != -1;
        }

        /// <summary>
        ///   Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see
        ///    cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
        /// </summary>
        /// <param name="array"> The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see
        ///    cref="T:System.Collections.Generic.ICollection`1"></see> . The <see cref="T:System.Array"></see> must have zero-based indexing. </param>
        /// <param name="arrayIndex"> The zero-based index in array at which copying begins. </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
        /// <exception cref="T:System.ArgumentNullException">array is null.</exception>
        /// <exception cref="T:System.ArgumentException">array is multidimensional.-or-arrayIndex is equal to or greater than the length of array.-or-The number of elements in the source
        ///   <see cref="T:System.Collections.Generic.ICollection`1"></see>
        ///   is greater than the available space from arrayIndex to the end of the destination array.-or-Type T cannot be cast automatically to the type of the destination array.</exception>
        public void CopyTo(TProperty[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if ((array.Length - arrayIndex) < Count)
                throw new ArgumentException(Resources.NotEnoughSpaceInTargetArray);
            foreach (var item in _list)
                array[arrayIndex++] = (TProperty) _property.GetValue(item);
        }

        /// <summary>
        ///   Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <value> </value>
        /// <returns> The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see> . </returns>
        public int Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        ///   Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
        /// </summary>
        /// <value> </value>
        /// <returns> true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false. </returns>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        ///   Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item"> The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see> . </param>
        /// <returns> true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see> ; otherwise, false. This method also returns false if item is not found in the original <see
        ///    cref="T:System.Collections.Generic.ICollection`1"></see> . </returns>
        /// <exception cref="T:System.NotSupportedException">The
        ///   <see cref="T:System.Collections.Generic.ICollection`1"></see>
        ///   is read-only.</exception>
        bool ICollection<TProperty>.Remove(TProperty item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///   Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns> A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection. </returns>
        public IEnumerator<TProperty> GetEnumerator()
        {
            for (var i = 0; i < _list.Count; i++)
                yield return this[i];
        }

        /// <summary>
        ///   Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns> An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection. </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region IVisitableCollection<TProperty> Members
        /// <summary>
        ///   Gets a value indicating whether this instance is of a fixed size.
        /// </summary>
        /// <value> <c>true</c> if this instance is fixed size; otherwise, <c>false</c> . </value>
        public bool IsFixedSize
        {
            get { return true; }
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
        ///   Gets a value indicating whether this collection is full.
        /// </summary>
        /// <value> <c>true</c> if this collection is full; otherwise, <c>false</c> . </value>
        public bool IsFull
        {
            get { return true; }
        }

        /// <summary>
        ///   Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj"> An object to compare with this instance. </param>
        /// <returns> A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than obj. Zero This instance is equal to obj. Greater than zero This instance is greater than obj. </returns>
        /// <exception cref="T:System.ArgumentException">obj is not the same type as this instance.</exception>
        public int CompareTo(object obj)
        {
            if (obj.GetType() == GetType())
            {
                var c = obj as ReadOnlyPropertyList<T, TProperty>;
                return Count.CompareTo(c.Count);
            }
            return String.Compare(GetType().FullName, obj.GetType().FullName, StringComparison.Ordinal);
        }

        /// <summary>
        ///   Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor"> The visitor. </param>
        public void Accept(IVisitor<TProperty> visitor)
        {
            for (var i = 0; i < _list.Count; i++)
                if (!visitor.HasCompleted)
                    visitor.Visit(this[i]);
        }
        #endregion
    }
}