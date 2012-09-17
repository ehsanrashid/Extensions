namespace System
{
    /// <summary>
    /// 	Extension methods for the Double data type
    /// </summary>
    public static class DoubleExtension
    {
        /// <summary>Checks whether the value is in range or returns the default value</summary>
        /// <param name="value">The Value</param>
        /// <param name="minValue">The minimum value</param>
        /// <param name="maxValue">The maximum value</param>
        /// <param name="defaultValue">The default value</param>
        public static Double InRange(this Double value, Double minValue, Double maxValue, Double defaultValue)
        {
            return value.InRange(minValue, maxValue) ? value : defaultValue;
        }
        
        /// <summary>Checks whether the value is in range</summary>
        /// <param name="value">The Value</param>
        /// <param name="minValue">The minimum value</param>
        /// <param name="maxValue">The maximum value</param>
        public static bool InRange(this Double value, Double minValue, Double maxValue)
        {
            return (minValue <= value && value <= maxValue);
        }

        #region TimeSpan

        /// <summary>
        /// Gets a TimeSpan from a Double number of days.
        /// </summary>
        /// <param name="days">The number of days the TimeSpan will contain.</param>
        /// <returns>A TimeSpan containing the specified number of days.</returns>
        /// <remarks>
        ///		Contributed by jceddy
        /// </remarks>
        public static TimeSpan Days(this Double days)
        {
            return TimeSpan.FromDays(days);
        }

        /// <summary>
        /// Gets a TimeSpan from a Double number of hours.
        /// </summary>
        /// <param name="hours">The number of hours the TimeSpan will contain.</param>
        /// <returns>A TimeSpan containing the specified number of hours.</returns>
        /// <remarks>
        ///		Contributed by jceddy
        /// </remarks>
        public static TimeSpan Hours(this Double hours)
        {
            return TimeSpan.FromHours(hours);
        }

        /// <summary>
        /// Gets a TimeSpan from a Double number of minutes.
        /// </summary>
        /// <param name="minutes">The number of minutes the TimeSpan will contain.</param>
        /// <returns>A TimeSpan containing the specified number of minutes.</returns>
        /// <remarks>
        ///		Contributed by jceddy
        /// </remarks>
        public static TimeSpan Minutes(this Double minutes)
        {
            return TimeSpan.FromMinutes(minutes);
        }

        /// <summary>
        /// Gets a TimeSpan from a Double number of seconds.
        /// </summary>
        /// <param name="seconds">The number of seconds the TimeSpan will contain.</param>
        /// <returns>A TimeSpan containing the specified number of seconds.</returns>
        /// <remarks>
        ///		Contributed by jceddy
        /// </remarks>
        public static TimeSpan Seconds(this Double seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        /// <summary>
        /// Gets a TimeSpan from a Double number of milliseconds.
        /// </summary>
        /// <param name="milliseconds">The number of milliseconds the TimeSpan will contain.</param>
        /// <returns>A TimeSpan containing the specified number of milliseconds.</returns>
        /// <remarks>
        ///		Contributed by jceddy
        /// </remarks>
        public static TimeSpan Milliseconds(this Double milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds);
        }

        //public static TimeSpan Ticks(this Double ticks)
        //{
        //    return TimeSpan.FromTicks((long) ticks);
        //}

        #endregion

    }
}