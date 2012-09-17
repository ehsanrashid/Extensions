
namespace System.Collections.Visitors
{
	/// <summary>
	/// Provides an interface for visitors.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IVisitor<T>
	{
		/// <summary>
		/// Gets a value indicating whether this instance is done performing it's work..
		/// </summary>
		/// <value><c>true</c> if this instance is done; otherwise, <c>false</c>.</value>
		bool HasCompleted { get; }

		/// <summary>
		/// Visits the specified object.
		/// </summary>
		/// <param name="obj">The object to visit.</param>
		void Visit(T obj);
	}
}
