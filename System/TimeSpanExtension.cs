namespace System
{
    /// <summary>
    /// TimeSpan extensions
    /// </summary>
    public static class TimeSpanExtension
    {
        /// <summary>
        ///     Multiply a <c>System.TimeSpan</c> by a <paramref name="factor"/>
        /// </summary>
        /// <param name="timeSpan">The given <c>System.TimeSpan</c> to be multiplied</param>
        /// <param name="factor">The multiplier factor</param>
        /// <returns>The multiplication of the <paramref name="timeSpan"/> by <paramref name="factor"/></returns>
        public static TimeSpan MultiplyBy(this TimeSpan timeSpan, int factor)
        {
            return TimeSpan.FromTicks(timeSpan.Ticks*factor);
        }

        /// <summary>
        ///     Multiply a <c>System.TimeSpan</c> by a <paramref name="factor"/>
        /// </summary>
        /// <param name="timeSpan">The given <c>System.TimeSpan</c> to be multiplied</param>
        /// <param name="factor">The multiplier factor</param>
        /// <returns>The multiplication of the <paramref name="timeSpan"/> by <paramref name="factor"/></returns>
        public static TimeSpan MultiplyBy(this TimeSpan timeSpan, double factor)
        {
            return TimeSpan.FromTicks((long) (timeSpan.Ticks*factor));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static String ToReadableString(this TimeSpan timeSpan)
        {
            var formatted = String.Format(@"{0}{1}{2}{3}",
                                          timeSpan.Days > 0
                                              ? String.Format(@"{0:0} days, ", timeSpan.Days)
                                              : String.Empty,
                                          timeSpan.Hours > 0
                                              ? String.Format(@"{0:0} hours, ", timeSpan.Hours)
                                              : String.Empty,
                                          timeSpan.Minutes > 0
                                              ? String.Format(@"{0:0} minutes, ", timeSpan.Minutes)
                                              : String.Empty,
                                          timeSpan.Seconds > 0
                                              ? String.Format(@"{0:0} seconds", timeSpan.Seconds)
                                              : String.Empty);

            if (formatted.EndsWith(", ")) formatted = formatted.Remove(formatted.Length - 2);

            return formatted;
        }
    }
}