namespace System.Collections.Visitors
{
    /// <summary>
    /// A Visitor that counts items in a visitable collection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public sealed class CountingVisitor<T> : IVisitor<T>
	{
		#region Globals

		int currentCount;

		#endregion

		#region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="CountingVisitor&lt;T&gt;"/> class.
        /// </summary>
		public CountingVisitor()
		{

		}

		#endregion

		#region Visitor<T> Members

		/// <summary>
		/// Gets a value indicating whether this instance is done performing it's work..
		/// </summary>
		/// <value><c>true</c> if this instance is done; otherwise, <c>false</c>.</value>
		public bool HasCompleted
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Visits the specified object.
		/// </summary>
		/// <param name="obj">The object to visit.</param>
		public void Visit(T obj)
		{
			currentCount++;
		}

		#endregion

		#region Public Members

		/// <summary>
		/// Resets the count to zero.
		/// </summary>
		public void ResetCount()
		{
			currentCount = 0;
		}

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <value>The current count value.</value>
		public int Count
		{
			get
			{
				return currentCount;
			}
		}

		#endregion
	}
}
