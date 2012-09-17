namespace System
{
    using Globalization;
    using SystemDependencies;
    using Text;

    public static class DateTimeExtension
    {
        public static DateTime? AddTimeNull(this DateTime? datetime, TimeSpan timeSpan)
        {
            return datetime.HasValue
                       ? datetime.Value.Date.Add(timeSpan)
                       : default(DateTime?);
        }

        public static DateTime? ToDayBegin(this DateTime? datetime)
        {
            return datetime.HasValue
                       ? new DateTime(datetime.Value.Year, datetime.Value.Month, datetime.Value.Day, 0, 0, 0)
                       : default(DateTime?);
        }

        public static DateTime? ToDayEnd(this DateTime? datetime)
        {
            return datetime.HasValue
                       ? new DateTime(datetime.Value.Year, datetime.Value.Month, datetime.Value.Day, 23, 59, 59)
                       : default(DateTime?);
        }

        public static DateTime? ToYearBegin(this DateTime? datetime)
        {
            return datetime.HasValue
                       ? new DateTime(datetime.Value.Year, 1, 1)
                       : default(DateTime?);
        }

        public static DateTime? ToYearEnd(this DateTime? datetime)
        {
            return datetime.HasValue
                       ? new DateTime(datetime.Value.Year, 12, 31)
                       : default(DateTime?);
        }

        public static String ToDayBeginString(this DateTime? datetime)
        {
            return datetime.HasValue
                       ? String.Format("{0} 00:00:00", datetime.Value.ToShortDateString())
                       : String.Empty;
        }

        public static String ToDayEndString(this DateTime? datetime)
        {
            return datetime.HasValue
                       ? String.Format("{0} 23:59:59", datetime.Value.ToShortDateString())
                       : String.Empty;
        }

        public static String ToYearBeginString(this DateTime? datetime)
        {
            return datetime.HasValue
                       ? (new DateTime(datetime.Value.Year, 1, 1, 0, 0, 0)).ToString(CultureInfo.InvariantCulture)
                       : String.Empty;
        }

        public static String ToYearEndString(this DateTime? datetime)
        {
            return datetime.HasValue
                       ? (new DateTime(datetime.Value.Year, 12, 31, 23, 59, 59)).ToString(CultureInfo.InvariantCulture)
                       : String.Empty;
        }

        public static String ToDateEmptyString(this DateTime? datetime)
        {
            return datetime.HasValue
                       ? datetime.Value.ToShortDateString()
                       : String.Empty;
        }

        public static String ToTimeEmptyString(this DateTime? datetime)
        {
            return datetime.HasValue
                       ? datetime.Value.ToShortTimeString()
                       : String.Empty;
        }

        private const int EveningEnds = 2;
        private const int MorningEnds = 12;
        private const int AfternoonEnds = 6;
        private static readonly DateTime Date1970 = new DateTime(1970, 1, 1);

        ///<summary>
        ///	Return System UTC Offset
        ///</summary>
        public static double UtcOffset
        {
            get { return DateTime.Now.Subtract(DateTime.UtcNow).TotalHours; }
        }

        /// <summary>
        /// 	Calculates the age based on a passed reference date.
        /// </summary>
        /// <param name="dateOfBirth">The date & time of birth. </param>
        /// <param name="dateOfRef">The reference date to calculate on </param>
        /// <returns>The calculated age.</returns>
        public static int CalculateAge(this DateTime dateOfBirth, DateTime dateOfRef)
        {
            var years = dateOfRef.Year - dateOfBirth.Year;
            if (dateOfRef.Month < dateOfBirth.Month
                || (dateOfRef.Month == dateOfBirth.Month && dateOfRef.Day < dateOfBirth.Day)) --years;
            return years;
        }

        /// <summary>
        /// 	Calculates the age based on today.
        /// </summary>
        /// <param name = "dateOfBirth">The date & time of birth.</param>
        /// <returns>The calculated age.</returns>
        public static int CalculateAge(this DateTime dateOfBirth)
        {
            return CalculateAge(dateOfBirth, Clock.Now.Date);
        }

        /// <summary>
        /// 	Returns the number of days in the month of the provided datetime.
        /// </summary>
        /// <param name = "datetime">The datetime.</param>
        /// <returns>The number of days.</returns>
        public static int GetCountDaysOfMonth(this DateTime datetime)
        {
            var nextMonth = datetime.AddMonths(1);
            return new DateTime(nextMonth.Year, nextMonth.Month, 1).AddDays(-1).Day;
        }

        /// <summary>
        /// 	Returns the first day of the month of the provided datetime.
        /// </summary>
        /// <param name = "datetime">The datetime.</param>
        /// <returns>The first day of the month</returns>
        public static DateTime GetFirstDayOfMonth(this DateTime datetime)
        {
            return new DateTime(datetime.Year, datetime.Month, 1);
        }

        /// <summary>
        /// 	Returns the first day of the month of the provided datetime.
        /// </summary>
        /// <param name = "datetime">The datetime.</param>
        /// <param name = "dayOfWeek">The desired day of week.</param>
        /// <returns>The first day of the month</returns>
        public static DateTime GetFirstDayOfMonth(this DateTime datetime, DayOfWeek dayOfWeek)
        {
            var dtFirst = datetime.GetFirstDayOfMonth();
            while (dtFirst.DayOfWeek != dayOfWeek) dtFirst = dtFirst.AddDays(1);
            return dtFirst;
        }

        /// <summary>
        /// 	Returns the last day of the month of the provided datetime.
        /// </summary>
        /// <param name = "datetime">The datetime.</param>
        /// <returns>The last day of the month.</returns>
        public static DateTime GetLastDayOfMonth(this DateTime datetime)
        {
            return new DateTime(datetime.Year, datetime.Month, GetCountDaysOfMonth(datetime));
        }

        /// <summary>
        /// 	Returns the last day of the month of the provided datetime.
        /// </summary>
        /// <param name = "datetime">The datetime.</param>
        /// <param name = "dayOfWeek">The desired day of week.</param>
        /// <returns>The datetime</returns>
        public static DateTime GetLastDayOfMonth(this DateTime datetime, DayOfWeek dayOfWeek)
        {
            var dtLast = datetime.GetLastDayOfMonth();
            while (dtLast.DayOfWeek != dayOfWeek) dtLast = dtLast.AddDays(-1);
            return dtLast;
        }

        /// <summary>
        /// 	Indicates whether the datetime is today.
        /// </summary>
        /// <param name = "datetime">The datetime.</param>
        /// <returns>
        /// 	<c>true</c> if the specified datetime is today; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsToday(this DateTime datetime)
        {
            return (datetime.Date == DateTime.Today);
        }

        /// <summary>
        /// 	Sets the time on the specified DateTime value.
        /// </summary>
        /// <param name = "datetime">The base datetime.</param>
        /// <param name = "timeSpan">The TimeSpan to be applied.</param>
        /// <returns>
        /// 	The DateTime including the new time value
        /// </returns>
        public static DateTime SetTime(this DateTime datetime, TimeSpan timeSpan)
        {
            return datetime.Date.Add(timeSpan);
        }

        /// <summary>
        /// 	Sets the time on the specified DateTime value.
        /// </summary>
        /// <param name = "datetime">The base datetime.</param>
        /// <param name="hours">The hour</param>
        /// <param name="minutes">The minute</param>
        /// <param name="seconds">The second</param>
        /// <param name="milliseconds">The millisecond</param>
        /// <returns>The DateTime including the new time value</returns>
        /// <remarks>Added overload for milliseconds - jtolar</remarks>
        public static DateTime SetTime(this DateTime datetime, int hours, int minutes, int seconds, int milliseconds)
        {
            return datetime.SetTime(new TimeSpan(0, hours, minutes, seconds, milliseconds));
        }

        /// <summary>
        /// 	Sets the time on the specified DateTime value.
        /// </summary>
        /// <param name = "datetime">The base datetime.</param>
        /// <param name = "hours">The hours to be set.</param>
        /// <param name = "minutes">The minutes to be set.</param>
        /// <param name = "seconds">The seconds to be set.</param>
        /// <returns>The DateTime including the new time value</returns>
        public static DateTime SetTime(this DateTime datetime, int hours, int minutes, int seconds)
        {
            return datetime.SetTime(hours, minutes, seconds, 0);
        }

        /// <summary>
        /// 	Converts a DateTime into a DateTimeOffset using the specified time zone.
        /// </summary>
        /// <param name = "localDateTime">The local DateTime.</param>
        /// <param name = "localTimeZone">The local time zone.</param>
        /// <returns>The converted DateTimeOffset</returns>
        public static DateTimeOffset ToDateTimeOffset(this DateTime localDateTime, TimeZoneInfo localTimeZone)
        {
            localTimeZone = (localTimeZone ?? TimeZoneInfo.Local);

            if (localDateTime.Kind != DateTimeKind.Unspecified) localDateTime = new DateTime(localDateTime.Ticks, DateTimeKind.Unspecified);

            return TimeZoneInfo.ConvertTimeToUtc(localDateTime, localTimeZone);
        }

        /// <summary>
        /// 	Converts a DateTime into a DateTimeOffset using the local system time zone.
        /// </summary>
        /// <param name = "localDateTime">The local DateTime.</param>
        /// <returns>The converted DateTimeOffset</returns>
        public static DateTimeOffset ToDateTimeOffset(this DateTime localDateTime)
        {
            return localDateTime.ToDateTimeOffset(default(TimeZoneInfo));
        }

        /// <summary>
        /// 	Gets the first day of the week using the specified culture.
        /// </summary>
        /// <param name = "datetime">The datetime.</param>
        /// <param name = "cultureInfo">The culture to determine the first weekday of a week.</param>
        /// <returns>The first day of the week</returns>
        public static DateTime GetFirstDayOfWeek(this DateTime datetime, CultureInfo cultureInfo)
        {
            var firstDayOfWeek = (cultureInfo ?? CultureInfo.CurrentCulture).DateTimeFormat.FirstDayOfWeek;
            while (datetime.DayOfWeek != firstDayOfWeek) datetime = datetime.AddDays(-1);
            return datetime;
        }

        /// <summary>
        /// 	Gets the first day of the week using the current culture.
        /// </summary>
        /// <param name = "datetime">The datetime.</param>
        /// <returns>The first day of the week</returns>
        /// <remarks>
        ///     modified by jtolar to implement culture settings
        /// </remarks>
        public static DateTime GetFirstDayOfWeek(this DateTime datetime)
        {
            return datetime.GetFirstDayOfWeek(ExtensionMethodSetting.DefaultCulture);
        }

        /// <summary>
        /// 	Gets the last day of the week using the specified culture.
        /// </summary>
        /// <param name = "datetime">The datetime.</param>
        /// <param name = "cultureInfo">The culture to determine the first weekday of a week.</param>
        /// <returns>The first day of the week</returns>
        public static DateTime GetLastDayOfWeek(this DateTime datetime, CultureInfo cultureInfo)
        {
            return datetime.GetFirstDayOfWeek(cultureInfo).AddDays(6);
        }

        /// <summary>
        /// 	Gets the last day of the week using the current culture.
        /// </summary>
        /// <param name = "datetime">The datetime.</param>
        /// <returns>The first day of the week</returns>
        /// <remarks>
        ///     modified by jtolar to implement culture settings
        /// </remarks>
        public static DateTime GetLastDayOfWeek(this DateTime datetime)
        {
            return datetime.GetLastDayOfWeek(ExtensionMethodSetting.DefaultCulture);
        }

        /// <summary>
        /// 	Gets the next occurence of the specified weekday within the current week using the specified culture.
        /// </summary>
        /// <param name = "datetime">The base datetime.</param>
        /// <param name = "weekday">The desired weekday.</param>
        /// <param name = "cultureInfo">The culture to determine the first weekday of a week.</param>
        /// <returns>The calculated datetime.</returns>
        /// <example>
        /// 	<code>
        /// 		var thisWeeksMonday = DateTime.Now.GetWeekday(DayOfWeek.Monday);
        /// 	</code>
        /// </example>
        public static DateTime GetWeeksWeekday(this DateTime datetime, DayOfWeek weekday, CultureInfo cultureInfo)
        {
            var firstDayOfWeek = datetime.GetFirstDayOfWeek(cultureInfo);
            return firstDayOfWeek.GetNextWeekday(weekday);
        }

        /// <summary>
        /// 	Gets the next occurence of the specified weekday within the current week using the current culture.
        /// </summary>
        /// <param name = "datetime">The base datetime.</param>
        /// <param name = "weekday">The desired weekday.</param>
        /// <returns>The calculated datetime.</returns>
        /// <example>
        /// 	<code>
        /// 		var thisWeeksMonday = DateTime.Now.GetWeekday(DayOfWeek.Monday);
        /// 	</code>
        /// </example>
        /// <remarks>
        ///     modified by jtolar to implement culture settings
        /// </remarks>
        public static DateTime GetWeeksWeekday(this DateTime datetime, DayOfWeek weekday)
        {
            return datetime.GetWeeksWeekday(weekday, ExtensionMethodSetting.DefaultCulture);
        }

        /// <summary>
        /// 	Gets the next occurence of the specified weekday.
        /// </summary>
        /// <param name = "datetime">The base datetime.</param>
        /// <param name = "weekday">The desired weekday.</param>
        /// <returns>The calculated datetime.</returns>
        /// <example>
        /// 	<code>
        /// 		var lastMonday = DateTime.Now.GetNextWeekday(DayOfWeek.Monday);
        /// 	</code>
        /// </example>
        public static DateTime GetNextWeekday(this DateTime datetime, DayOfWeek weekday)
        {
            while (datetime.DayOfWeek != weekday) datetime = datetime.AddDays(1);
            return datetime;
        }

        /// <summary>
        /// 	Gets the previous occurence of the specified weekday.
        /// </summary>
        /// <param name = "datetime">The base datetime.</param>
        /// <param name = "weekday">The desired weekday.</param>
        /// <returns>The calculated datetime.</returns>
        /// <example>
        /// 	<code>
        /// 		var lastMonday = DateTime.Now.GetPreviousWeekday(DayOfWeek.Monday);
        /// 	</code>
        /// </example>
        public static DateTime GetPreviousWeekday(this DateTime datetime, DayOfWeek weekday)
        {
            while (datetime.DayOfWeek != weekday) datetime = datetime.AddDays(-1);
            return datetime;
        }

        /// <summary>
        /// 	Determines whether the date only part of twi DateTime values are equal.
        /// </summary>
        /// <param name = "datetime">The DateTime.</param>
        /// <param name = "dateToCompare">The date to compare with.</param>
        /// <returns>
        /// 	<c>true</c> if both date values are equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDateEqual(this DateTime datetime, DateTime dateToCompare)
        {
            return (datetime.Date == dateToCompare.Date);
        }

        /// <summary>
        /// 	Determines whether the time only part of two DateTime values are equal.
        /// </summary>
        /// <param name = "datetime">The time.</param>
        /// <param name = "timeToCompare">The time to compare.</param>
        /// <returns>
        /// 	<c>true</c> if both time values are equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsTimeEqual(this DateTime datetime, DateTime timeToCompare)
        {
            return (datetime.TimeOfDay == timeToCompare.TimeOfDay);
        }

        /// <summary>
        /// 	Get milliseconds of UNIX area. This is the milliseconds since 1/1/1970
        /// </summary>
        /// <param name = "datetime">Up to which time.</param>
        /// <returns>number of milliseconds.</returns>
        /// <remarks>
        /// 	Contributed by blaumeister, http://www.codeplex.com/site/users/view/blaumeiser
        /// </remarks>
        public static long GetMillisecondsSince1970(this DateTime datetime)
        {
            var timeSpan = datetime.Subtract(Date1970);
            return (long) timeSpan.TotalMilliseconds;
        }

        /// <summary>
        /// Get milliseconds of UNIX area. This is the milliseconds since 1/1/1970
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        /// <remarks>This is the same as GetMillisecondsSince1970 but more descriptive</remarks>
        public static long ToUnixEpoch(this DateTime dateTime)
        {
            return GetMillisecondsSince1970(dateTime);
        }

        /// <summary>
        /// 	Indicates whether the specified date is a weekend (Saturday or Sunday).
        /// </summary>
        /// <param name = "datetime">The DateTime.</param>
        /// <returns>
        /// 	<c>true</c> if the specified date is a weekend; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWeekend(this DateTime datetime)
        {
            return datetime.DayOfWeek.EqualsAny(DayOfWeek.Saturday, DayOfWeek.Sunday);
        }

        /// <summary>
        /// 	Adds the specified amount of weeks (=7 days gregorian calendar) to the passed date value.
        /// </summary>
        /// <param name = "datetime">The origin date.</param>
        /// <param name = "value">The amount of weeks to be added.</param>
        /// <returns>The enw date value</returns>
        public static DateTime AddWeeks(this DateTime datetime, int value)
        {
            return datetime.AddDays(value*7);
        }

        ///<summary>
        ///	Get the number of days between two dates.
        ///</summary>
        ///<param name = "fromDate">The origin year.</param>
        ///<param name = "toDate">To year</param>
        ///<returns>The number of days between the two years</returns>
        /// <remarks>
        /// 	Contributed by Michael T, http://about.me/MichaelTran
        /// </remarks>
        public static int GetDays(this DateTime fromDate, DateTime toDate)
        {
            return Convert.ToInt32(toDate.Subtract(fromDate).TotalDays);
        }

        /////<summary>
        /////	Get the number of days within that year. Uses the culture specified.
        /////</summary>
        /////<param name = "year">The year.</param>
        /////<param name="culture">Specific culture</param>
        /////<returns>the number of days within that year</returns>
        //static int GetDays(int year, CultureInfo culture)
        //{
        //    var first = new DateTime(year, 1, 1, culture.Calendar);
        //    var last = new DateTime(year + 1, 1, 1, culture.Calendar);
        //    return GetDays(first, last);
        //}

        /////<summary>
        /////	Get the number of days within that year.
        /////</summary>
        /////<param name = "year">The year.</param>
        /////<returns>the number of days within that year</returns>
        //static int GetDays(int year)
        //{
        //    return GetDays(year, ExtensionMethodSetting.DefaultCulture);
        //}

        ///<summary>
        ///	Get the number of days within that date year. Allows user to specify culture
        ///</summary>
        ///<param name = "datetime">The DateTime.</param>
        ///<param name="culture">Specific culture</param>
        ///<returns>the number of days within that year</returns>
        /// <remarks>
        /// 	Contributed by Michael T, http://about.me/MichaelTran
        ///     Modified by JTolar to implement Culture Settings 
        /// </remarks>
        public static int GetDays(this DateTime datetime, CultureInfo culture)
        {
            var year = datetime.Year;
            var dtFirst = new DateTime(year, 1, 1, culture.Calendar);
            var dtLast = new DateTime(year + 1, 1, 1, culture.Calendar);
            return GetDays(dtFirst, dtLast);
        }

        ///<summary>
        ///	Get the number of days within that date year. Allows user to specify culture.
        ///</summary>
        ///<param name = "datetime">The DateTime.</param>
        ///<returns>the number of days within that year</returns>
        public static int GetDays(this DateTime datetime)
        {
            return GetDays(datetime, ExtensionMethodSetting.DefaultCulture);
        }

        ///<summary>
        ///	Return a period "Morning", "Afternoon", or "Evening"
        ///</summary>
        ///<param name = "datetime">The DateTime.</param>
        ///<returns>The period "morning", "afternoon", or "evening"</returns>
        /// <remarks>
        /// 	Contributed by Michael T, http://about.me/MichaelTran
        /// </remarks>
        public static string GetPeriodOfDay(this DateTime datetime)
        {
            var hour = datetime.Hour;
            if (hour < EveningEnds) return "evening";
            if (hour < MorningEnds) return "morning";
            return hour < AfternoonEnds ? "afternoon" : "evening";
        }

        /// <summary>
        /// Gets the week number for a provided date time value based on a specific culture.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="culture">Specific culture</param>
        /// <returns>The week number</returns>
        /// <remarks>
        ///     modified by jtolar to implement culture settings
        /// </remarks>
        public static int GetWeekOfYear(this DateTime dateTime, CultureInfo culture)
        {
            var calendar = culture.Calendar;
            var dateTimeFormatInfo = culture.DateTimeFormat;

            return calendar.GetWeekOfYear(dateTime, dateTimeFormatInfo.CalendarWeekRule,
                                          dateTimeFormatInfo.FirstDayOfWeek);
        }

        /// <summary>
        /// Gets the week number for a provided date time value based on the current culture settings. 
        /// Uses DefaultCulture from ExtensionMethodSetting
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The week number</returns>
        /// <remarks>
        ///     modified by jtolar to implement culture settings
        /// </remarks>
        public static int GetWeekOfYear(this DateTime dateTime)
        {
            return GetWeekOfYear(dateTime, ExtensionMethodSetting.DefaultCulture);
        }

        /// <summary>
        ///     Indicates whether the specified date is Easter in the Christian calendar.
        /// </summary>
        /// <param name="datetime">Instance value.</param>
        /// <returns>True if the instance value is a valid Easter Date.</returns>
        public static bool IsEaster(this DateTime datetime)
        {
            var year = datetime.Year;

            var a = year%19;
            var b = year/100;
            var c = year%100;
            var d = b/4;
            var e = b%4;
            var f = (b + 8)/25;
            var g = (b - f + 1)/3;
            var h = (19*a + b - d - g + 15)%30;
            var i = c/4;
            var k = c%4;
            var l = (32 + 2*e + 2*i - h - k)%7;
            var m = (a + 11*h + 22*l)/451;

            var month = (h + l - 7*m + 114)/31;
            var day = ((h + l - 7*m + 114)%31) + 1;

            var dtEasterSunday = new DateTime(year, month, day);

            return datetime == dtEasterSunday;
        }

        /// <summary>
        ///     Indicates whether the source DateTime is before the supplied DateTime.
        /// </summary>
        /// <param name="datetime">The source DateTime.</param>
        /// <param name="other">The compared DateTime.</param>
        /// <returns>True if the source is before the other DateTime, False otherwise</returns>
        public static bool IsBefore(this DateTime datetime, DateTime other)
        {
            return datetime.CompareTo(other) < 0;
        }

        /// <summary>
        ///     Indicates whether the source DateTime is before the supplied DateTime.
        /// </summary>
        /// <param name="datetime">The source DateTime.</param>
        /// <param name="other">The compared DateTime.</param>
        /// <returns>True if the source is before the other DateTime, False otherwise</returns>
        public static bool IsAfter(this DateTime datetime, DateTime other)
        {
            return datetime.CompareTo(other) > 0;
        }

        /// <summary>
        /// Gets a DateTime representing Next Day
        /// </summary>
        /// <param name="datetime">The current day</param>
        /// <returns></returns>
        public static DateTime Tomorrow(this DateTime datetime)
        {
            return datetime.AddDays(1);
        }

        /// <summary>
        /// Gets a DateTime representing Previous Day
        /// </summary>
        /// <param name="datetime">The current day</param>
        /// <returns></returns>
        public static DateTime Yesterday(this DateTime datetime)
        {
            return datetime.AddDays(-1);
        }

        /// <summary>
        /// The ToFriendlyString() method represents dates in a user friendly way. 
        /// For example, when displaying a news article on a webpage, you might want 
        /// articles that were published one day ago to have their publish dates 
        /// represented as "yesterday at 12:30 PM". Or if the article was publish today, 
        /// show the date as "Today, 3:33 PM".
        /// </summary>
        /// <param name="datetime">The DateTime.</param>
        /// <param name="culture">Specific Culture</param>
        /// <returns>string</returns>
        /// <remarks>
        ///     modified by jtolar to implement culture settings
        /// </remarks>/// <remarks></remarks>
        public static string ToFriendlyDateString(this DateTime datetime, CultureInfo culture)
        {
            var sb = new StringBuilder();
            if (datetime.Date == DateTime.Today) sb.Append("Today");
            else if (datetime.Date == DateTime.Today.AddDays(-1)) sb.Append("Yesterday");
            else if (datetime.Date > DateTime.Today.AddDays(-6)) // *** Show the Day of the week
                sb.Append(datetime.ToString("dddd").ToString(culture));
            else sb.Append(datetime.ToString("MMMM dd, yyyy").ToString(culture));

            //append the time portion to the output
            sb.Append(" at ").Append(datetime.ToString("t").ToLower());
            return sb.ToString();
        }

        ///<summary>
        /// The ToFriendlyString() method represents dates in a user friendly way. 
        /// For example, when displaying a news article on a webpage, you might want 
        /// articles that were published one day ago to have their publish dates 
        /// represented as "yesterday at 12:30 PM". Or if the article was publish today, 
        /// show the date as "Today, 3:33 PM". Uses DefaultCulture from ExtensionMethodSetting.
        /// </summary>
        /// <param name="datetime">The DateTime.</param>
        /// <returns>string</returns>
        /// <remarks>
        ///     modified by jtolar to implement culture settings
        /// </remarks>/// <remarks></remarks>
        public static string ToFriendlyDateString(this DateTime datetime)
        {
            return ToFriendlyDateString(datetime, ExtensionMethodSetting.DefaultCulture);
        }

        /// <summary>
        /// Returns the date at 23:59.59.999 for the specified DateTime
        /// </summary>
        /// <param name="datetime">The DateTime to be processed</param>
        /// <returns>The date at 23:50.59.999</returns>
        public static DateTime EndOfDay(this DateTime datetime)
        {
            return datetime.SetTime(23, 59, 59, 999);
        }

        /// <summary>
        /// Returns the date at 12:00:00 for the specified DateTime
        /// </summary>
        /// <param name="datetime">The current date</param>
        public static DateTime Noon(this DateTime datetime)
        {
            return datetime.SetTime(12, 0, 0);
        }

        /// <summary>
        /// Returns the date at 00:00:00 for the specified DateTime
        /// </summary>
        /// <param name="datetime">The current date</param>
        public static DateTime Midnight(this DateTime datetime)
        {
            return datetime.SetTime(0, 0, 0, 0);
        }

        /// <summary>
        /// Returns whether the DateTime falls on a weekday
        /// </summary>
        /// <param name="datetime">The date to be processed</param>
        /// <returns>Whether the specified date occurs on a weekday</returns>
        public static bool IsWeekDay(this DateTime datetime)
        {
            return !datetime.IsWeekend();
        }
    }
}