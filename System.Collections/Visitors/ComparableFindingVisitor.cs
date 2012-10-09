namespace System.Collections.Visitors
{
	/// <summary>
	/// A visitor that searches objects for an equality, using the IComparable interface.
	/// </summary>	
	public sealed class ComparableFindingVisitor<T> : IFindingIVisitor<T>  where T:IComparable
	{
		#region Globals

		bool _found;
		readonly T _searchValue;

		#endregion

		#region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ComparableFindingVisitor&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="searchValue">The search value.</param>
		public ComparableFindingVisitor(T searchValue)
		{
			_searchValue = searchValue;
		}

		#endregion

		#region IVisitor<T> Members

		/// <summary>
		/// Gets a value indicating whether this instance is done performing it's work..
		/// </summary>
		/// <value><c>true</c> if this instance is done; otherwise, <c>false</c>.</value>
		public bool HasCompleted
		{
			get
			{
				return _found;
			}
		}

		/// <summary>
		/// Visits the specified object.
		/// </summary>
		/// <param name="obj">The object.</param>
		public void Visit(T obj)
		{
			if (obj.CompareTo(_searchValue) == 0) {
				_found = true;
			}
		}

		#endregion

		#region IFindingIVisitor<T> Members

		/// <summary>
		/// Gets a value indicating whether the value being searched for has been found.
		/// </summary>
		/// <value><c>true</c> if found; otherwise, <c>false</c>.</value>
		public bool Found
		{
			get
			{
				return _found;
			}
		}

		/// <summary>
		/// Gets the search value.
		/// </summary>
		/// <value>The search value.</value>
		public T SearchValue
		{
			get
			{
				return _searchValue;
			}			
		}

		#endregion
	}
}
