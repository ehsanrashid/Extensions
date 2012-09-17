namespace System.Collections.Sorter
{
    using Linq;
    using Linq.Expressions;
    using Generic;

    /// <summary>
    ///   This help class is used to encapsule sorting logic.
    /// </summary>
    /// <typeparam name="T"> The type to sort. </typeparam>
    public class GenericSorter<T>
    {
        public IEnumerable<T> Sort(IEnumerable<T> sequence, String sortBy)
        {
            var paramExp = Expression.Parameter(typeof (T), "item");
            var sortExpression =
                Expression.Lambda<Func<T, Object>>
                    (Expression.Convert(Expression.Property(paramExp, sortBy),
                                        typeof (Object)), paramExp);
            return sequence.AsQueryable().OrderBy(sortExpression);
        }
    }
}