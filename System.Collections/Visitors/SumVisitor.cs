namespace System.Collections.Visitors
{
	/// <summary>
	/// A visitor that sums integers in any collection of type int.
	/// </summary>
	public sealed class SumVisitor : IVisitor<int>
	{
		#region Globals

		private int sum;

		#endregion

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="SumVisitor"/> class.
		/// </summary>
		public SumVisitor() { }

		#endregion

		#region IVisitor<int> Members

		/// <summary>
		/// Visits the specified object.
		/// </summary>
		/// <param name="obj">The object.</param>
		public void Visit(int obj)
		{
			sum += obj;
		}

		/// <summary>
		/// Gets a value indicating whether this instance is done performing it's work..
		/// </summary>
        /// <returns><c>true</c> if this instance is done; otherwise, <c>false</c>.</returns>
		/// <value><c>true</c> if this instance is done; otherwise, <c>false</c>.</value>
		public bool HasCompleted
		{
			get
			{
				return false;
			}
		}

		#endregion

		#region Public Members

		/// <summary>
		/// Gets the sum accumulated by this SumVisitor.
		/// </summary>
		/// <value>The sum.</value>
		public int Sum
		{
			get
			{
				return sum;
			}
		}

		#endregion
	}
}
