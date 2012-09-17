namespace System.Collections.Generic
{
	/// <summary>
	/// An interface for a dequeue
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IDeque<T>
	{
		/// <summary>
		/// Dequeues the head.
		/// </summary>
		/// <returns>The head of the deque.</returns>
		T DequeueHead();

		/// <summary>
		/// Dequeues the tail.
		/// </summary>
		/// <returns>The tail of the deque.</returns>
		T DequeueTail();

		/// <summary>
		/// Enqueues the head.
		/// </summary>
		/// <param name="obj">The obj.</param>
		void EnqueueHead(T obj);

		/// <summary>
		/// Enqueues the tail.
		/// </summary>
		/// <param name="obj">The obj.</param>
		void EnqueueTail(T obj);

		/// <summary>
		/// Gets the head.
		/// </summary>
		/// <value>The head.</value>
		T Head { get; }

		/// <summary>
		/// Gets the tail.
		/// </summary>
		/// <value>The tail.</value>
		T Tail { get; }
	}
}
