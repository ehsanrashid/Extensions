namespace System.Collections.Generic.Interface
{
    public interface IPaged
    {
        int PageIndex { get; }
        int PageSize { get; }

        bool HasNextPage { get; }
        bool HasPreviousPage { get; }

        int NoOfItems { get; }
        int NoOfPages { get; }
    }

    public interface IPagedList<T> : IPaged, IList<T>
    {
    }
}
