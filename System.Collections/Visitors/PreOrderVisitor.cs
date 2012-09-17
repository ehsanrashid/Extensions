namespace System.Collections.Visitors
{
	/// <summary>
	/// A visitor that visits objects only in pre order.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class PreOrderVisitor<T> : OrderedVisitor<T>
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="PreOrderVisitor&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="visitor">The visitor to use when visiting the object.</param>
		public PreOrderVisitor(IVisitor<T> visitor) : base(visitor) { }

		#endregion

		#region OrderedVisitor<T> Members
				
		/// <summary>
		/// Visits the object in order.
		/// </summary>
		/// <param name="obj">The obj.</param>
		public override void VisitInOrder(T obj)
		{
			// Do nothing.
		}

		/// <summary>
		/// Visits the object in post order.
		/// </summary>
		/// <param name="obj">The obj.</param>
		public override void VisitPostOrder(T obj)
		{
			// Do nothing.
		}

		#endregion
	}
}
