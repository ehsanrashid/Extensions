namespace System
{
    /// <summary>
    ///   Extension methods for the array data type
    /// </summary>
    public static class ArrayExtension
    {
        ///<summary>
        ///  Check if the array is null or empty
        ///</summary>
        ///<param name="array"> </param>
        ///<returns> </returns>
        public static bool IsNullOrEmpty(this Array array)
        {
            return default(Array) != array && array.Length <= 0;
        }

        /// <summary>
        ///   Tests if the array is empty.
        /// </summary>
        /// <param name="array"> The array to test. </param>
        /// <returns> True if the array is empty. </returns>
        public static bool IsEmpty(this Array array)
        {
            array.ExceptionIfNull("The array cannot be null.", "array");
            return (array.Length <= 0);
        }

        ///<summary>
        ///  Check if the index is within the array
        ///</summary>
        ///<param name="array"> </param>
        ///<param name="index"> </param>
        ///<param name="dimension"> </param>
        ///<returns> </returns>
        public static bool WithinIndex(this Array array, int index, int dimension)
        {
            return default(Array) != array &&
                   (array.GetLowerBound(dimension) <= index && index <= array.GetUpperBound(dimension));
        }

        ///<summary>
        ///  Check if the index is within the array
        ///</summary>
        ///<param name="array"> </param>
        ///<param name="index"> </param>
        ///<returns> </returns>
        public static bool WithinIndex(this Array array, int index)
        {
            return default(Array) != array && (0 <= index && index < array.Length);
        }

        /// <summary>
        ///   To clear the contents of the array.
        /// </summary>
        /// <param name="array"> The array to clear </param>
        /// <returns> Cleared array </returns>
        /// <example>
        ///   <code>Array array = Array.CreateInstance(typeof(string), 2);
        ///     array.SetValue("One", 0); array.SetValue("Two", 1);
        ///     Array arrayToClear = array.ClearAll();</code>
        /// </example>
        public static Array ClearAll(this Array array)
        {
            if (default(Array) != array)
                Array.Clear(array, 0, array.Length);
            return array;
        }

        /// <summary>
        ///   To clear a specific item in the array.
        /// </summary>
        /// <param name="array"> The array in where to clean the item. </param>
        /// <param name="at"> Which element to clear. </param>
        /// <returns> </returns>
        /// <example>
        ///   <code>Array array = Array.CreateInstance(typeof(string), 2);
        ///     array.SetValue("One", 0); array.SetValue("Two", 1);
        ///     Array result = array.ClearAt(2);</code>
        /// </example>
        public static Array ClearAt(this Array array, int at)
        {
            if (default(Array) != array)
            {
                var arrIndex = at.GetArrayIndex();
                if (arrIndex.IsIndexInArray(array))
                    Array.Clear(array, arrIndex, 1);
            }
            return array;
        }

        // ----------------------------------------------------

        /// <summary>
        ///   To clear the contents of the array.
        /// </summary>
        /// <typeparam name="T"> The type of array </typeparam>
        /// <param name="array"> The array to clear </param>
        /// <returns> Cleared array </returns>
        /// <example>
        ///   <code>int[] result = new[] { 1, 2, 3, 4 }.ClearAll
        ///     <int />
        ///     ();</code>
        /// </example>
        public static T[] ClearAll<T>(this T[] array)
        {
            if (default(T[]) != array)
                for (var i = array.GetLowerBound(0); i <= array.GetUpperBound(0); ++i)
                    array[i] = default(T);
            return array;
        }

        /// <summary>
        ///   To clear a specific item in the array.
        /// </summary>
        /// <typeparam name="T"> The type of array </typeparam>
        /// <param name="array"> Array </param>
        /// <param name="at"> Which element to clear. </param>
        /// <returns> </returns>
        /// <example>
        ///   <code>string[] clearString = new[] { "A" }.ClearAt
        ///     <string />
        ///     (0);</code>
        /// </example>
        public static T[] ClearAt<T>(this T[] array, int at)
        {
            if (default(T[]) != array)
            {
                var arrIndex = at.GetArrayIndex();
                if (arrIndex.IsIndexInArray(array))
                    array[arrIndex] = default(T);
            }
            return array;
        }

        /// <summary>
        ///   Combine two arrays into one.
        /// </summary>
        /// <typeparam name="T"> Type of Array </typeparam>
        /// <param name="array1"> Base array in which arrayToCombine will add. </param>
        /// <param name="array2"> Array to combine with Base array. </param>
        /// <returns> </returns>
        /// <example>
        ///   <code>int[] arrayOne = new[] { 1, 2, 3, 4 };
        ///     int[] arrayTwo = new[] { 5, 6, 7, 8 };
        ///     Array combinedArray = arrayOne.CombineArray
        ///     <int />
        ///     (arrayTwo);</code>
        /// </example>
        public static T[] CombineArray<T>(this T[] array1, T[] array2)
        {
            if (default(T[]) != array1 && default(T[]) != array2)
            {
                var initialSize = array1.Length;
                Array.Resize(ref array1, initialSize + array2.Length);
                Array.Copy(array2, array2.GetLowerBound(0), array1, initialSize, array2.Length);
            }
            return array1;
        }

        // ------------------------------------------------------------

        public static T[] Slice<T>(this T[] array, int index, int count)
        {
            if (index < 0 || count < 0 || (array.Length - index) < count)
                throw new ArgumentException();
            var result = new T[count];
            Array.Copy(array, index, result, 0, count);
            return result;
        }

        public static String Join<T>(this T[][] array2D)
        {
            return String.Join(Environment.NewLine,
                               Array.ConvertAll(array2D,
// ReSharper disable RedundantLambdaSignatureParentheses
                                                (array1D) => String.Join(",",
// ReSharper restore RedundantLambdaSignatureParentheses
                                                                         Array.ConvertAll(array1D,
// ReSharper disable RedundantLambdaSignatureParentheses
                                                                                          (item) => item.ToString()))));
// ReSharper restore RedundantLambdaSignatureParentheses
        }
    }
}