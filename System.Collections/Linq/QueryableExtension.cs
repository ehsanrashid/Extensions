using System.Collections.Generic;
using System.Web.Mvc;

namespace System.Linq
{
    public static class QueryableExtension
    {
        /// <summary>
        /// Apply paging to an IQueryable.
        /// </summary>
        /// <typeparam name="T">The type that the collection contains.</typeparam>
        /// <param name="query">The data source.</param>
        /// <param name="pageIndex">The page index, starting at 0.</param>
        /// <param name="pageSize">The max number of items to return.</param>
        /// <returns>The resulting object collection.</returns>
        public static IQueryable<T> Page<T>(this IQueryable<T> query, int? pageIndex, int pageSize)
        {
            return query.Skip((pageIndex ?? 0) * pageSize).Take(pageSize);
        }


        /// <summary>
        /// Returns a paginated list.
        /// </summary>
        /// <typeparam name="T">Type of items in list.</typeparam>
        /// <param name="query">A IQueryable instance to apply.</param>
        /// <param name="pageIndex">Page number that starts with zero.</param>
        /// <param name="pageSize">Size of each page.</param>
        /// <returns>Returns a paginated list.</returns>
        /// <remarks>This functionality may not work in SQL Compact 3.5</remarks>
        /// <example>
        ///     Following example shows how use this extension method in ASP.NET MVC web application.
        ///     <code>
        ///     public ActionResult Customers(int? page, int? size)
        ///     {
        ///         var q = from p in customers
        ///                 select p;
        ///     
        ///         return View(q.ToPaginatedList(page.HasValue ? page.Value : 1, size.HasValue ? size.Value : 15));
        ///     }
        ///     </code>
        /// </example>
        public static PaginatedList<T> ToPaginatedList<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        {
            return new PaginatedList<T>(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Returns a paginated list. This function returns 15 rows from specific pageIndex.
        /// </summary>
        /// <typeparam name="T">Type of items in list.</typeparam>
        /// <param name="query">A IQueryable instance to apply.</param>
        /// <param name="pageIndex">Page number that starts with zero.</param>    
        /// <returns>Returns a paginated list.</returns>
        /// <remarks>This functionality may not work in SQL Compact 3.5</remarks>
        public static PaginatedList<T> ToPaginatedList<T>(this IQueryable<T> query, int pageIndex)
        {
            return new PaginatedList<T>(query, pageIndex, 15);
        }

        /// <summary>
        /// Returns a paginated list. This function returns 15 rows from page one.
        /// </summary>
        /// <typeparam name="T">Type of items in list.</typeparam>
        /// <param name="query">A IQueryable instance to apply.</param>    
        /// <returns>Returns a paginated list.</returns>
        /// <remarks>This functionality may not work in SQL Compact 3.5</remarks>
        public static PaginatedList<T> ToPaginatedList<T>(this IQueryable<T> query)
        {
            return new PaginatedList<T>(query, 1, 15);
        }


        public static SelectList ToSelectList<T>(this IQueryable<T> query, string dataValueField, string dataTextField, object selectedValue)
        {
            return new SelectList(query, dataValueField, dataTextField, selectedValue ?? -1);
        }

    }
}