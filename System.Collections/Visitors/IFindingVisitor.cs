namespace System.Collections.Visitors
{

	public interface IFindingIVisitor<T> : IVisitor<T>
	{
		bool Found { get; }
		T SearchValue { get; }
	}
}
