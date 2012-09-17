using System.Collections.Generic;
using System.Linq;

namespace System.Collections
{
    public static class EnumerableExtension
    {
        // note that the template is not used, and we never need to pass one in...
        //IEnumerable data = new[] { new { Foo = "abc" }, new { Foo = "def" }, new { Foo = "ghi" } };
        //var typed = data.Cast(() => new { Foo = "never used" });
        public static IEnumerable<T> Cast<T>(this IEnumerable seq, Func<T> template)
        {
            return Enumerable.Cast<T>(seq);
        }

        public static IEnumerable<T> Convert<T>(this IEnumerable seq)
        {
            
            /*
            foreach (Object obj in seq)
            {
                Object item = obj;
                yield return (T) item;
            }
            */
            return seq.GetEnumerator() as IEnumerable<T>;
        }


        public static TCol ToTypedCollection<TCol, T>(this IEnumerable seq)
            where TCol : IList<T>, new()
        {
            TCol collection = new TCol();

            foreach (Object item in seq)
            {
                collection.Add((T) item);
            }

            return collection;
        }

    }
}
