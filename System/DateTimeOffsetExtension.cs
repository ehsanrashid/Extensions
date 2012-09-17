namespace System
{
    /// <summary>
    /// 	Extension methods for the DateTimeOffset data type.
    /// </summary>
    public static class DateTimeOffsetExtension
    {
        /// <summary>
        /// 	Indicates whether the date is today.
        /// </summary>
        /// <param name = "datetimeOff">The date.</param>
        /// <returns>
        /// 	<c>true</c> if the specified date is today; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsToday(this DateTimeOffset datetimeOff)
        {
            return datetimeOff.Date.IsToday();
        }

        /// <summary>
        /// 	Sets the time on the specified DateTime value using the specified time zone.
        /// </summary>
        /// <param name = "datetimeOff">The base date.</param>
        /// <param name = "timespan">The TimeSpan to be applied.</param>
        /// <param name = "localTimeZone">The local time zone.</param>
        /// <returns>/// The DateTimeOffset including the new time value/// </returns>
        public static DateTimeOffset SetTime(this DateTimeOffset datetimeOff, TimeSpan timespan, TimeZoneInfo localTimeZone)
        {
            var localDate = datetimeOff.ToLocalDateTime(localTimeZone);
            localDate.SetTime(timespan);
            return localDate.ToDateTimeOffset(localTimeZone);
        }

        /// <summary>
        /// 	Sets the time on the specified DateTime value using the local system time zone.
        /// </summary>
        /// <param name = "datetimeOff">The base date.</param>
        /// <param name = "timespan">The TimeSpan to be applied.</param>
        /// <returns>
        /// 	The DateTimeOffset including the new time value
        /// </returns>
        public static DateTimeOffset SetTime(this DateTimeOffset datetimeOff, TimeSpan timespan)
        {
            return datetimeOff.SetTime(timespan, default(TimeZoneInfo));
        }

        /// <summary>
        /// 	Sets the time on the specified DateTimeOffset value using the local system time zone.
        /// </summary>
        /// <param name = "datetimeOff">The base date.</param>
        /// <param name = "hours">The hours to be set.</param>
        /// <param name = "minutes">The minutes to be set.</param>
        /// <param name = "seconds">The seconds to be set.</param>
        /// <returns>The DateTimeOffset including the new time value</returns>
        public static DateTimeOffset SetTime(this DateTimeOffset datetimeOff, int hours, int minutes, int seconds)
        {
            return datetimeOff.SetTime(new TimeSpan(hours, minutes, seconds));
        }

        /// <summary>
        /// 	Converts a DateTimeOffset into a DateTime using the specified time zone.
        /// </summary>
        /// <param name = "dateTimeUtc">The base DateTimeOffset.</param>
        /// <param name = "localTimeZone">The time zone to be used for conversion.</param>
        /// <returns>The converted DateTime</returns>
        public static DateTime ToLocalDateTime(this DateTimeOffset dateTimeUtc, TimeZoneInfo localTimeZone)
        {
            return TimeZoneInfo.ConvertTime(dateTimeUtc, (localTimeZone ?? TimeZoneInfo.Local)).DateTime;
        }

        /// <summary>
        /// 	Converts a DateTimeOffset into a DateTime using the local system time zone.
        /// </summary>
        /// <param name = "dateTimeUtc">The base DateTimeOffset.</param>
        /// <returns>The converted DateTime</returns>
        public static DateTime ToLocalDateTime(this DateTimeOffset dateTimeUtc)
        {
            return dateTimeUtc.ToLocalDateTime(default(TimeZoneInfo));
        }

    }
}