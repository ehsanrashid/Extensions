namespace System.Collections.Generic
{
    using Sorter;
    using Globalization;
    using Linq;
    using Reflection;
    using Text;
    using Web.Mvc;

    public static class EnumerableExtension
    {
        /// <summary>
        ///   Returns true if the <paramref name="enumerable" /> is null or without any items.
        /// </summary>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return (null == enumerable) || !enumerable.Any();
        }

        /// <summary>
        ///   Returns true if the <paramref name="enumerable" /> is contains at least one item.
        /// </summary>
        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return !IsNullOrEmpty(enumerable);
        }

        static IEnumerable<T> Where<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            if (null == enumerable) throw new ArgumentNullException("enumerable");
            //foreach (T item in enumerable)
            //{
            //    if (predicate(item))
            //        yield return item;
            //}
            return Enumerable.Where(enumerable, predicate);
        }

        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> enumerable) where T : class
        {
            return Where(enumerable, x => x.IsNotNull());
        }


        public static String Join<T>(this IEnumerable<T> enumerable, String separator)
        {
            //if (null == enumerable) return String.Empty;
            // ---------------------------------------------------
            //return enumerable.Aggregate(String.Empty,
            //            (join, item) => join + item + separator,
            //            (join) =>
            //            {
            //                if (0 < join.Length) join = join.Remove(join.Length - 1);
            //                return join;
            //            });
            // ---------------------------------------------------
            //return enumerable.Aggregate(new StringBuilder(),
            //            (sb, v) => sb.Append(v).Append(separator),
            //            (sb) =>
            //            {
            //                if (0 < sb.Length) sb.Length--;
            //                return sb.ToString();
            //            });
            // ---------------------------------------------------
            return (null == enumerable)
                       ? String.Empty
                       : // String.Join(separator, enumerable.Select((item) => item.ToString()));
                // String.Join(separator, enumerable.Select((item) => item));
                   String.Join(separator, enumerable);
        }

        /// <summary>
        ///   Performs an action for each item in the enumerable
        /// </summary>
        /// <typeparam name="T"> The enumerable data type </typeparam>
        /// <param name="enumerable"> The data values. </param>
        /// <param name="action"> The action to be performed. </param>
        /// <example>
        ///   var values = new[] { "1", "2", "3" };
        ///   values.ConvertList&lt;String, int&gt;().ForEach(Console.WriteLine);
        /// </example>
        /// <remarks>
        ///   This method was intended to return the passed values to provide method chaining. Howver due to defered execution the compiler would actually never run the entire code at all.
        /// </remarks>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (null == enumerable) throw new ArgumentNullException("enumerable");
            if (null == action) throw new ArgumentNullException("action");
            foreach (var item in enumerable)
                action(item);
        }

        public static String Join<T>(this IEnumerable<T[]> enumerable)
        {
            var sb = new StringBuilder();
            var firstRow = true;
            foreach (var row in enumerable)
            {
                if (firstRow)
                    firstRow = false;
                else
                    sb.AppendLine();
                if (row.Length > 0)
                {
                    sb.Append(row[0]);
                    for (var i = 1; i < row.Length; ++i)
                        sb.Append(',').Append(row[i]);
                }
            }
            return sb.ToString();
        }

        ///<summary>
        ///  Returns enumerable object based on target, which does not contains null references.
        ///  If target is null reference, returns empty enumerable object.
        ///</summary>
        ///<typeparam name="T"> Type of items in target. </typeparam>
        ///<param name="enumerable"> Target enumerable object. Can be null. </param>
        ///<example>
        ///  object[] items = null;
        ///  foreach(var item in items.NotNull()){
        ///  // result of items.NotNull() is empty but not null enumerable
        ///  }
        /// 
        ///  object[] items = new object[]{ null, "Hello World!", null, "Good bye!" };
        ///  foreach(var item in items.NotNull()){
        ///  // result of items.NotNull() is enumerable with two strings
        ///  }
        ///</example>
        ///<remarks>
        ///  Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
        ///</remarks>
        public static IEnumerable<T> IgnoreNulls<T>(this IEnumerable<T> enumerable)
        {
            if (null == enumerable) yield break;
            foreach (var item in enumerable.Where(item => !ReferenceEquals(item, null)))
                yield return item;
        }

        ///<summary>
        ///  Get Distinct
        ///</summary>
        ///<param name="enumerable"> </param>
        ///<param name="expression"> </param>
        ///<typeparam name="T"> </typeparam>
        ///<typeparam name="TKey"> </typeparam>
        ///<returns> </returns>
        ///<remarks>
        ///  Contributed by Michael T, http://about.me/MichaelTran
        ///</remarks>
        public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> expression)
        {
            return (null == enumerable) ? Enumerable.Empty<T>() : enumerable.GroupBy(expression).Select(i => i.First());
        }

        ///<summary>
        ///  Remove matching items from a list
        ///</summary>
        ///<param name="enumerable"> </param>
        ///<param name="predicate"> </param>
        ///<typeparam name="T"> </typeparam>
        ///<returns> </returns>
        [Obsolete("Use RemoveWhere instead..")]
        public static IEnumerable<T> RemoveAll<T>(this IEnumerable<T> enumerable, Predicate<T> predicate)
        {
            if (null == enumerable) return Enumerable.Empty<T>();
            var list = enumerable.ToList();
            list.RemoveAll(predicate);
            return list;
        }

        /// <summary>
        ///   Removes matching items from a enumerable
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="enumerable"> The source. </param>
        /// <param name="predicate"> The predicate. </param>
        /// <returns> </returns>
        /// <remarks>
        ///   Renamed by James Curran, to match corresponding HashSet.RemoveWhere()
        /// </remarks>
        public static IEnumerable<T> RemoveWhere<T>(this IEnumerable<T> enumerable, Predicate<T> predicate)
        {
            if (null == enumerable) yield break;
            foreach (var t in Where(enumerable, t => !predicate(t)))
                yield return t;
        }

        ///<summary>
        ///  Turn the list of objects to a String of Common Seperated Value
        ///</summary>
        ///<param name="enumerable"> </param>
        ///<param name="separator"> </param>
        ///<typeparam name="T"> </typeparam>
        ///<returns> </returns>
        ///<example>
        ///  <code>var values = new[] { 1, 2, 3, 4, 5 };
        ///    String csv = values.ToCSV(';');</code>
        ///</example>
        public static String ToCSV<T>(this IEnumerable<T> enumerable, char separator = ',')
        {
            if (null == enumerable) return String.Empty;
            var sbCSV = new StringBuilder();
            enumerable.ForEach(value => sbCSV.Append(String.Concat(value, separator)));
            return sbCSV.ToString(0, sbCSV.Length - 1);
        }

        /// <summary>
        ///   Returns the first item or the <paramref name="defaultValue" /> if the <paramref name="enumerable" />
        ///   does not contain any item.
        /// </summary>
        public static T FirstOrDefault<T>(this IEnumerable<T> enumerable, T defaultValue)
        {
            var list = enumerable as List<T> ?? enumerable.ToList();
            return list.IsNotNullOrEmpty() ? list.First() : defaultValue;
        }

        /// <summary>
        ///   Appends an element to the end of the current collection and returns the new collection.
        /// </summary>
        /// <typeparam name="T"> The enumerable data type </typeparam>
        /// <param name="enumerable"> The data values. </param>
        /// <param name="item"> The element to append the current collection with. </param>
        /// <returns> The modified collection. </returns>
        /// <example>
        ///   var integers = Enumerable.Range(0, 3);  // 0, 1, 2
        ///   integers = integers.Append(3);          // 0, 1, 2, 3
        /// </example>
        public static IEnumerable<T> Append<T>(this IEnumerable<T> enumerable, T item)
        {
            foreach (var i in enumerable)
                yield return i;
            yield return item;
        }

        /// <summary>
        ///   Prepends an element to the start of the current collection and returns the new collection.
        /// </summary>
        /// <typeparam name="T"> The enumerable data type </typeparam>
        /// <param name="enumerable"> The data values. </param>
        /// <param name="item"> The element to prepend the current collection with. </param>
        /// <returns> The modified collection. </returns>
        /// <example>
        ///   var integers = Enumerable.Range(1, 3);  // 1, 2, 3
        ///   integers = integers.Prepend(0);         // 0, 1, 2, 3
        /// </example>
        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> enumerable, T item)
        {
            yield return item;
            foreach (var i in enumerable)
                yield return i;
        }

        /// <summary>
        ///   Converts an enumeration of groupings into a Dictionary of those groupings.
        /// </summary>
        /// <typeparam name="TKey"> Key type of the grouping and dictionary. </typeparam>
        /// <typeparam name="TValue"> Element type of the grouping and dictionary list. </typeparam>
        /// <param name="groupings"> The enumeration of groupings from a GroupBy() clause. </param>
        /// <returns> A dictionary of groupings such that the key of the dictionary is TKey type and the value is List of TValue type. </returns>
        public static Dictionary<TKey, List<TValue>> ToDictionary<TKey, TValue>(
            this IEnumerable<IGrouping<TKey, TValue>> groupings)
        {
            return groupings.ToDictionary(group => @group.Key, group => @group.ToList());
        }

        /// <summary>
        ///   Returns whether the enumerable contains a certain amount of elements.
        /// </summary>
        /// <typeparam name="T"> The type of the elements of the input enumerable. </typeparam>
        /// <param name="enumerable"> The source for this extension method. </param>
        /// <param name="count"> The amount of elements the enumerable should contain. </param>
        /// <returns> True when the enumerable contains the specified amount of elements, false otherwise. </returns>
        public static bool HasCountOf<T>(this IEnumerable<T> enumerable, int count)
        {
            return enumerable.Take(count + 1).Count() == count;
        }

        /// <summary>
        ///   Overload the Select to allow null as a return
        /// </summary>
        /// <typeparam name="T1"> </typeparam>
        /// <typeparam name="T2"> </typeparam>
        /// <param name="enumerable"> </param>
        /// <param name="selector"> </param>
        /// <param name="allowNull"> </param>
        /// <returns> An <see cref="IEnumerable{TResult}" /> using the selector containing null or non-null results based on <see
        ///    cref="allowNull" /> . </returns>
        /// <example>
        ///   <code>var list = new List{object}{ new object(), null, null };
        ///     var noNulls = list.Select(x => x, false);</code>
        /// </example>
        public static IEnumerable<T2> Select<T1, T2>(this IEnumerable<T1> enumerable, Func<T1, T2> selector,
                                                     bool allowNull = true)
        {
            //foreach (var item in enumerable)
            //{
            //    var select = selector(item);
            //    if (allowNull || !Equals(select, default(T1)))
            //        yield return select;
            //}
            return Enumerable.Select(enumerable, selector).Where(select => allowNull || !Equals(select, default(T1)));
        }

        /// <summary>
        ///   Allows you to create Enumerable List of the Enum's Values.
        /// </summary>
        /// <typeparam name="T"> Enum Type to enumerate </typeparam>
        /// <returns> </returns>
        public static IEnumerable<T> ToEnumValues<T>(this IEnumerable<T> enumerable)
        {
            var enumType = typeof(T);
            // Can't use generic type constraints on value types,
            // so have to do check like this
            if (typeof(Enum) != enumType.BaseType) throw new ArgumentException("T must be of type System.Enum");
            var enumValArray = Enum.GetValues(enumType);
            var enumValList = new List<T>(enumValArray.Length);
            //enumValList.AddRange(from int val in enumValArray select (T) Enum.Parse(enumType, val.ToString(CultureInfo.InvariantCulture)));
            enumValList.AddRange(enumValArray
                                     .Cast<int>()
                                     .Select(val => (T) Enum.Parse(enumType, val.ToString(CultureInfo.InvariantCulture))));
            return enumValList;
        }

        /// <summary>
        ///   Allows you to create a enumerable String list of the items name in the Enum.
        /// </summary>
        /// <typeparam name="T"> Enum Type to enumerate </typeparam>
        /// <returns> </returns>
        public static IEnumerable<String> ToEnumNames<T>(this IEnumerable<T> enumerable)
        {
            var cls = typeof(T);
            var enumArrayList = cls.GetInterfaces();
            //return (from objType in enumArrayList where objType.IsEnum select objType.Name).ToList();
            return (Select(enumArrayList.Where(objType => objType.IsEnum), objType => objType.Name)).ToList();
        }

        /// <summary>
        ///   Creates an Array from an IEnumerable&lt;T&gt; using the specified transform function.
        /// </summary>
        /// <typeparam name="T1"> The source data type </typeparam>
        /// <typeparam name="T2"> The target data type </typeparam>
        /// <param name="enumerable"> The source data. </param>
        /// <param name="selector"> A transform function to apply to each element. </param>
        /// <returns> An Array of the target data type </returns>
        /// <example>
        ///   var integers = Enumerable.Range(1, 3);
        ///   var intStrings = values.ToArray(i => i.ToString());
        /// </example>
        /// <remarks>
        ///   This method is a shorthand for the frequently use pattern IEnumerable&lt;T&gt;.Select(Func).ToArray()
        /// </remarks>
        public static T2[] ToArray<T1, T2>(this IEnumerable<T1> enumerable, Func<T1, T2> selector)
        {
            return Select(enumerable, selector).ToArray();
        }

        /// <summary>
        ///   Creates a List&lt;T&gt; from an IEnumerable&lt;T&gt; using the specified transform function.
        /// </summary>
        /// <typeparam name="T1"> The source data type </typeparam>
        /// <typeparam name="T2"> The target data type </typeparam>
        /// <param name="enumerable"> The source data. </param>
        /// <param name="selector"> A transform function to apply to each element. </param>
        /// <returns> An IEnumerable &lt; T &gt; of the target data type </returns>
        /// <example>
        ///   var integers = Enumerable.Range(1, 3);
        ///   var intStrings = values.ToList(i => i.ToString());
        /// </example>
        /// <remarks>
        ///   This method is a shorthand for the frequently use pattern IEnumerable&lt;T&gt;.Select(Func).ToList()
        /// </remarks>
        public static List<T2> ToList<T1, T2>(this IEnumerable<T1> enumerable, Func<T1, T2> selector)
        {
            return enumerable.Select(selector).ToList();
        }

        /// <summary>
        ///   Converts all items of a list and returns them as enumerable.
        /// </summary>
        /// <typeparam name="T1"> The source data type </typeparam>
        /// <typeparam name="T2"> The target data type </typeparam>
        /// <param name="enumerable"> The source data. </param>
        /// <returns> The converted data </returns>
        /// <example>
        ///   var values = new[] { "1", "2", "3" };
        ///   values.ConvertList&lt;String, int&gt;().ForEach(Console.WriteLine);
        /// </example>
        public static IEnumerable<T2> ConvertTo<T1, T2>(this IEnumerable<T1> enumerable)
        {
            if (null == enumerable) throw new ArgumentNullException("enumerable");
            return Select(enumerable, value => value.ConvertTo<T2>());
        }

        //public static List<T2> CastAs<T1, T2>(this IEnumerable<T1> enumerable, Func<T1, T2> fn)
        //{
        //    if (null == enumerable) throw new ArgumentNullException("enumerable");
        //    List<T2> list2 = new List<T2>();
        //    foreach (T1 item1 in enumerable)
        //    {
        //        T2 item2 = fn(item1);
        //        list2.Add(item2);
        //    }
        //    return list2;
        //}

        public static IEnumerable<T2> CastAs<T1, T2>(this IEnumerable<T1> enumerable, Func<T1, T2> fn)
        {
            if (null == enumerable) throw new ArgumentNullException("enumerable");
            //foreach (T1 item1 in enumerable)
            //{
            //    T2 item2 = fn(item1);
            //    yield return item2;
            //}
            return Select(enumerable, fn);
        }

        // Convenience method on IEnumerable<T> to allow passing of a
        // Comparison<T> delegate to the OrderBy method.
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> enumerable, Comparison<T> comparison)
        {
            return enumerable.OrderBy((item) => item, comparison as IComparer<T>);
        }

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> enumerable, String orderExp)
        {
            orderExp += String.Empty;
            var parts = orderExp.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 0 && !String.IsNullOrWhiteSpace(parts[0]))
            {
                var descending = false;
                var property = parts[0];
                if (parts.Length > 1)
                    descending = parts[1].ToLower().Contains("esc");
                var prop = typeof(T).GetProperty(property);
                if (default(PropertyInfo) == prop)
                    throw new Exception("No property '" + property + "' in + " + typeof(T).Name + "'");
                return (descending)
                           ? enumerable.OrderByDescending(x => prop.GetValue(x, null))
                           : enumerable.OrderBy(x => prop.GetValue(x, null));
            }
            return enumerable;
        }

        /// <summary>
        ///   Apply an OrderBy rule that is based on a sort property.
        /// </summary>
        /// <typeparam name="T"> The type of the objects that are stored in the collection. </typeparam>
        /// <param name="enumerable"> The collection to sort. </param>
        /// <param name="sortProperty"> The property to sort on. </param>
        /// <returns> A sorter collection. </returns>
        public static IEnumerable<T> SortBy<T>(this IEnumerable<T> enumerable, String sortProperty)
        {
            if (null == enumerable) throw new ArgumentNullException("enumerable");
            //Get the collection type
            var typeDataSource = enumerable.GetType();
            // Determine the data type of the items in the data source at runtime.
            var typeDataItem = typeof(Object);
            if (typeDataSource.HasElementType)
                typeDataItem = typeDataSource.GetElementType();
            else if (typeDataSource.IsGenericType)
                typeDataItem = typeDataSource.GetGenericArguments()[0];
            // Create an instance of the GenericSorter class passing in the data item type.
            var typeSorter = typeof(GenericSorter<>).MakeGenericType(typeDataItem);
            var objSorter = Activator.CreateInstance(typeSorter);
            // Now I can call the "Sort" method passing in my runtime types.
            return
                typeSorter.GetMethod("Sort", new[] { typeDataSource, typeof(String) }).Invoke(objSorter,
                                                                                             new Object[]
                                                                                                 {
                                                                                                     enumerable,
                                                                                                     sortProperty
                                                                                                 }) as
                IEnumerable<T>;
        }

        /// <summary>
        ///   Apply paging to an IEnumerable.
        /// </summary>
        /// <typeparam name="T"> The type that the collection contains. </typeparam>
        /// <param name="enumerable"> The data source. </param>
        /// <param name="pageIndex"> The page index, starting at 0. </param>
        /// <param name="pageSize"> The max number of items to return. </param>
        /// <returns> The resulting object collection. </returns>
        public static IEnumerable<T> Page<T>(this IEnumerable<T> enumerable, int? pageIndex, int pageSize)
        {
            return (null != enumerable) ? enumerable.Skip((pageIndex ?? 0) * pageSize).Take(pageSize) : null;
        }

        public static IEnumerable<SelectListItem> ToSelectItemList<T>(this IEnumerable<T> enumerable,
                                                                      Func<T, String> text,
                                                                      Func<T, String> value)
        {
            return Select(enumerable,
                          item => new SelectListItem
                                      {
                                          Text = text(item),
                                          Value = value(item)
                                      });
        }

        #region Min-Max
        /// <summary>
        ///   Returns the minimum item based on a provided selector.
        /// </summary>
        /// <typeparam name="T"> The item type </typeparam>
        /// <typeparam name="TValue"> The value item </typeparam>
        /// <param name="enumerable"> The items. </param>
        /// <param name="selector"> The selector. </param>
        /// <param name="minValue"> The min value as output parameter. </param>
        /// <returns> The minimum item </returns>
        /// <example>
        ///   <code>int age;
        ///     var youngestPerson = persons.MinItem(p =&gt; p.Age, out age);</code>
        /// </example>
        public static T MinItem<T, TValue>(this IEnumerable<T> enumerable, Func<T, TValue> selector, out TValue minValue)
            where T : class
            where TValue : IComparable
        {
            T minItem = null;
            minValue = default(TValue);
            foreach (var item in enumerable)
            {
                if (null == item) continue;
                var itemValue = selector(item);
                if ((null != minItem) && (itemValue.CompareTo(minValue) >= 0)) continue;
                minValue = itemValue;
                minItem = item;
            }
            return minItem;
        }

        /// <summary>
        ///   Returns the minimum item based on a provided selector.
        /// </summary>
        /// <typeparam name="T"> The item type </typeparam>
        /// <typeparam name="TValue"> The value item </typeparam>
        /// <param name="enumerable"> The items. </param>
        /// <param name="selector"> The selector. </param>
        /// <returns> The minimum item </returns>
        /// <example>
        ///   <code>var youngestPerson = persons.MinItem(p =&gt; p.Age);</code>
        /// </example>
        public static T MinItem<T, TValue>(this IEnumerable<T> enumerable, Func<T, TValue> selector)
            where T : class
            where TValue : IComparable
        {
            TValue minValue;
            return MinItem(enumerable, selector, out minValue);
        }

        /// <summary>
        ///   Returns the maximum item based on a provided selector.
        /// </summary>
        /// <typeparam name="T"> The item type </typeparam>
        /// <typeparam name="TValue"> The value item </typeparam>
        /// <param name="enumerable"> The items. </param>
        /// <param name="selector"> The selector. </param>
        /// <param name="maxValue"> The max value as output parameter. </param>
        /// <returns> The maximum item </returns>
        /// <example>
        ///   <code>int age;
        ///     var oldestPerson = persons.MaxItem(p =&gt; p.Age, out age);</code>
        /// </example>
        public static T MaxItem<T, TValue>(this IEnumerable<T> enumerable, Func<T, TValue> selector, out TValue maxValue)
            where T : class
            where TValue : IComparable
        {
            T maxItem = null;
            maxValue = default(TValue);
            foreach (var item in enumerable)
            {
                if (null == item) continue;
                var itemValue = selector(item);
                if ((null != maxItem) && (itemValue.CompareTo(maxValue) <= 0)) continue;
                maxValue = itemValue;
                maxItem = item;
            }
            return maxItem;
        }

        /// <summary>
        ///   Returns the maximum item based on a provided selector.
        /// </summary>
        /// <typeparam name="T"> The item type </typeparam>
        /// <typeparam name="TValue"> The value item </typeparam>
        /// <param name="enumerable"> The items. </param>
        /// <param name="selector"> The selector. </param>
        /// <returns> The maximum item </returns>
        /// <example>
        ///   <code>var oldestPerson = persons.MaxItem(p =&gt; p.Age);</code>
        /// </example>
        public static T MaxItem<T, TValue>(this IEnumerable<T> enumerable, Func<T, TValue> selector)
            where T : class
            where TValue : IComparable
        {
            TValue maxValue;
            return MaxItem(enumerable, selector, out maxValue);
        }
        #endregion

        #region Sum
        /// <summary>
        ///   Computes the sum of a enumerable of UInt32 values.
        /// </summary>
        /// <param name="enumerable"> A enumerable of UInt32 values to calculate the sum of. </param>
        /// <returns> The sum of the values in the enumerable. </returns>
        public static uint Sum(this IEnumerable<uint> enumerable)
        {
            return enumerable.Aggregate(0U, (current, number) => current + number);
        }

        /// <summary>
        ///   Computes the sum of a enumerable of UInt64 values.
        /// </summary>
        /// <param name="enumerable"> A enumerable of UInt64 values to calculate the sum of. </param>
        /// <returns> The sum of the values in the enumerable. </returns>
        public static ulong Sum(this IEnumerable<ulong> enumerable)
        {
            return enumerable.Aggregate(0UL, (current, number) => current + number);
        }

        /// <summary>
        ///   Computes the sum of a enumerable of nullable UInt32 values.
        /// </summary>
        /// <param name="enumerable"> A enumerable of nullable UInt32 values to calculate the sum of. </param>
        /// <returns> The sum of the values in the enumerable. </returns>
        public static uint? Sum(this IEnumerable<uint?> enumerable)
        {
            return Where(enumerable, nullable => nullable.HasValue).Aggregate(0U,
                                                                              (current, nullable) =>
                                                                              current + nullable.GetValueOrDefault());
        }

        /// <summary>
        ///   Computes the sum of a enumerable of nullable UInt64 values.
        /// </summary>
        /// <param name="enumerable"> A enumerable of nullable UInt64 values to calculate the sum of. </param>
        /// <returns> The sum of the values in the enumerable. </returns>
        public static ulong? Sum(this IEnumerable<ulong?> enumerable)
        {
            return Where(enumerable, nullable => nullable.HasValue).Aggregate(0UL,
                                                                              (current, nullable) =>
                                                                              current + nullable.GetValueOrDefault());
        }

        /// <summary>
        ///   Computes the sum of a enumerable of UInt32 values that are obtained by invoking a transformation function on each element of the intput enumerable.
        /// </summary>
        /// <param name="enumerable"> A enumerable of values that are used to calculate a sum. </param>
        /// <param name="selection"> A transformation function to apply to each element. </param>
        /// <returns> The sum of the projected values. </returns>
        public static uint Sum<T>(this IEnumerable<T> enumerable, Func<T, uint> selection) where T : class
        {
            return WhereNotNull(enumerable).Select(selection).Sum();
        }

        /// <summary>
        ///   Computes the sum of a enumerable of nullable UInt32 values that are obtained by invoking a transformation function on each element of the intput enumerable.
        /// </summary>
        /// <param name="enumerable"> A enumerable of values that are used to calculate a sum. </param>
        /// <param name="selection"> A transformation function to apply to each element. </param>
        /// <returns> The sum of the projected values. </returns>
        public static uint? Sum<T>(this IEnumerable<T> enumerable, Func<T, uint?> selection) where T : class
        {
            return WhereNotNull(enumerable).Select(selection).Sum();
        }

        /// <summary>
        ///   Computes the sum of a enumerable of UInt64 values that are obtained by invoking a transformation function on each element of the intput enumerable.
        /// </summary>
        /// <param name="enumerable"> A enumerable of values that are used to calculate a sum. </param>
        /// <param name="selector"> A transformation function to apply to each element. </param>
        /// <returns> The sum of the projected values. </returns>
        public static ulong Sum<T>(this IEnumerable<T> enumerable, Func<T, ulong> selector) where T : class
        {
            return WhereNotNull(enumerable).Select(selector).Sum();
        }

        /// <summary>
        ///   Computes the sum of a enumerable of nullable UInt64 values that are obtained by invoking a transformation function on each element of the intput enumerable.
        /// </summary>
        /// <param name="enumerable"> A enumerable of values that are used to calculate a sum. </param>
        /// <param name="selector"> A transformation function to apply to each element. </param>
        /// <returns> The sum of the projected values. </returns>
        public static ulong? Sum<T>(this IEnumerable<T> enumerable, Func<T, ulong?> selector) where T : class
        {
            return WhereNotNull(enumerable).Select(selector).Sum();
        }
        #endregion

        #region Combinations
        public static IEnumerable<T[]> Combinations<T>(this IEnumerable<T> enumerable, int k)
        {
            var result = new List<T[]>();
            if (k == 0)
                // single combination: empty set
                result.Add(new T[0]);
            else
            {
                var index = 0;
                var list = enumerable as List<T> ?? enumerable.ToList();
                foreach (var element in list)
                // combine each element with (k - 1)-combinations of subsequent elements
                {
                    var elem = element;
                    result.AddRange(list
                                        .Skip(++index)
                                        .Combinations(k - 1)
                                        .Select(combination => (new[] { elem }).Concat(combination).ToArray()));
                }
            }
            return result;
        }

        public static IEnumerable<T[]> Combinations<T>(this IEnumerable<T> enumerable)
        {
            var result = new List<T[]>();
            var list = enumerable as List<T> ?? enumerable.ToList();
            for (var i = 0; i <= list.Count; i++)
                result.AddRange(list.Combinations(i));
            return result;
        }

        public static IEnumerable<T[]> CombinationsNotNull<T>(this IEnumerable<T> enumerable)
        {
            var result = new List<T[]>();
            var list = enumerable as List<T> ?? enumerable.ToList();
            for (var i = 1; i <= list.Count; i++)
                result.AddRange(list.Combinations(i));
            return result;
        }
        #endregion

        #region Chain Action
        // Allows Chaining => seq.Apply().Apply().Apply()
        public static IEnumerable<T> Apply<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (null == action) throw new ArgumentNullException("action");
            foreach (var item in enumerable)
            {
                action(item);
                yield return item;
            }
        }

        // Allow Completion => seq.Apply()....Done()
        public static void Done<T>(this IEnumerable<T> enumerable)
        {
            //return enumerable as List<T> ?? enumerable.ToList();
            if (null != enumerable) foreach (var item in enumerable) { }
        }
        #endregion

        #region Safe Function
        //null.ToList() returns empty list
        public static List<T> SafeToList<T>(this IEnumerable<T> enumerable)
        {
            return (enumerable ?? new List<T>()).ToList();
        }

        public static void SafeForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            SafeToList(enumerable).SafeForEach(action);
        }
        #endregion

        #region Numbers
        //public static int Sum(this IEnumerable<int> numbers)
        //{
        //    return numbers.Aggregate((x, y) => x + y);
        //}

        //public static int Min(this IEnumerable<int> numbers)
        //{
        //    return numbers.Aggregate((x, y) => Math.Min(x, y));
        //}

        //public static int Max(this IEnumerable<int> numbers)
        //{
        //    return numbers.Aggregate((x, y) => Math.Max(x, y));
        //} 
        #endregion
    }
}