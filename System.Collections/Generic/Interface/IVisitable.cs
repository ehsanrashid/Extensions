namespace System.Collections.Generic
{
    using Visitors;

    /// <summary>
    /// The IVisitable interface provides data structures a method of allowing a visitor
    /// to visit every object contained in the structure.
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public interface IVisitable<out T>
	{
		/// <summary>
		/// Accepts the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		void Accept(IVisitor<T> visitor);
	}
}
