namespace System
{
    public static class BooleanExtension
    {
        public static Boolean ToFalse(this Boolean? value)
        {
            //return value.HasValue ? value.Value : false;
            return value.HasValue && value.Value;
        }

        public static Boolean ToTrue(this Boolean? value)
        {
            //return value.HasValue ? value.Value : true;
            return !value.HasValue || value.Value;
        }

        /// <summary>
        /// Converts the value of this instance to its equivalent String representation (either "Yes" or "No").
        /// </summary>
        /// <param name="boolean"></param>
        /// <returns>String</returns>
        public static String ToYesNoString(this Boolean boolean) { return boolean ? "Yes" : "No"; }

        /// <summary>
        /// Converts the value in number format {1 , 0}.
        /// </summary>
        /// <param name="boolean"></param>
        /// <returns>int</returns>
        /// <example>
        /// 	<code>
        /// 		int result= default(bool).ToBinaryTypeNumber()
        /// 	</code>
        /// </example>
        public static int ToBinaryTypeNumber(this Boolean boolean) { return boolean ? 1 : 0; }

        //public static String ToString(this Boolean value)
        //{
        //    return value ? "True" : "False";
        //}
    }
}