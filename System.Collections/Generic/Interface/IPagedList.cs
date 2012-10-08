namespace System.Collections.Generic.Interface
{
    public interface IPagedList
    {
        int PageIndex { get; }
        int PageSize { get; }

        bool HasNextPage { get; }
        bool HasPreviousPage { get; }

        int TotalCount { get; }
        int TotalPages { get; }
    }
}
