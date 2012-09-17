namespace System.Collections.Generic
{
    /// <summary>
    /// A queue interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQueue<T>
    {
        /// <summary>
        /// Enqueues the item at the back of the queue.
        /// </summary>
        /// <param name="item">The item.</param>
        void Enqueue(T item);

        /// <summary>
        /// Dequeues the item at the front of the queue.
        /// </summary>
        /// <returns>The item at the front of the queue.</returns>
        T Dequeue();

        /// <summary>
        /// Peeks at the item in the front of the queue, without removing it.
        /// </summary>
        /// <returns>The item at the front of the queue.</returns>
        T Peek();
    }
}
