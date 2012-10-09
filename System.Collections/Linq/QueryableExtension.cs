

namespace System.Linq
{
    using Collections.Generic;
    using Web.Mvc;
    using Expressions;
    using DirectoryServices;

    public static class QueryableExtension
    {
        public static IQueryable<T> And<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> predicate)
        {
            return queryable.Where(predicate);
        }


        public static IQueryable<T> Sort<T>(this IQueryable<T> query, String sortField, SortDirection direction)
        {
            return direction == SortDirection.Ascending
                       ? query.OrderBy(s => s.GetType().GetProperty(sortField))
                       : query.OrderByDescending(s => s.GetType().GetProperty(sortField));
        }

        public static IQueryable<T> Skip<T>(this IQueryable<T> queryable, int? count)
        {
            return (count.HasValue ? Queryable.Skip(queryable, count.Value) : queryable);
        }

        public static IQueryable<T> WhereAny<T>(this IQueryable<T> source, params Expression<Func<T, bool>>[] predicates)
        {
            if (null == source) throw new ArgumentNullException("source");
            if (null == predicates) throw new ArgumentNullException("predicates");
            if (0 == predicates.Length) return source.Where(x => false); // no matches!
            if (1 == predicates.Length) return source.Where(predicates[0]); // simple

            var paramExpr = Expression.Parameter(typeof(T), "x");
            Expression body = Expression.Invoke(predicates[0], paramExpr);
            for (var i = 1; i < predicates.Length; ++i)
            {
                body = Expression.OrElse(body, Expression.Invoke(predicates[i], paramExpr));
            }
            return source.Where(Expression.Lambda<Func<T, bool>>(body, paramExpr));
        }


        /*
        public static IQueryable OrderBy(this IQueryable source,
                                      String ordering,
                                      params object[] values)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (ordering == null) throw new ArgumentNullException("ordering");
            ParameterExpression[] parameters = new ParameterExpression[] {
        Expression.Parameter(source.ElementType, "") };
            ExpressionParser parser = new ExpressionParser(parameters,
                                                           ordering,
                                                           values);
            IEnumerable<DynamicOrdering> orderings = parser.ParseOrdering();
            Expression queryExpr = source.Expression;
            String methodAsc = "OrderBy";
            String methodDesc = "OrderByDescending";
            foreach (DynamicOrdering o in orderings)
            {
                queryExpr = Expression.Call(
                    typeof(Queryable), o.Ascending ? methodAsc : methodDesc,
                    new Type[] { source.ElementType, o.Selector.Type },
                    queryExpr, Expression.Quote(Expression.Lambda(o.Selector,
                                                                  parameters)));
                methodAsc = "ThenBy";
                methodDesc = "ThenByDescending";
            }
            return source.Provider.CreateQuery(queryExpr);
        }
        */

        ///// <summary>
        ///// Apply paging to an IQueryable.
        ///// </summary>
        ///// <typeparam name="T">The type that the collection contains.</typeparam>
        ///// <param name="query">The data source.</param>
        ///// <param name="pageIndex">The page index, starting at 1.</param>
        ///// <param name="pageSize">The max number of items to return.</param>
        ///// <returns>The resulting object collection.</returns>
        //public static IQueryable<T> Page<T>(this IQueryable<T> query, int? pageIndex, int pageSize)
        //{
        //    return query.Skip((pageIndex ?? 1) * pageSize).Take(pageSize);
        //}

        ///// <summary>
        ///// Returns a paginated list.
        ///// </summary>
        ///// <typeparam name="T">Type of items in list.</typeparam>
        ///// <param name="query">A IQueryable instance to apply.</param>
        ///// <param name="pageIndex">Page number that starts with zero.</param>
        ///// <param name="pageSize">Size of each page.</param>
        ///// <returns>Returns a paginated list.</returns>
        ///// <remarks>This functionality may not work in SQL Compact 3.5</remarks>
        ///// <example>
        /////     Following example shows how use this extension method in ASP.NET MVC web application.
        /////     <code>
        /////     public ActionResult Customers(int? page, int? size)
        /////     {
        /////         var q = from p in customers
        /////                 select p;
        /////     
        /////         return View(q.ToPaginatedList(page.HasValue ? page.Value : 1, size.HasValue ? size.Value : 15));
        /////     }
        /////     </code>
        ///// </example>
        //public static PagedList<T> ToPagedList<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        //{
        //    return new PagedList<T>(query, pageIndex, pageSize);
        //}

        ///// <summary>
        ///// Returns a paginated list. This function returns 15 rows from specific pageIndex.
        ///// </summary>
        ///// <typeparam name="T">Type of items in list.</typeparam>
        ///// <param name="query">A IQueryable instance to apply.</param>
        ///// <param name="pageIndex">Page number that starts with zero.</param>    
        ///// <returns>Returns a paginated list.</returns>
        ///// <remarks>This functionality may not work in SQL Compact 3.5</remarks>
        //public static PagedList<T> ToPagedList<T>(this IQueryable<T> query, int pageIndex)
        //{
        //    return new PagedList<T>(query, pageIndex, 15);
        //}

        ///// <summary>
        ///// Returns a paginated list. This function returns 15 rows from page one.
        ///// </summary>
        ///// <typeparam name="T">Type of items in list.</typeparam>
        ///// <param name="query">A IQueryable instance to apply.</param>    
        ///// <returns>Returns a paginated list.</returns>
        ///// <remarks>This functionality may not work in SQL Compact 3.5</remarks>
        //public static PagedList<T> ToPagedList<T>(this IQueryable<T> query)
        //{
        //    return new PagedList<T>(query, 1, 15);
        //}

        //public static SelectList ToSelectList<T>(this IQueryable<T> query, String dataValueField, String dataTextField, object selectedValue)
        //{
        //    return new SelectList(query, dataValueField, dataTextField, selectedValue ?? -1);
        //}
    }
}