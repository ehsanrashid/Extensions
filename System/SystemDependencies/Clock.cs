namespace System.SystemDependencies
{
    static class Clock
    {
        /// <summary>
        ///   Set a substitute (and fix) value for Now.  See <see cref="SubForSystemDate" />
        ///   for usage example.
        /// </summary>
        public static DateTime? SubstituteForNow;

        public static DateTime Now
        {
            get { return (SubstituteForNow ?? DateTime.Now); }
        }
    }
}