namespace System
{
    using Globalization;
    using Linq;

    /// <summary>
    /// Extension methods for the enum data type
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Removes a flag and returns the new value
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="value">Source enum</param>
        /// <param name="flag">Dumped flag</param>
        /// <returns>Result enum value</returns>
        public static T ClearFlag<T>(this Enum value, T flag)
        {
            return ClearFlags(value, flag);
        }

        /// <summary>
        /// Removes flags and returns the new value
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="value">Source enum</param>
        /// <param name="flags">Dumped flags</param>
        /// <returns>Result enum value</returns>
        public static T ClearFlags<T>(this Enum value, params T[] flags)
        {
            var result = Convert.ToUInt64(value);
            //foreach (T flag in flags)
            //    result &= ~Convert.ToUInt64(flag);

            result = flags.Aggregate(result, (current, flag) => current & ~Convert.ToUInt64(flag));

            return (T) Enum.Parse(value.GetType(), result.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Includes a flag and returns the new value
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="value">Source enum</param>
        /// <param name="flag">Established flag</param>
        /// <returns>Result enum value</returns>
        public static T SetFlag<T>(this Enum value, T flag)
        {
            return SetFlags(value, flag);
        }

        /// <summary>
        /// Includes flags and returns the new value
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="value">Source enum</param>
        /// <param name="flags">Established flags</param>
        /// <returns>Result enum value</returns>
        public static T SetFlags<T>(this Enum value, params T[] flags)
        {
            var result = Convert.ToUInt64(value);
            //foreach (T flag in flags)
            //    result |= Convert.ToUInt64(flag);

            result = flags.Aggregate(result, (current, flag) => current | Convert.ToUInt64(flag));

            return (T) Enum.Parse(value.GetType(), result.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Check to see if enumeration has a specific flag
        /// </summary>
        /// <param name="self">Enumeration to check</param>
        /// <param name="flag">Flag to check for</param>
        /// <returns>Result of check</returns>
        ///<remarks>  This will never be called. Enum.HasFlag is a native method of Enum</remarks>
        public static bool HasFlag<T>(this T self, T flag)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            return HasFlags(self, flag);
        }

        /// <summary>
        /// Check to see if enumeration has a specific flag set
        /// </summary>
        /// <param name="self">Enumeration to check</param>
        /// <param name="flags">Flags to check for</param>
        /// <returns>Result of check</returns>
        public static bool HasFlags<T>(this T self, params T[] flags)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            if (!typeof (T).IsEnum) throw new ArgumentException("variable must be an Enum", "self");

            foreach (var flag in flags)
            {
                if (!Enum.IsDefined(typeof (T), flag)) return false;
                var numFlag = Convert.ToUInt64(flag);
                if ((Convert.ToUInt64(self) & numFlag) != numFlag) return false;
            }

            return true;
        }

        /// <summary>
        /// Description, specified by attribute <c>DisplayStringAttribute</c>.
        /// <para>If the attribute is not specified, returns the default name obtained by the method <c>ToString()</c></para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns>
        /// Returns the description given by the attribute <c>DisplayStringAttribute</c>. 
        /// <para>If the attribute is not specified, returns the default name obtained by the method <c>ToString()</c></para>
        /// </returns>
        /// <see cref="DisplayStringAttribute"/>
        /// <example>
        ///     <code>
        ///         enum OperatingSystem
        ///         {
        ///            [DisplayString("MS-DOS")]
        ///            Msdos,
        ///         
        ///            [DisplayString("Windows 98")]
        ///            Win98,
        ///         
        ///            [DisplayString("Windows XP")]
        ///            Xp,
        ///         
        ///            [DisplayString("Windows Vista")]
        ///            Vista,
        ///         
        ///            [DisplayString("Windows 7")]
        ///            Seven,
        ///         }
        ///         
        ///         public string GetMyOSName()
        ///         {
        ///             var myOS = OperatingSystem.Seven;
        ///             return myOS.DisplayString();
        ///         }
        ///     </code>
        /// </example>
        public static string DisplayString(this Enum value)
        {
            var info = value.GetType().GetField(value.ToString());
            var attributes = (DisplayStringAttribute[]) info.GetCustomAttributes(typeof (DisplayStringAttribute), false);
            return attributes.Length >= 1 ? attributes[0].DisplayString : value.ToString();
        }

        public static T ToEnum<T>(this T self, String value)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            if (!typeof (T).IsEnum) throw new ArgumentException("variable must be an Enum", "self");

            return (T) Enum.Parse(typeof (T), value);
        }
    }
}