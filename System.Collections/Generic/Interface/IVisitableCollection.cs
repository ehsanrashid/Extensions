namespace System.Collections.Generic
{ 
	/// <summary>
	/// The interface for a custom collection, extending the standard ICollection interface.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IVisitableCollection<T> : ICollection<T>, IEnumerable<T>, IComparable, IVisitable<T>
	{
		/// <summary>
		/// Gets a value indicating whether this instance is of a fixed size.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is fixed size; otherwise, <c>false</c>.
		/// </value>
		bool IsFixedSize { get;}
		
		/// <summary>
		/// Gets a value indicating whether this collection is empty.
		/// </summary>
		/// <value><c>true</c> if this collection is empty; otherwise, <c>false</c>.</value>
		bool IsEmpty { get;}

		/// <summary>
		/// Gets a value indicating whether this collection is full.
		/// </summary>
		/// <value><c>true</c> if this collection is full; otherwise, <c>false</c>.</value>
		bool IsFull { get;}
	}
}
