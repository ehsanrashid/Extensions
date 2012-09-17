namespace System.Collections.Generic
{
	/// <summary>
	/// An interface for the Heap data structure.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IHeap<T>
	{
		/// <summary>
		/// Adds the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		void Add(T item);

		/// <summary>
		/// Removes the root and returns it.
		/// </summary>
		/// <returns>The root of the heap.</returns>
		T RemoveRoot();

		/// <summary>
		/// Gets the root.
		/// </summary>
		/// <value>The root.</value>
		T Root { get; }
	}
}
