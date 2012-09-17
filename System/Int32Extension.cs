namespace System
{
    using Globalization;

    /// <summary>
    /// 	Extension methods for the integer data type
    /// </summary>
    public static class Int32Extension
    {
        #region Times

        /// <summary>
        /// 	Performs the specified action n times based on the underlying Int32 value.
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <param name = "action">The action.</param>
        public static void Times(this Int32 value, Action action)
        {
            //for (Int32 i = 0; i < value; ++i)
            //    action();
            value.AsInt64().Times(action);
        }

        /// <summary>
        /// 	Performs the specified action n times based on the underlying Int32 value.
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <param name = "action">The action.</param>
        public static void Times(this Int32 value, Action<Int32> action)
        {
            for (var i = 0; i < value; ++i)
                action(i);
            // NOTE: Is it possible to reuse LongExtensions for this call?
            // --value.AsInt64().Times(action as Action<Int64>);
        }
        #endregion

        #region Validate

        /// <summary>
        /// 	Determines whether the value is even
        /// </summary>
        /// <param name = "value">The Value</param>
        /// <returns>true or false</returns>
        public static bool IsEven(this Int32 value)
        {
            return value.AsInt64().IsEven();
        }

        /// <summary>
        /// 	Determines whether the value is odd
        /// </summary>
        /// <param name = "value">The Value</param>
        /// <returns>true or false</returns>
        public static bool IsOdd(this Int32 value)
        {
            return value.AsInt64().IsOdd();
        }


        /// <summary>Checks whether the value is in range or returns the default value</summary>
        /// <param name="value">The Value</param>
        /// <param name="minValue">The minimum value</param>
        /// <param name="maxValue">The maximum value</param>
        /// <param name="defaultValue">The default value</param>
        public static Int32 InRange(this Int32 value, Int32 minValue, Int32 maxValue, Int32 defaultValue)
        {
            return (Int32) value.AsInt64().InRange(minValue, maxValue, defaultValue);
        }

        /// <summary>Checks whether the value is in range</summary>
        /// <param name="value">The Value</param>
        /// <param name="minValue">The minimum value</param>
        /// <param name="maxValue">The maximum value</param>
        public static bool InRange(this Int32 value, Int32 minValue, Int32 maxValue)
        {
            return value.AsInt64().InRange(minValue, maxValue);
        }

        /// <summary>
        /// A prime number (or a prime) is a natural number that has exactly two distinct natural number divisors: 1 and itself.
        /// </summary>
        /// <param name="value">Object value</param>
        /// <returns>Returns true if the value is a prime number.</returns>
        public static bool IsPrime(this Int32 value)
        {
            return value.AsInt64().IsPrime();
        }

        #endregion

        #region To Value

        public static Int32 ToMinusOne(this Int32? value)
        {
            return value.HasValue ? value.Value : -1;
        }

        public static Int32 ToZero(this Int32? value)
        {
            return value.HasValue ? value.Value : 0;
        }

        public static String ToStringEmpty(this Int32? value)
        {
            return value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : String.Empty;
        }

        public static String ToStringZero(this Int32? value)
        {
            return value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : "0";
        }

        public static DateTime? ToYearBegin(this Int32? value)
        {
            return value.HasValue ? new DateTime(value.Value, 1, 1) : default(DateTime);
        }

        public static DateTime? ToYearEnd(this Int32? value)
        {
            return value.HasValue ? new DateTime(value.Value, 12, 31, 23, 59, 59) : default(DateTime);
        }

        #region To Ordinal
        /// <summary>
        /// Converts the value to ordinal String. (English)
        /// </summary>
        /// <param name="value">Object value</param>
        /// <returns>Returns String containing ordinal indicator adjacent to a numeral denoting. (English)</returns>
        public static String ToOrdinal(this Int32 value)
        {
            return value.AsInt64().ToOrdinal();
        }

        /// <summary>
        /// Converts the value to ordinal String with specified format. (English)
        /// </summary>
        /// <param name="value">Object value</param>
        /// <param name="format">A standard or custom format String that is supported by the object to be formatted.</param>
        /// <returns>Returns String containing ordinal indicator adjacent to a numeral denoting. (English)</returns>
        public static String ToOrdinal(this Int32 value, String format)
        {
            return value.AsInt64().ToOrdinal(format);
        }
        
        #endregion

        #endregion

        #region TimeSpan

        /// <summary>
        /// Gets a TimeSpan from an integer number of days.
        /// </summary>
        /// <param name="days">The number of days the TimeSpan will contain.</param>
        /// <returns>A TimeSpan containing the specified number of days.</returns>
        public static TimeSpan Days(this Int32 days)
        {
            return TimeSpan.FromDays(days);
        }

        /// <summary>
        /// Gets a TimeSpan from an integer number of hours.
        /// </summary>
        /// <param name="hours">The number of hours the TimeSpan will contain.</param>
        /// <returns>A TimeSpan containing the specified number of hours.</returns>
        /// <remarks>
        ///		Contributed by jceddy
        /// </remarks>
        public static TimeSpan Hours(this Int32 hours)
        {
            return TimeSpan.FromHours(hours);
        }

        /// <summary>
        /// Gets a TimeSpan from an integer number of minutes.
        /// </summary>
        /// <param name="minutes">The number of minutes the TimeSpan will contain.</param>
        /// <returns>A TimeSpan containing the specified number of minutes.</returns>
        public static TimeSpan Minutes(this Int32 minutes)
        {
            return TimeSpan.FromMinutes(minutes);
        }

        /// <summary>
        /// Gets a TimeSpan from an integer number of seconds.
        /// </summary>
        /// <param name="seconds">The number of seconds the TimeSpan will contain.</param>
        /// <returns>A TimeSpan containing the specified number of seconds.</returns>
        public static TimeSpan Seconds(this Int32 seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        /// <summary>
        /// Gets a TimeSpan from an integer number of milliseconds.
        /// </summary>
        /// <param name="milliseconds">The number of milliseconds the TimeSpan will contain.</param>
        /// <returns>A TimeSpan containing the specified number of milliseconds.</returns>
        public static TimeSpan Milliseconds(this Int32 milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds);
        }

        /// <summary>
        /// Gets a TimeSpan from an integer number of ticks.
        /// </summary>
        /// <param name="ticks">The number of ticks the TimeSpan will contain.</param>
        /// <returns>A TimeSpan containing the specified number of ticks.</returns>
        public static TimeSpan Ticks(this Int32 ticks)
        {
            return TimeSpan.FromTicks(ticks);
        }

        #endregion

        
        /// <summary>
        /// Returns the integer as Int64.
        /// </summary>
        public static Int64 AsInt64(this Int32 value)
        {
            return value;
        }

        /// <summary>
        /// To check whether an index is in the range of the given array.
        /// </summary>
        /// <param name="index">Index to check</param>
        /// <param name="array">Array where to check</param>
        /// <returns></returns>
        public static bool IsIndexInArray(this Int32 index, Array array)
        {
            return index.GetArrayIndex().InRange(array.GetLowerBound(0), array.GetUpperBound(0));
        }

        /// <summary>
        /// To get Array index from a given based on a number.
        /// </summary>
        /// <param name="at">Real world postion </param>
        /// <returns></returns>
        public static Int32 GetArrayIndex(this Int32 at)
        {
            return (at == 0) ? 0 : at - 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int64 Factorial(this Int32 value)
        {
              return value.AsInt64().Factorial();
        }


    }
}