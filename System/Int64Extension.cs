namespace System
{
    using Globalization;

    /// <summary>
    /// 	Extension methods for the Long data type
    /// </summary>
    public static class Int64Extension
    {
        #region Times

        /// <summary>
        /// 	Performs the specified action n times based on the underlying Int64 value.
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <param name = "action">The action.</param>
        public static void Times(this Int64 value, Action action)
        {
            if (value <= 0) return;
            //for (Int64 i = 0; i < value; ++i)
            //    action();
            while (value-- > 0) action();
        }

        /// <summary>
        /// 	Performs the specified action n times based on the underlying Int64 value.
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <param name = "action">The action.</param>
        public static void Times(this Int64 value, Action<Int64> action)
        {
            if (value <= 0) return;
            for (Int64 i = 0; i < value; ++i)
                action(i);
        }

        #endregion

        #region Validate

        /// <summary>
        /// 	Determines whether the value is even
        /// </summary>
        /// <param name = "value">The Value</param>
        /// <returns>true or false</returns>
        public static bool IsEven(this Int64 value)
        {
            return (value % 2 == 0);
        }

        /// <summary>
        /// 	Determines whether the value is odd
        /// </summary>
        /// <param name = "value">The Value</param>
        /// <returns>true or false</returns>
        public static bool IsOdd(this Int64 value)
        {
            return (value % 2 != 0);
        }

        /// <summary>Checks whether the value is in range or returns the default value</summary>
        /// <param name="value">The Value</param>
        /// <param name="minValue">The minimum value</param>
        /// <param name="maxValue">The maximum value</param>
        /// <param name="defaultValue">The default value</param>
        public static Int64 InRange(this Int64 value, Int64 minValue, Int64 maxValue, Int64 defaultValue)
        {
            return value.InRange(minValue, maxValue) ? value : defaultValue;
        }

        /// <summary>Checks whether the value is in range</summary>
        /// <param name="value">The Value</param>
        /// <param name="minValue">The minimum value</param>
        /// <param name="maxValue">The maximum value</param>
        public static bool InRange(this Int64 value, Int64 minValue, Int64 maxValue)
        {
            return (value >= minValue && value <= maxValue);
        }
        
        /// <summary>
        /// A prime number (or a prime) is a natural number that has exactly two distinct natural number divisors: 1 and itself.
        /// </summary>
        /// <param name="value">Object value</param>
        /// <returns>Returns true if the value is a prime number.</returns>
        public static bool IsPrime(this Int64 value)
        {
            if ((value & 1) == 0) return (value == 2);
            for (Int64 i = 3; (i * i) <= value; i += 2)
                if ((value % i) == 0) return false;
            return (value != 1);
        }

        #endregion

        #region To Value

        public static Int64 ToMinusOne(this Int64? value)
        {
            return value.HasValue ? value.Value : -1;
        }

        public static Int64 ToZero(this Int64? value)
        {
            return value.HasValue ? value.Value : 0;
        }

        public static String ToStringEmpty(this Int64? value)
        {
            return value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : String.Empty;
        }

        public static String ToStringZero(this Int64? value)
        {
            return value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : "0";
        }

        #region TO Ordinal

        /// <summary>
        /// Converts the value to ordinal String. (English)
        /// </summary>
        /// <param name="value">Object value</param>
        /// <returns>Returns String containing ordinal indicator adjacent to a numeral denoting. (English)</returns>
        public static String ToOrdinal(this Int64 value)
        {
            var suffix = "th";
            switch (value % 100)
            {
            case 11:
            case 12:
            case 13:
                break;
            default:
                switch (value % 10)
                {
                case 1:
                    suffix = "st";
                    break;
                case 2:
                    suffix = "nd";
                    break;
                case 3:
                    suffix = "rd";
                    break;
                }
                break;
            }

            return String.Format("{0}{1}", value, suffix);
        }

        /// <summary>
        /// Converts the value to ordinal String with specified format. (English)
        /// </summary>
        /// <param name="value">Object value</param>
        /// <param name="format">A standard or custom format String that is supported by the object to be formatted.</param>
        /// <returns>Returns String containing ordinal indicator adjacent to a numeral denoting. (English)</returns>
        public static String ToOrdinal(this Int64 value, String format)
        {
            return String.Format(format, value.ToOrdinal());
        }

        #endregion
        #endregion

        #region TimeSpan
        /// <summary>
        /// Gets a TimeSpan from a Int64 number of days.
        /// </summary>
        /// <param name="days">The number of days the TimeSpan will contain.</param>
        /// <returns>A TimeSpan containing the specified number of days.</returns>
        public static TimeSpan Days(this Int64 days)
        {
            return TimeSpan.FromDays(days);
        }

        /// <summary>
        /// Gets a TimeSpan from a Int64 number of hours.
        /// </summary>
        /// <param name="hours">The number of hours the TimeSpan will contain.</param>
        /// <returns>A TimeSpan containing the specified number of hours.</returns>
        public static TimeSpan Hours(this Int64 hours)
        {
            return TimeSpan.FromHours(hours);
        }

        /// <summary>
        /// Gets a TimeSpan from a Int64 number of minutes.
        /// </summary>
        /// <param name="minutes">The number of minutes the TimeSpan will contain.</param>
        /// <returns>A TimeSpan containing the specified number of minutes.</returns>
        public static TimeSpan Minutes(this Int64 minutes)
        {
            return TimeSpan.FromMinutes(minutes);
        }

        /// <summary>
        /// Gets a TimeSpan from a Int64 number of seconds.
        /// </summary>
        /// <param name="seconds">The number of seconds the TimeSpan will contain.</param>
        /// <returns>A TimeSpan containing the specified number of seconds.</returns>
        public static TimeSpan Seconds(this Int64 seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        /// <summary>
        /// Gets a TimeSpan from a Int64 number of milliseconds.
        /// </summary>
        /// <param name="milliseconds">The number of milliseconds the TimeSpan will contain.</param>
        /// <returns>A TimeSpan containing the specified number of milliseconds.</returns>
        public static TimeSpan Milliseconds(this Int64 milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds);
        }

        /// <summary>
        /// Gets a TimeSpan from a Int64 number of ticks.
        /// </summary>
        /// <param name="ticks">The number of ticks the TimeSpan will contain.</param>
        /// <returns>A TimeSpan containing the specified number of ticks.</returns>
        public static TimeSpan Ticks(this Int64 ticks)
        {
            return TimeSpan.FromTicks(ticks);
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int64 Factorial(this Int64 value)
        {
            if (value < 0) value = -value;
            if (value == 0 || value == 1) return 1;
            if (value == 2) return 2;
            if (value == 3) return 6;

            return value * Factorial(value - 1);
        }


    }
}