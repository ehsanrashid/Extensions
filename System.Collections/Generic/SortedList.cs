namespace System.Collections.Generic
{
    using Visitors;
    using Properties;

    /// <summary>
    /// An implementation of a SortedList datastructure, which keeps any objects
    /// added to it sorted.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SortedList<T> : IVisitableCollection<T>, IList<T>
    {
        #region Globals

        readonly VisitableList<T> _data;
        readonly IComparer<T> _comparerToUse;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedList&lt;T&gt;"/> class.
        /// </summary>
        public SortedList()
        {
            _data = new VisitableList<T>();
            _comparerToUse = Comparer<T>.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedList&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="comparer">The comparer to use.</param>
        public SortedList(IComparer<T> comparer)
        {
            _data = new VisitableList<T>();
            _comparerToUse = comparer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedList&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="capacity">The intial capacity of the sorted list.</param>
        public SortedList(int capacity)
        {
            _data = new VisitableList<T>(capacity);
            _comparerToUse = Comparer<T>.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedList&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="capacity">The intial capacity of the sorted list.</param>
        /// <param name="comparer">The comparer to use.</param>
        public SortedList(int capacity, IComparer<T> comparer)
        {
            _data = new VisitableList<T>(capacity);
            _comparerToUse = comparer;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="SortedList&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="collection">The collection to copy into the sorted list.</param>
        public SortedList(IEnumerable<T> collection)
        {
            _data = new VisitableList<T>();

            var enumerator = collection.GetEnumerator();

            while (enumerator.MoveNext())
            {
                Add(enumerator.Current);
            }
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Removes the item at the specified index.
        /// </summary>
        /// <param name="index">The index of the item</param>
        public void RemoveAt(int index)
        {
            CheckIndexValid(index);
            _data.RemoveAt(index);
        }

        /// <summary>
        /// Gets the comparer.
        /// </summary>
        /// <value>The comparer.</value>
        public IComparer<T> Comparer
        {
            get
            {
                return _comparerToUse;
            }
        }

        #endregion

        #region Private Members

        /// <summary>
        /// Checks if the index is valid.
        /// </summary>
        /// <param name="index">The index to check.</param>
        private void CheckIndexValid(int index)
        {
            if ((index < 0) || (index > _data.Count - 1))
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        #region IVisitableCollection<T> Members

        /// <summary>
        /// Gets a value indicating whether this instance is of a fixed size.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is fixed size; otherwise, <c>false</c>.
        /// </value>
        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this collection is empty.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this collection is empty; otherwise, <c>false</c>.
        /// </value>
        public bool IsEmpty
        {
            get { return Count == 0; }
        }

        /// <summary>
        /// Gets a value indicating whether this collection is full.
        /// </summary>
        /// <value><c>true</c> if this collection is full; otherwise, <c>false</c>.</value>
        public bool IsFull
        {
            get { return false; }
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
        /// <exception cref="T:System.ArgumentNullException">array is null.</exception>
        /// <exception cref="T:System.ArgumentException">array is multidimensional.-or-arrayIndex is equal to or greater than the length of array.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"></see> is greater than the available space from arrayIndex to the end of the destination array.-or-Type T cannot be cast automatically to the type of the destination array.</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException("array");
            if ((array.Length - arrayIndex) < Count) throw new ArgumentException(Resources.NotEnoughSpaceInTargetArray);
            foreach (var item in _data)
            {
                array.SetValue(item, arrayIndex++);
            }
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public void Accept(IVisitor<T> visitor)
        {
            if (visitor == null) throw new ArgumentNullException("visitor");

            _data.Accept(visitor);
        }

        /// <summary>
        /// Adds the specified object.
        /// </summary>
        /// <param name="item">The object to add to the collection</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        public void Add(T item)
        {
            if (_data.Count == 0)
                _data.Add(item);
            else
            {
                var index = _data.BinarySearch(item, _comparerToUse);

                // Item was found
                _data.Insert(index >= 0 ? index : ~index, item);
            }
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>
        /// true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        public bool Remove(T item)
        {
            return _data.Remove(item);
        }

        /// <summary>
        /// Determines whether the Sorted List contains the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// 	<c>true</c> if the item is contained in the list; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(T item)
        {
            return _data.Contains(item);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.
        /// </summary>
        /// <value></value>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.</returns>
        public int Count
        {
            get { return _data.Count; }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        /// <summary>
        /// Clears all the objects in this instance.
        /// </summary>
        public void Clear()
        {
            _data.Clear();
        }

        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than obj. Zero This instance is equal to obj. Greater than zero This instance is greater than obj.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">obj is not the same type as this instance. </exception>
        public int CompareTo(object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            if (obj.GetType() == GetType())
            {
                var list = obj as SortedList<T>;
                return Count.CompareTo(list.Count);
            }
            return String.Compare(GetType().FullName, obj.GetType().FullName, StringComparison.Ordinal);
        }

        #endregion

        #region Operator Overloads

        /// <summary>
        /// Gets the item at the specified position.
        /// </summary>
        /// <value></value>
        public T this[int i]
        {
            get
            {
                CheckIndexValid(i);
                return _data[i];
            }
        }

        #endregion

        #region ICollection<T> Members

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IList<T> Members

        /// <summary>
        /// Returns the first index of the item specified.
        /// </summary>
        /// <param name="item">The index of the first item found, -1 if the item isn't found.</param>
        /// <returns>
        /// The index of item if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(T item)
        {
            return _data.IndexOf(item);
        }

        /// <summary>
        /// Inserts the item at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item to insert.</param>
        void IList<T>.Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets or sets the value at the specified index.
        /// </summary>
        /// <value></value>
        T IList<T>.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        #endregion
    }
}
