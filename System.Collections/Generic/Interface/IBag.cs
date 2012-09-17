
namespace System.Collections.Generic
{
	/// <summary>
	///  An interface for a Bag data structure.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IBag<T>
	{

		/// <summary>
		/// Adds n * the specified item to the bag.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="amount">The amount.</param>
		void Add(T item, int amount);

		/// <summary>
		/// Applies the Difference operation on two bags.
		/// </summary>
		/// <param name="bag">The other bag.</param>
		/// <returns>The difference between the current bag and the specified bag.</returns>
		IBag<T> Difference(IBag<T> bag);

		/// <summary>
		/// Applies the Intersection opertion on two bags.
		/// </summary>
		/// <param name="bag">The other bag.</param>
		/// <returns>The intersection of the current bag and the specified bag.</returns>
		IBag<T> Intersection(IBag<T> bag);

		/// <summary>
		/// Determines whether the specified bag is equal to this instance.
		/// </summary>
		/// <param name="bag">The bag to compare.</param>
		/// <returns>
		/// 	<c>true</c> if the specified bag is equal; otherwise, <c>false</c>.
		/// </returns>
		bool IsEqual(IBag<T> bag);

		/// <summary>
		/// Removes the specified amount of items from the bag.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="max">The maximum amount of items to remove.</param>
		/// <returns>An indication of whether the items were found (and removed).</returns>
		bool Remove(T item, int max);

		/// <summary>
		/// Gets the count of the specified item contained in the bag.
		/// </summary>
		/// <value></value>
		int this[T item] { get; }

		/// <summary>
		/// Applies the Union operation with two bags.
		/// </summary>
		/// <param name="bag">The other bag.</param>
		/// <returns>The union of the current bag and the specified bag.</returns>
		IBag<T> Union(IBag<T> bag);
	}
}
