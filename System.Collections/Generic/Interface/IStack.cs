namespace System.Collections.Generic
{
    /// <summary>
    ///   An Interface for the Stack data structure
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    public interface IStack<T>
    {
        /// <summary>
        ///   Pushes the specified item onto the stack.
        /// </summary>
        /// <param name="item"> The item. </param>
        void Push(T item);

        /// <summary>
        ///   Pops this instance from the stack.
        /// </summary>
        /// <returns> The item at the top of the stack. </returns>
        T Pop();

        /// <summary>
        ///   Peeks at the top item without popping it from the stack.
        /// </summary>
        /// <returns> The item at the top of the stack. </returns>
        T Peek();
    }
}