namespace System.Collections.Visitors
{
	/// <summary>
	/// A visitor that visits objects in order (PreOrder, PostOrder, or InOrder).
	/// Used primarily as a base class for Visitors specializing in a specific order type.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class OrderedVisitor<T>
	{
		#region Globals
		
		readonly IVisitor<T> _visitorToUse;
		
		#endregion

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="OrderedVisitor&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="visitorToUse">The visitor to use when visiting the object.</param>
		public OrderedVisitor(IVisitor<T> visitorToUse)
		{
		    if (visitorToUse == null) throw new ArgumentNullException("visitorToUse");
		    _visitorToUse = visitorToUse;
		}

	    #endregion

		#region IOrderedVisitor<T> Members

		/// <summary>
		/// Determines whether this visitor is done.
		/// </summary>
		/// <value></value>
		/// <returns>
		/// 	<c>true</c> if this visitor is done; otherwise, <c>false</c>.
		/// </returns>
		public bool HasCompleted
		{
			get
			{
				return _visitorToUse.HasCompleted;
			}
		}

		/// <summary>
		/// Visits the object in pre order.
		/// </summary>
		/// <param name="obj">The obj.</param>
		public virtual void VisitPreOrder(T obj) {
			_visitorToUse.Visit(obj);
		}

		/// <summary>
		/// Visits the object in post order.
		/// </summary>
		/// <param name="obj">The obj.</param>
		public virtual void VisitPostOrder(T obj)
		{
			_visitorToUse.Visit(obj);
		}

		/// <summary>
		/// Visits the object in order.
		/// </summary>
		/// <param name="obj">The obj.</param>
		public virtual void VisitInOrder(T obj)
		{
			_visitorToUse.Visit(obj);
		}

		#endregion

		#region Public Members

		/// <summary>
		/// Gets the visitor to use.
		/// </summary>
		/// <value>The visitor to use.</value>
		public IVisitor<T> VisitorToUse
		{
			get
			{
				return _visitorToUse;
			}
		}

		#endregion
	}
}
