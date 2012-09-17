namespace System.Collections.Visitors
{
	/// <summary>
	/// An Post order implementation of the <see cref="OrderedVisitor&lt;T&gt;"/> class.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class PostOrderVisitor<T> : OrderedVisitor<T>
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="PostOrderVisitor&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="visitor">The visitor to use when visiting the object.</param>
		public PostOrderVisitor(IVisitor<T> visitor) : base(visitor) { }

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
		/// Visits the object in pre order.
		/// </summary>
		/// <param name="obj">The obj.</param>
		public override void VisitPreOrder(T obj)
		{
			// Do nothing.
		}

		#endregion
	}
}
