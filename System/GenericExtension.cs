using System.Text;
using System.Xml.Serialization;

namespace System
{
    using Collections.Generic;
    using IO;
    using Runtime.Serialization.Formatters.Binary;

    public static class GenericExtension
    {
        #region Validate
        /// <summary>
        ///   Returns TRUE, if specified target reference is equals with null reference.
        ///   Othervise returns FALSE.
        /// </summary>
        /// <typeparam name="T"> Type of target. </typeparam>
        /// <param name="self"> Target reference. Can be null. </param>
        /// <remarks>
        ///   Some types has overloaded '==' and '!=' operators.
        ///   So the code "null == ((MyClass)null)" can returns <c>false</c>.
        ///   The most correct way how to test for null reference is using "System.Object.ReferenceEquals(Object, Object)" method.
        ///   However the notation with ReferenceEquals method is long and uncomfortable - this extension method solve it.
        /// </remarks>
        /// <example>
        ///   MyClass someObject = GetSomeObject();
        ///   if ( someObject.IsNull() ) { // the someObject is null // }
        ///   else { // the someObject is not null // }
        /// </example>
        public static bool IsNull<T>(this T self) where T : class
        {
            return ReferenceEquals(default(T), self);
        }

        /// <summary>
        ///   Returns TRUE, if specified target reference is equals with null reference.
        ///   Othervise returns FALSE.
        /// </summary>
        /// <typeparam name="T"> Type of target. </typeparam>
        /// <param name="self"> Target reference. Can be null. </param>
        /// <remarks>
        ///   Some types has overloaded '==' and '!=' operators.
        ///   So the code "null == ((MyClass)null)" can returns <c>false</c>.
        ///   The most correct way how to test for null reference is using "System.Object.ReferenceEquals(Object, Object)" method.
        ///   However the notation with ReferenceEquals method is long and uncomfortable - this extension method solve it.
        /// </remarks>
        /// <example>
        ///   MyClass someObject = GetSomeObject();
        ///   if ( someObject.IsNotNull() ) { // the someObject is not null // }
        ///   else { // the someObject is null // }
        /// </example>
        public static bool IsNotNull<T>(this T self) where T : class
        {
            return !IsNull(self);
        }
        #endregion

        #region Equals
        /// <summary>
        ///   Determines whether the Object is equal to any of the provided values.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="self"> The Object to be compared. </param>
        /// <param name="values"> The values to compare with the Object. </param>
        /// <returns> </returns>
        public static bool EqualsAny<T>(this T self, params T[] values)
        {
            return (Array.IndexOf(values, self) != -1);
        }

        /// <summary>
        ///   Determines whether the Object is equal to none of the provided values.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="self"> The Object to be compared. </param>
        /// <param name="values"> The values to compare with the Object. </param>
        /// <returns> </returns>
        public static bool EqualsNone<T>(this T self, params T[] values)
        {
            return !EqualsAny(self, values);
        }
        #endregion

        #region Convert To
        /// <summary>
        ///   Converts the specified value to a different type.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="self"> The value. </param>
        /// <returns> An universal converter suppliying additional target conversion methods </returns>
        /// <example>
        ///   <code>var value = "123";
        ///     var numeric = value.ConvertTo().ToInt32();</code>
        /// </example>
        public static IConverter<T> ConvertTo<T>(this T self)
        {
            return new Converter<T>(self);
        }
        #endregion

        /// <summary>
        ///   Gets the type default value for the underlying data type, in case of reference types: null
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="self"> The value. </param>
        /// <returns> The default value </returns>
        public static T GetTypeDefaultValue<T>(this T self)
        {
            return default(T);
        }

        /// <summary>
        ///   Converts the specified value to a database value and returns DBNull.Value if the value equals its default.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="self"> The value. </param>
        /// <returns> </returns>
        public static Object ToDatabaseValue<T>(this T self)
        {
            return self.Equals(self.GetTypeDefaultValue()) ? DBNull.Value : (Object) self;
        }

        #region NotNull
        /// <summary>
        ///   If target is null reference, returns notNullValue.
        ///   Othervise returns self.
        /// </summary>
        /// <typeparam name="T"> Type of self. </typeparam>
        /// <param name="self"> Self which is maybe null. Can be null. </param>
        /// <param name="notNullValue"> Value used instead of null. </param>
        /// <example>
        ///   const int DEFAULT_NUMBER = 123;
        /// 
        ///   int? number = null;
        ///   int notNullNumber1 = number.NotNull( DEFAULT_NUMBER ).Value; // returns 123
        /// 
        ///   number = 57;
        ///   int notNullNumber2 = number.NotNull( DEFAULT_NUMBER ).Value; // returns 57
        /// </example>
        public static T NotNull<T>(this T self, T notNullValue)
        {
            return ReferenceEquals(null, self) ? notNullValue : self;
        }

