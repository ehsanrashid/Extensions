namespace System.Collections.Generic
{
    using Globalization;
    using Linq;
    using Linq.Expressions;
    using Text;
    using Text.RegularExpressions;

    public static class ListExtension
    {
        /// <summary>
        ///   Tests if the collection is empty.
        /// </summary>
        /// <param name="list"> The collection to test. </param>
        /// <returns> True if the collection is empty. </returns>
        public static bool IsEmpty(this IList list)
        {
            list.ExceptionIfNull("The list cannot be null.", "list");
            return (list.Count == 0);
        }

        /// <summary>
        ///   Tests if the collection is empty.
        /// </summary>
        /// <typeparam name="T"> The type of the items in the collection. </typeparam>
        /// <param name="list"> The collection to test. </param>
        /// <returns> True if the collection is empty. </returns>
        public static bool IsEmpty<T>(this IList<T> list)
        {
            list.ExceptionIfNull("The list cannot be null.", "list");
            return (list.Count == 0);
        }

        ///<summary>
        ///  Cast this list into a List
        ///</summary>
        ///<param name="list"> </param>
        ///<typeparam name="T"> </typeparam>
        ///<returns> </returns>
        public static List<T> Cast<T>(this IList list)
        {
            var listCast = new List<T>();
            listCast.AddRange(list.OfType<T>());
            return listCast;
        }

        /// <summary>
        ///   Fast version of the RemoveAt function. Overwrites the element at the specified index
        ///   with the last element in the list, then removes the last element, thus lowering the
        ///   inherent O(n) cost to O(1). Intended to be used on *unordered* lists only.
        /// </summary>
        /// <param name="list"> IList </param>
        /// <param name="index"> Index of the element to be removed. </param>
        public static void RemoveAtFast<T>(this IList<T> list, int index)
        {
            if (index < 0) return;
            //get the amount of items in the list once
            var count = list.Count - 1;
            if (index > count) return;
            //copy the last item to the index being removed
            list[index] = list[count];
            //still calling remove at because the old item was copied to the removed index
            //and we need the list to reflect the remove operation
            list.RemoveAt(count);
            //this will remove the last item which will not allow array.copy to be called resulting in a faster removal
            //array.copy is not called  because the element being removed it at the end of the array.
        }

        /// <summary>
        ///   Performs the specified action on each element in the list while providing the index to the action
        /// </summary>
        /// <typeparam name="T"> The Type of Elements in the IList </typeparam>
        /// <param name="list"> The IList to perform the Action on </param>
        /// <param name="action"> The Action to perform on the element of the IList </param>
        public static void ForEach<T>(this IList<T> list, Action<T> action)
        {
            for (int index = 0, end = list.Count; index < end; ++index)
                action(list[index]);
        }

        /// <summary>
        ///   Performs the specified action on each element in the list while providing the index to the action
        /// </summary>
        /// <typeparam name="T"> The Type of Elements in the IList </typeparam>
        /// <param name="list"> The IList to perform the Action on </param>
        /// <param name="action"> The Action to perform on the element of the IList </param>
        public static void ForEachWithIndex<T>(this IList<T> list, Action<T, int> action)
        {
            for (int index = 0, end = list.Count; index < end; ++index)
                action(list[index], index);
        }

        /// <summary>
        ///   Performs the specified action on each element in the list while providing the index and the origional list to the action
        /// </summary>
        /// <typeparam name="T"> The Type of Elements in the IList </typeparam>
        /// <param name="list"> The IList to perform the Action on </param>
        /// <param name="action"> The Action to perform on the element of the IList </param>
        public static void ForEachWithIndex<T>(this IList<T> list, Action<IList<T>, T, int> action)
        {
            for (int index = 0, end = list.Count; index < end; ++index)
                action(list, list[index], index);
        }

        [Obsolete("Wrong orthography.  Use InsertUnique")]
        public static bool InsertUnqiue<T>(this IList<T> list, int index, T item)
        {
            return list.InsertUnique(index, item);
        }

        /// <summary>
        ///   Inserts an item uniquely to to a list and returns a value whether the item was inserted or not.
        /// </summary>
        /// <typeparam name="T"> The generic list item type. </typeparam>
        /// <param name="list"> The list to be inserted into. </param>
        /// <param name="index"> The index to insert the item at. </param>
        /// <param name="item"> The item to be added. </param>
        /// <returns> Indicates whether the item was inserted or not </returns>
        public static bool InsertUnique<T>(this IList<T> list, int index, T item)
        {
            if (list.Contains(item) == false)
            {
                list.Insert(index, item);
                return true;
            }
            return false;
        }

        /// <summary>
        ///   Inserts a range of items uniquely to a list starting at a given index and returns the amount of items inserted.
        /// </summary>
        /// <typeparam name="T"> The generic list item type. </typeparam>
        /// <param name="list"> The list to be inserted into. </param>
        /// <param name="startIndex"> The start index. </param>
        /// <param name="items"> The items to be inserted. </param>
        /// <returns> The amount if items that were inserted. </returns>
        public static int InsertRangeUnique<T>(this IList<T> list, int startIndex, IEnumerable<T> items)
        {
            var index = startIndex + items.Reverse().Count(item => list.InsertUnique(startIndex, item));
            return (index - startIndex);
        }

        /// <summary>
        ///   Return the index of the first matching item or -1.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="list"> The list. </param>
        /// <param name="comparison"> The comparison. </param>
        /// <returns> The item index </returns>
        public static int IndexOf<T>(this IList<T> list, Func<T, bool> comparison)
        {
            for (var i = 0; i < list.Count; i++)
                if (comparison(list[i]))
                    return i;
            return -1;
        }

        /// <summary>
        ///   Join all the elements in the list and create a string seperated by the specified char.
        /// </summary>
        /// <param name="list"> The list. </param>
        /// <param name="joinChar"> The join char. </param>
        /// <typeparam name="T"> </typeparam>
        /// <returns> The resulting string of the elements in the list. </returns>
        /// <remarks>
        ///   Contributed by Michael T, http://about.me/MichaelTran
        /// </remarks>
        public static string Join<T>(this IList<T> list, char joinChar)
        {
            return list.Join(joinChar.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///   Join all the elements in the list and create a string seperated by the specified string.
        /// </summary>
        /// <param name="list"> The list. </param>
        /// <param name="joinString"> The join string. </param>
        /// <typeparam name="T"> </typeparam>
        /// <returns> The resulting string of the elements in the list. </returns>
        /// <remarks>
        ///   Contributed by Michael T, http://about.me/MichaelTran
        ///   Optimised by Mario Majcica
        /// </remarks>
        public static string Join<T>(this IList<T> list, string joinString)
        {
            if (default(IList<T>) == list || !list.Any())
                return String.Empty;
            var result = new StringBuilder();
            var listCount = list.Count;
            var listCountMinusOne = listCount - 1;
            if (listCount > 1)
                for (var i = 0; i < listCount; i++)
                    if (i != listCountMinusOne)
                    {
                        result.Append(list[i]);
                        result.Append(joinString);
                    }
                    else
                        result.Append(list[i]);
            else
                result.Append(list[0]);
            return result.ToString();
        }

        /// <summary>
        ///   Using Relugar Expression, find the top matches for each item in the source specified by the arguments to search.
        /// </summary>
        /// <param name="list"> The source. </param>
        /// <param name="searchString"> The search string. </param>
        /// <param name="top"> The top. </param>
        /// <param name="args"> The args. </param>
        /// <typeparam name="T"> </typeparam>
        /// <returns> A List of top matches. </returns>
        /// <remarks>
        ///   Contributed by Michael T, http://about.me/MichaelTran
        /// </remarks>
        public static List<T> Match<T>(this IList<T> list, String searchString, int top,
                                       params Expression<Func<T, Object>>[] args)
        {
            // Create a new list of results and matches;
            var results = new List<T>();
            var matches = new Dictionary<T, int>();
            var maxMatch = 0;
            // For each item in the source
            list.ForEach(
                (item) =>
                    {
                        // Generate the expression string from the argument.
                        var regExp = String.Empty;
                        if (args != null)
                            // For each argument
                            Array.ForEach(args,
                                          (expression) =>
                                              {
                                                  // Compile the expression
                                                  var property = expression.Compile();
                                                  // Attach the new property to the expression string
                                                  regExp += (string.IsNullOrEmpty(regExp) ? "(?:" : "|(?:") +
                                                            property(item) + ")+?";
                                              });
                        // Get the matches
                        var match = Regex.Matches(searchString, regExp, RegexOptions.IgnoreCase);
                        // If there are more than one match
                        if (match.Count > 0)
                            // Add it to the match dictionary, including the match count.
                            matches.Add(item, match.Count);
                        // Get the highest max matching
                        maxMatch = (match.Count > maxMatch) ? match.Count : maxMatch;
                    });
            // Convert the match dictionary into a list
            var matchList = matches.ToList();
            // Sort the list by decending match counts
            // matchList.Sort((s1, s2) => s2.Value.CompareTo(s1.Value));
            // Remove all matches that is less than the best match.
            matchList.RemoveAll(s => s.Value < maxMatch);
            // If the top value is set and is less than the number of matches
            var getTop = top > 0 && top < matchList.Count ? top : matchList.Count;
            // Add the maches into the result list.
            for (var i = 0; i < getTop; ++i)
                results.Add(matchList[i].Key);
            return results;
        }

        #region Sort
        //  Sorts an IList<T> in place.
        public static void Sort<T>(this IList<T> list, Comparison<T> comparison)
        {
            ArrayList.Adapter((IList) list).Sort(comparison as IComparer);
        }

        ///// <summary>         
        ///// Sort a collection based on a sort property.         
        ///// </summary>         
        ///// <typeparam name="T">The type of the objects that are stored in the collection.</typeparam>         /// <param name="list">The list to sort.</param>         /// <param name="sortProperty">The property to sort on.</param>         /// <returns>A sorter collection.</returns>
        //public static void Sort<T>(this List<T> list, String sortProperty)
        //{
        //    IEnumerable<T> sorted = list.OrderBy(sortProperty).ToList();
        //    list.Clear();
        //    list.AddRange(sorted);
        //}
        #endregion

        #region GetRandomItem
        /// <summary>
        ///   Get's an random item from list.
        /// </summary>
        /// <typeparam name="T"> Type of list item. </typeparam>
        /// <param name="list"> The List. </param>
        /// <param name="random"> Random instance to get random index. </param>
        /// <returns> A random item from list. </returns>
        public static T GetRandomItem<T>(this IList<T> list, Random random)
        {
            if (list.Count > 0)
                // The maxValue for the upper-bound in the Next() method is exclusive, see: http://stackoverflow.com/q/5063269/375958
                return list[random.Next(0, list.Count)];
            else
                throw new InvalidOperationException("Could not get item from empty list.");
        }

        /// <summary>
        ///   Get's an random item from list.
        /// </summary>
        /// <typeparam name="T"> Type of list item. </typeparam>
        /// <param name="list"> The List. </param>
        /// <param name="seed"> MSDN: A number used to calculate a starting value for the pseudo-random number sequence. If a negative number is specified, the absolute value of the number is used.. </param>
        /// <returns> A random item from list. </returns>
        public static T GetRandomItem<T>(this IList<T> list, int seed)
        {
            var random = new Random(seed);
            return list.GetRandomItem(random);
        }

        /// <summary>
        ///   Get's an random item from list.
        /// </summary>
        /// <typeparam name="T"> Type of list item. </typeparam>
        /// <param name="list"> The List. </param>
        /// <returns> A random item from list. </returns>
        public static T GetRandomItem<T>(this IList<T> list)
        {
            var random = new Random(DateTime.Now.Millisecond);
            return list.GetRandomItem(random);
        }
        #endregion

        #region Merge
        /// <summary>
        ///   The merge.
        /// </summary>
        /// <param name="lists"> The lists. </param>
        /// <typeparam name="T"> </typeparam>
        /// <returns> </returns>
        /// <remarks>
        ///   Contributed by Michael T, http://about.me/MichaelTran
        /// </remarks>
        public static List<T> Merge<T>(params List<T>[] lists)
        {
            var merged = new List<T>();
            foreach (var list in lists) merged.Merge(list);
            return merged;
        }

        /// <summary>
        ///   The merge.
        /// </summary>
        /// <param name="match"> The match. </param>
        /// <param name="lists"> The lists. </param>
        /// <typeparam name="T"> </typeparam>
        /// <returns> </returns>
        /// <remarks>
        ///   Contributed by Michael T, http://about.me/MichaelTran
        /// </remarks>
        public static List<T> Merge<T>(Expression<Func<T, object>> match, params List<T>[] lists)
        {
            var merged = new List<T>();
            foreach (var list in lists) merged.Merge(list, match);
            return merged;
        }

        /// <summary>
        ///   The merge.
        /// </summary>
        /// <param name="list1"> The list 1. </param>
        /// <param name="list2"> The list 2. </param>
        /// <param name="match"> The match. </param>
        /// <typeparam name="T"> </typeparam>
        /// <returns> </returns>
        /// <remarks>
        ///   Contributed by Michael T, http://about.me/MichaelTran
        /// </remarks>
        public static List<T> Merge<T>(this List<T> list1, List<T> list2, Expression<Func<T, object>> match)
        {
            if (list1 != null && list2 != null && match != null)
            {
                var matchFunc = match.Compile();
                foreach (var item in list2)
                {
                    var key = matchFunc(item);
                    if (!list1.Exists(i => matchFunc(i).Equals(key))) list1.Add(item);
                }
            }
            return list1;
        }

        /// <summary>
        ///   The merge.
        /// </summary>
        /// <param name="list1"> The list 1. </param>
        /// <param name="list2"> The list 2. </param>
        /// <typeparam name="T"> </typeparam>
        /// <returns> </returns>
        /// <remarks>
        ///   Contributed by Michael T, http://about.me/MichaelTran
        /// </remarks>
        public static List<T> Merge<T>(this List<T> list1, List<T> list2)
        {
            if (list1 != null && list2 != null)
                foreach (var item in list2.Where(item => !list1.Contains(item))) list1.Add(item);
            return list1;
        }
        #endregion

        #region Safe
        //x.SafeForEach(y) is a no-op for null x or null y
        //This is a shortcut that should probably go in a class called SafeListExtensions later
        public static void SafeForEach<T>(this List<T> list, Action<T> action)
        {
            action = action ?? (t => { });
            list = list ?? new List<T>();
            list.ForEach(action);
        }
        #endregion
    }
}