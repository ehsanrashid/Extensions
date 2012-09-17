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

		VisitableList<T> data;
		IComparer<T> comparerToUse;
		
		#endregion

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="SortedList&lt;T&gt;"/> class.
		/// </summary>
		public SortedList()
		{
			data = new VisitableList<T>();
			comparerToUse = Comparer<T>.Default;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SortedList&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="comparer">The comparer to use.</param>
		public SortedList(IComparer<T> comparer)
		{
			data = new VisitableList<T>();
			this.comparerToUse = comparer;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SortedList&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="capacity">The intial capacity of the sorted list.</param>
		public SortedList(int capacity)
		{
			data = new VisitableList<T>(capacity);
			this.comparerToUse = Comparer<T>.Default;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SortedList&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="capacity">The intial capacity of the sorted list.</param>
		/// <param name="comparer">The comparer to use.</param>
		public SortedList(int capacity, IComparer<T> comparer)
		{
			data = new VisitableList<T>(capacity);
			this.comparerToUse = comparer;
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="SortedList&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="collection">The collection to copy into the sorted list.</param>
		public SortedList(IEnumerable<T> collection)
		{
			data = new VisitableList<T>();

			IEnumerator<T> enumerator = collection.GetEnumerator();

			while (enumerator.MoveNext())
			{
				this.Add(enumerator.Current);
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
			data.RemoveAt(index);
		}

        /// <summary>
        /// Gets the comparer.
        /// </summary>
        /// <value>The comparer.</value>
		public IComparer<T> Comparer
		{
			get
			{
				return comparerToUse;
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
			if ((index < 0) || (index > data.Count - 1))
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
			get
			{
				return this.Count == 0;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this collection is full.
		/// </summary>
		/// <value><c>true</c> if this collection is full; otherwise, <c>false</c>.</value>
		public bool IsFull
		{
			get
			{
				return false;
			}
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
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}

			if ((array.Length - arrayIndex) < this.Count)
			{
				throw new ArgumentException(Resources.NotEnoughSpaceInTargetArray);
			}

			for (int i = 0; i < data.Count; i++)
			{
				array.SetValue(data[i], arrayIndex++);
			}
		}

		/// <summary>
		/// Accepts the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		public void Accept(IVisitor<T> visitor)
		{
			if (visitor == null)
			{
				throw new ArgumentNullException("visitor");
			}

			data.Accept(visitor);
		}

		/// <summary>
		/// Adds the specified object.
		/// </summary>
		/// <param name="item">The object to add to the collection</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		public void Add(T item)
		{
			if (data.Count == 0)
			{
				data.Add(item);
			}
			else
			{				
				int index = data.BinarySearch(item, comparerToUse);

				// Item was found
				if (index >= 0)
				{
					data.Insert(index, item);
				}
				else
				{
					data.Insert(~index, item);
				}
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
			return data.Remove(item);
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
			return data.Contains(item);
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.</returns>
		public int Count
		{
			get
			{
				return data.Count;
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<T> GetEnumerator()
		{
			return data.GetEnumerator();
		}

		/// <summary>
		/// Clears all the objects in this instance.
		/// </summary>
		public void Clear()
		{
			data.Clear();
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
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}

			if (obj.GetType() == this.GetType())
			{
				SortedList<T> list = obj as SortedList<T>;
				return this.Count.CompareTo(list.Count);
			}
			else
			{
				return this.GetType().FullName.CompareTo(obj.GetType().FullName);
			}
		}

		#endregion

		#region Operator Overloads

		/// <summary>
		/// Gets the item at the specified position.
		/// </summary>
		/// <value></value>
		public T this[int i]  {
			get
			{
				CheckIndexValid(i);
				return data[i];
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
			get {
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
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
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
			return data.IndexOf(item);
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