        /// <summary>
        ///   If target is null reference, returns result from notNullValueProvider.
        ///   Othervise returns target.
        /// </summary>
        /// <typeparam name="T"> Type of target. </typeparam>
        /// <param name="self"> Target which is maybe null. Can be null. </param>
        /// <param name="notNullValueProvider"> Delegate which return value is used instead of null. </param>
        /// <example>
        ///   int? number = null;
        ///   int notNullNumber1 = number.NotNull( ()=> GetRandomNumber(10, 20) ).Value; // returns random number from 10 to 20
        /// 
        ///   number = 57;
        ///   int notNullNumber2 = number.NotNull( ()=> GetRandomNumber(10, 20) ).Value; // returns 57
        /// </example>
        public static T NotNull<T>(this T self, Func<T> notNullValueProvider)
        {
            return ReferenceEquals(null, self) ? notNullValueProvider() : self;
        }
        #endregion

        /// <summary>
        ///   Counts and returns the recursive execution of the passed function until it returns null.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="self"> The item to start peforming on. </param>
        /// <param name="function"> The function to be executed. </param>
        /// <returns> The number of executions until the function returned null </returns>
        public static int CountLoopsToNull<T>(this T self, Func<T, T> function) where T : class
        {
            var num = 0;
            while (default(T) != (self = function(self)))
                ++num;
            return num;
        }

        /// <summary>
        ///   Finds a type instance using a recursive call. The method is useful to find specific parents for example.
        /// </summary>
        /// <typeparam name="T"> The source type to perform on. </typeparam>
        /// <typeparam name="K"> The targte type to be returned </typeparam>
        /// <param name="self"> The item to start performing on. </param>
        /// <param name="function"> The function to be executed. </param>
        /// <returns> An target type instance or null. </returns>
        /// <example>
        ///   <code>var tree = ...
        ///     var node = tree.FindNodeByValue("");
        ///     var parentByType = node.FindTypeByRecursion%lt;TheType&gt;(n => n.Parent);</code>
        /// </example>
        public static K FindTypeByRecursion<T, K>(this T self, Func<T, T> function)
            where T : class
            where K : class, T
        {
            do
            {
                if (self is K) return (K) self;
            } while (default(T) != (self = function(self)));
            return default(K);
        }

        /// <summary>
        ///   Perform a deep Copy of the Object.
        /// </summary>
        /// <typeparam name="T"> The type of Object being copied. </typeparam>
        /// <param name="self"> The Object instance to copy. </param>
        /// <returns> The copied Object. </returns>
        public static T Clone<T>(this T self)
        {
            if (!typeof(T).IsSerializable)
                throw new ArgumentException("The type must be serializable.", "self");
            // Don't serialize a null Object, simply return the default for that Object
            if (ReferenceEquals(self, null)) return default(T);
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, self);
                stream.Seek(0, SeekOrigin.Begin);
                return (T) formatter.Deserialize(stream);
            }
        }

        /// <summary>
        ///   A generic version of System.String.Join()
        /// </summary>
        /// <typeparam name="T"> The type of the array to join </typeparam>
        /// <param name="separator"> The separator to appear between each element </param>
        /// <param name="value"> An array of values </param>
        /// <returns> The join. </returns>
        public static String Join<T>(this T[] value, String separator = null)
        {
            if (value.IsNullOrEmpty()) return String.Empty;
            if (separator.IsNull()) separator = String.Empty;
            var converter = new Converter<T, string>((obj) => obj.ToString());
            return String.Join(separator, Array.ConvertAll(value, converter));
        }

        public static T[] CopySlice<T>(this T[] source, int index, int length, bool padToLength = false)
        {
            var n = length;
            T[] slice = null;
            if (source.Length < index + length)
            {
                n = source.Length - index;
                if (padToLength)
                    slice = new T[length];
            }
            if (null == slice) slice = new T[n];
            Array.Copy(source, index, slice, 0, n);
            return slice;
        }

        public static IEnumerable<T[]> Slices<T>(this T[] source, int count, bool padToLength = false)
        {
            for (var i = 0; i < source.Length; i += count)
                yield return source.CopySlice(i, count, padToLength);
        }



        #region Serialize Deserialize

        public static Byte[] Serialize<T>(this T self) where T : class
        {
            using (var memStream = new MemoryStream())
            {
                (new BinaryFormatter()).Serialize(memStream, self);
                return memStream.ToArray();
            }
        }

        public static T Deserialize<T>(this T self, Byte[] buffer) where T : class
        {
            using (var memStream = new MemoryStream(buffer))
                return (new BinaryFormatter()).Deserialize(memStream) as T;
        }

        public static T SerializeClone<T>(this T self) where T : class
        {
            using (var memStream = new MemoryStream())
            {
                var binFormatter = new BinaryFormatter();
                binFormatter.Serialize(memStream, self);
                memStream.Seek(0, SeekOrigin.Begin);
                return binFormatter.Deserialize(memStream) as T;
            }
        }

        public static String XmlSerializeToString<T>(this T self)
        {
            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                (new XmlSerializer(self.GetType())).Serialize(writer, self);
            }
            return sb.ToString();
        }

        #endregion
    }
}