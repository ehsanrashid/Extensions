namespace System
{
    using Collections.Generic;
    using Collections.Specialized;
    using ComponentModel;
    using Globalization;
    using IO;
    using Linq;
    using Security;
    using Security.Cryptography;
    using Text;
    using Text.RegularExpressions;
    using Xml;
    using Xml.Linq;
    using Xml.XPath;

    /// <summary>
    ///   Extension methods for the String data type
    /// </summary>
    public static class StringExtension
    {
        #region Validate
        /// <summary>
        ///   Determines whether the specified String is null or empty.
        /// </summary>
        /// <param name="str"> The String to check. </param>
        public static bool IsNullOrEmpty(this String str)
        {
            //return (null == str) || (str.Length == 0);
            return String.IsNullOrEmpty(str);
        }

        /// <summary>
        ///   Determines whether the specified String is not null or empty.
        /// </summary>
        /// <param name="str"> The String to check. </param>
        public static bool IsNotNullOrEmpty(this String str)
        {
            return !IsNullOrEmpty(str);
        }

        /// <summary>
        ///   Finds out if the specified String contains null, empty or consists only of white-space characters
        /// </summary>
        /// <param name="str"> The String to check. </param>
        public static bool IsNullOrWhiteSpace(this String str)
        {
            //return (IsNullOrEmpty(str) || str.All(Char.IsWhiteSpace));
            return String.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        ///   Determines whether the specified String is not null, empty or consists only of white-space characters
        /// </summary>
        /// <param name="str"> The String to check. </param>
        public static bool IsNotNullOrWhiteSpace(this String str)
        {
            return !IsNullOrWhiteSpace(str);
        }

        /// <summary>
        ///   Tests whether the contents of a String is a alphabets
        /// </summary>
        /// <param name="str"> </param>
        /// <returns> </returns>
        public static bool IsAlpha(this String str)
        {
            //var letters = "abcdefghijklmnopqrstuvwxyz";
            //foreach (var c in str.ToLower())
            //{
            //    if (!letters.Contains(c.ToString()))
            //        return false;
            //}
            //return true;
            // ----------------------
            //foreach (var c in str)
            //{
            //    if (!Char.IsLetter(c))
            //        return false;
            //}
            //return true;
            // ----------------------
            return (IsNotNullOrEmpty(str) && str.All(Char.IsLetter));
        }

        /// <summary>
        ///   Tests whether the contents of a String is a numeric value
        /// </summary>
        /// <param name="str"> String </param>
        /// <returns> Boolean indicating whether or not the String contents are numeric </returns>
        public static bool IsNumeric(this String str)
        {
            return (IsNotNullOrEmpty(str) && str.All(Char.IsDigit));
        }

        public static bool IsInteger(this String str)
        {
            const string patInt = "^-[0-9]+$|^[0-9]+$";
            return new Regex(patInt).IsMatch(str);
        }

        public static bool IsEmailAddress(this String str)
        {
            const string patEmailAdd =
                @"^[\w-\.]+" // user
                + @"@([\w-]+\.)+[\w-]{2,6}$"; // domain
            return new Regex(patEmailAdd).IsMatch(str);
        }

        public static bool IsUrl(this String str)
        {
            const string patUrl =
                @"^(https?://)"
                + @"?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?" // user@
                + @"(([0-9]{1,3}\.){3}[0-9]{1,3}" // IP- 199.194.52.184
                + @"|" // allows either IP or domain
                + @"([0-9a-z_!~*'()-]+\.)*" // tertiary domain(s)- www.
                + @"([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]" // second level domain
                + @"(\.[a-z]{2,6})?)" // first level domain- .com or .museum is optional
                + @"(:[0-9]{1,5})?" // port number- :80
                + @"((/?)|" // a slash isn't required if there is no file name
                + @"(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$";
            return new Regex(patUrl).IsMatch(str);
        }

        // -----
        public static bool IsNumber(this String str, bool floatpoint)
        {
            if (IsNullOrEmpty(str)) return false;
            str = str.Trim();

            #region Old
            // ============================
            //foreach (var c in str)
            //{
            //    if (Char.IsDigit(c)) continue;
            //    if (floatpoint)
            //        if (c == '.')
            //        {
            //            floatpoint = false;
            //            continue;
            //        }
            //    return false;
            //}
            //return true;
            // ----------------------------
            //foreach (var c in str.Where((c) => !Char.IsDigit(c)))
            //{
            //    if (floatpoint)
            //        if (c == '.')
            //        {
            //            floatpoint = false;
            //            continue;
            //        }
            //    return false;
            //}
            //return true;
            #endregion

            // ============================
            if (floatpoint)
            {
                double dbl;
                return double.TryParse(str, out dbl);
            }
            long lint;
            return long.TryParse(str, out lint);
        }
        #endregion

        #region If Condition
        /// <summary>
        ///   Returns a default value if the String is null or empty.
        /// </summary>
        /// <param name="str"> The String </param>
        /// <param name="defaultValue"> The default value. </param>
        /// <returns> Either the String or the default value. </returns>
        public static String IfNullOrEmpty(this String str, String defaultValue)
        {
            return IsNullOrEmpty(str) ? defaultValue : str;
        }

        /// <summary>
        ///   Returns a default value if the String is null or consists only of white-space characters
        /// </summary>
        /// <param name="str"> The String to check </param>
        /// <param name="defaultValue"> The default value </param>
        /// <returns> Either the String or the default value </returns>
        public static String IfNullOrWhiteSpace(this String str, String defaultValue)
        {
            return IsNullOrWhiteSpace(str) ? defaultValue : str;
        }

        /// <summary>
        ///   Throws an <see cref="System.ArgumentException" /> if the String value is empty.
        /// </summary>
        /// <param name="str"> The value to test. </param>
        /// <param name="message"> The message to display if the value is null. </param>
        /// <param name="name"> The name of the parameter being tested. </param>
        public static String ExceptionIfNullOrEmpty(this String str, String message, String name)
        {
            if (IsNullOrEmpty(str)) throw new ArgumentException(message, name);
            return str;
        }
        #endregion

        #region Ensure
        /// <summary>
        ///   Ensures that a String starts with a given prefix.
        /// </summary>
        /// <param name="str"> The String value to check. </param>
        /// <param name="prefix"> The prefix value to check for. </param>
        /// <returns> The String value including the prefix </returns>
        /// <example>
        ///   <code>var extension = "txt";
        ///     var fileName = String.Concat(file.Name, extension.EnsureStartsWith("."));</code>
        /// </example>
        public static String EnsureStartsWith(this String str, String prefix)
        {
            return (IsNotNullOrEmpty(str) && str.StartsWith(prefix)) ? str : String.Concat(prefix, str);
        }

        /// <summary>
        ///   Ensures that a String ends with a given suffix.
        /// </summary>
        /// <param name="str"> The String value to check. </param>
        /// <param name="suffix"> The suffix value to check for. </param>
        /// <returns> The String value including the suffix </returns>
        /// <example>
        ///   <code>var url = "http://www.pgk.de";
        ///     url = url.EnsureEndsWith("/"));</code>
        /// </example>
        public static String EnsureEndsWith(this String str, String suffix)
        {
            return (IsNotNullOrEmpty(str) && str.EndsWith(suffix)) ? str : String.Concat(str, suffix);
        }
        #endregion

        #region To Value
        public static String ToNull(this String str)
        {
            return IsNullOrWhiteSpace(str) ? default(String) : str.Trim();
        }

        public static String ToEmpty(this String str)
        {
            return IsNullOrWhiteSpace(str) ? String.Empty : str.Trim();
        }

        public static Int16? ToInt16(this String str)
        {
            if (IsNullOrWhiteSpace(str)) return default(Int16?);
            try
            {
                return Convert.ToInt16(str);
            }
            catch
            {
            }
            return default(Int16?);
        }

        public static Int32? ToInt32(this String str)
        {
            if (IsNullOrWhiteSpace(str)) return default(Int32?);
            try
            {
                return Convert.ToInt32(str);
            }
            catch
            {
            }
            return default(Int32?);
        }

        public static Int64? ToInt64(this String str)
        {
            if (IsNullOrWhiteSpace(str)) return default(Int64?);
            try
            {
                return Convert.ToInt64(str);
            }
            catch
            {
            }
            return default(Int64?);
        }

        public static Double? ToDouble(this String str)
        {
            if (IsNullOrWhiteSpace(str)) return default(Double?);
            try
            {
                return Convert.ToDouble(str);
            }
            catch
            {
            }
            return default(Double?);
        }

        public static Decimal? ToDecimal(this String str)
        {
            if (IsNullOrWhiteSpace(str)) return default(Decimal?);
            try
            {
                return Convert.ToDecimal(str);
            }
            catch
            {
            }
            return default(Decimal?);
        }

        public static DateTime? ToDateTime(this String str)
        {
            if (IsNullOrWhiteSpace(str)) return default(DateTime?);
            try
            {
                return Convert.ToDateTime(str).Date;
            }
            catch
            {
            }
            return default(DateTime?);
        }

        public static TimeSpan? ToTimeZero(this String str)
        {
            if (IsNullOrWhiteSpace(str)) return default(TimeSpan?);
            try
            {
                return Convert.ToDateTime(str).TimeOfDay;
            }
            catch
            {
            }
            return default(TimeSpan?);
        }

        public static bool ToBoolean(this String str)
        {
            if (IsNullOrWhiteSpace(str)) return false;
            try
            {
                return Convert.ToBoolean(str);
            }
            catch
            {
            }
            return false;
        }

        public static Int32 ToMinusOne(this String str)
        {
            if (IsNullOrWhiteSpace(str)) return -1;
            try
            {
                return Convert.ToInt32(str);
            }
            catch
            {
            }
            return -1;
        }

        public static Int32 ToZero(this String str)
        {
            if (IsNullOrWhiteSpace(str)) return 0;
            try
            {
                return Convert.ToInt32(str);
            }
            catch
            {
            }
            return 0;
        }

        public static T ToEnum<T>(this String str)
        {
            return IsNullOrWhiteSpace(str) ? default(T) : (T) Enum.Parse(typeof (T), str);
        }
        #endregion

        #region Manupulate

        #region Remove
        /// <summary>
        ///   Return the String with the leftmost number_of_characters characters removed.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="count"> The character count to remove. </param>
        /// <returns> </returns>
        /// <remarks>
        /// </remarks>
        public static String RemoveLeft(this String str, int count)
        {
            return str.Length <= count ? String.Empty : str.Substring(count);
        }

        /// <summary>
        ///   Return the String with the rightmost number_of_characters characters removed.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="count"> The character count to remove. </param>
        /// <returns> </returns>
        /// <remarks>
        /// </remarks>
        public static String RemoveRight(this String str, int count)
        {
            return str.Length <= count ? String.Empty : str.Substring(0, str.Length - count);
        }
        #endregion

        #region Trim
        /// <summary>
        ///   Trims the text to a provided maximum length and adds a suffix if required.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="maxLength"> Maximum length. </param>
        /// <param name="suffix"> The suffix. </param>
        /// <returns> </returns>
        public static String TrimToMaxLength(this String str, int maxLength, String suffix)
        {
            return (IsNullOrEmpty(str) || str.Length <= maxLength)
                       ? str
                       : String.Concat(str.Substring(0, maxLength), suffix);
        }

        /// <summary>
        ///   Trims the text to a provided maximum length.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="maxLength"> Maximum length. </param>
        /// <returns> </returns>
        public static String TrimToMaxLength(this String str, int maxLength)
        {
            return TrimToMaxLength(str, maxLength, String.Empty);
        }
        #endregion

        /// <summary>
        ///   Returns the left part of the String.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="count"> The character count to be returned. </param>
        /// <returns> The left part </returns>
        public static String Left(this String str, int count)
        {
            if (IsNullOrEmpty(str)) return str;
            if (count >= str.Length)
                throw new ArgumentOutOfRangeException("count", count,
                                                      "count must be less than length of String");
            return
                //str.Substring(0, count);
                str.Remove(count);
        }

        /// <summary>
        ///   Returns the Right part of the String.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="count"> The character count to be returned. </param>
        /// <returns> The right part </returns>
        public static String Right(this String str, int count)
        {
            if (IsNullOrEmpty(str)) return str;
            if (count >= str.Length)
                throw new ArgumentOutOfRangeException("count", count,
                                                      "count must be less than length of String");
            return str.Substring(str.Length - count); //, countChar);
        }

        /// <summary>
        ///   Reverses / mirrors a String.
        /// </summary>
        /// <param name="str"> The String to be reversed. </param>
        /// <returns> The reversed String </returns>
        public static String Reverse(this String str)
        {
            if (IsNullOrWhiteSpace(str) || (str.Length == 1)) return str;
            var chars = str.ToCharArray();
            //Array.Reverse(chars); return new String(chars);
            return new String(chars.Reverse().ToArray());
        }

        /// <summary>
        /// </summary>
        /// <param name="str"> </param>
        /// <returns> </returns>
        public static String UpperCaseFirst(this String str)
        {
            if (IsNotNullOrWhiteSpace(str))
            {
                var chars = str.ToCharArray();
                if (Char.IsLower(chars[0]))
                {
                    chars[0] = Char.ToUpper(chars[0]);
                    str = new String(chars);
                }
            }
            return str;
        }

        /// <summary>
        /// </summary>
        /// <param name="str"> </param>
        /// <returns> </returns>
        public static String LowerCaseFirst(this String str)
        {
            if (IsNotNullOrWhiteSpace(str))
            {
                var chars = str.ToCharArray();
                if (Char.IsUpper(chars[0]))
                {
                    chars[0] = Char.ToLower(chars[0]);
                    str = new String(chars);
                }
            }
            return str;
        }

        /// <summary>
        ///   Convert text's case to a title case
        /// </summary>
        /// <remarks>
        ///   UppperCase characters is the source String after the first of each word are lowered, unless the word is exactly 2 characters
        /// </remarks>
        public static String TitleCase(this String str, CultureInfo culture = default (CultureInfo))
        {
            return (culture ?? ExtensionMethodSetting.DefaultCulture).TextInfo.ToTitleCase(str);
        }
        #endregion

        #region Replace
        /// <summary>
        ///   Replace all values in String
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="oldValues"> List of old values, which must be replaced </param>
        /// <param name="predicateReplace"> Function for replacement old values </param>
        /// <returns> Returns new String with the replaced values </returns>
        /// <example>
        ///   <code>var str = "White Red Blue Green Yellow Black Gray";
        ///     var achromaticColors = new[] {"White", "Black", "Gray"};
        ///     str = str.ReplaceAll(achromaticColors, v => "[" + v + "]");
        ///     // str == "[White] Red Blue Green Yellow [Black] [Gray]"</code>
        /// </example>
        public static String Replace(this String str, IEnumerable<String> oldValues,
                                     Func<String, String> predicateReplace)
        {
            if (IsNullOrEmpty(str) || null == oldValues) return str;
            //var sb = new StringBuilder(str);
            //// ----------------------------------
            ////foreach (var oldValue in oldValues)
            ////{
            ////    var newValue = predicateReplace(oldValue);
            ////    sb.Replace(oldValue, newValue);
            ////}
            //// ----------------------------------
            //oldValues.ForEach((item) => sb.Replace(item, predicateReplace(item)));
            //// ----------------------------------
            //return sb.ToString();
            // ====================================
            return oldValues.Aggregate(new StringBuilder(str), (sb, s) => sb.Replace(s, predicateReplace(s))).ToString();
        }

        /// <summary>
        ///   Replace all values in String
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="oldValues"> List of old values, which must be replaced </param>
        /// <param name="newValue"> New value for all old values </param>
        /// <returns> Returns new String with the replaced values </returns>
        /// <example>
        ///   <code>var str = "White Red Blue Green Yellow Black Gray";
        ///     var achromaticColors = new[] {"White", "Black", "Gray"};
        ///     str = str.ReplaceAll(achromaticColors, "[AchromaticColor]");
        ///     // str == "[AchromaticColor] Red Blue Green Yellow [AchromaticColor] [AchromaticColor]"</code>
        /// </example>
        public static String Replace(this String str, String newValue, params String[] oldValues)
        {
            if (IsNullOrEmpty(str) || null == oldValues) return str;
            //return oldValues.Aggregate(new StringBuilder(str), (sb, s) => sb.Replace(s, newValue)).ToString();
            return Replace(str, oldValues, (item) => newValue);
        }

        /// <summary>
        ///   Replace all values in String
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="oldValues"> List of old values, which must be replaced </param>
        /// <param name="newValue"> New value for all old values </param>
        /// <returns> Returns new String with the replaced values </returns>
        /// <example>
        ///   <code>var str = "White Red Blue Green Yellow Black Gray";
        ///     var achromaticColors = new[] {"White", "Black", "Gray"};
        ///     str = str.ReplaceAll(achromaticColors, "[AchromaticColor]");
        ///     // str == "[AchromaticColor] Red Blue Green Yellow [AchromaticColor] [AchromaticColor]"</code>
        /// </example>
        public static String Replace(this String str, String newValue, IEnumerable<String> oldValues)
        {
            if (IsNullOrEmpty(str) || null == oldValues) return str;
            //var sb = new StringBuilder(str);
            //// ----------------------------------
            ////foreach (var oldValue in oldValues)
            ////    sb.Replace(oldValue, newValue);
            //// ----------------------------------
            //oldValues.ForEach((item) => sb.Replace(item, newValue));
            //// ----------------------------------
            //return sb.ToString();
            // ====================================
            //return oldValues.Aggregate(new StringBuilder(str), (sb, s) => sb.Replace(s, newValue)).ToString();
            return Replace(str, oldValues, (item) => newValue);
        }

        /// <summary>
        ///   Replace all values in String
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="oldValues"> List of old values, which must be replaced </param>
        /// <param name="newValues"> List of new values </param>
        /// <returns> Returns new String with the replaced values </returns>
        /// <example>
        ///   <code>var str = "White Red Blue Green Yellow Black Gray";
        ///     var achromaticColors = new[] {"White", "Black", "Gray"};
        ///     var exquisiteColors = new[] {"FloralWhite", "Bistre", "DavyGrey"};
        ///     str = str.ReplaceAll(achromaticColors, exquisiteColors);
        ///     // str == "FloralWhite Red Blue Green Yellow Bistre DavyGrey"</code>
        /// </example>
        public static String Replace(this String str, IEnumerable<String> oldValues, IEnumerable<String> newValues)
        {
            if (IsNullOrEmpty(str) || null == oldValues) return str;
            var sb = new StringBuilder(str);
            var newValuesEnum = newValues.GetEnumerator();
            foreach (var oldValue in oldValues)
            {
                if (!newValuesEnum.MoveNext())
                    throw new ArgumentOutOfRangeException("newValues",
                                                          "newValues sequence is shorter than oldValues sequence");
                sb.Replace(oldValue, newValuesEnum.Current);
            }
            if (newValuesEnum.MoveNext())
                throw new ArgumentOutOfRangeException("newValues",
                                                      "newValues sequence is longer than oldValues sequence");
            return sb.ToString();
        }

        /// <summary>
        ///   Uses regular expressions to replace parts of a String.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="regexPattern"> The regular expression pattern. </param>
        /// <param name="replaceValue"> The replacement value. </param>
        /// <param name="options"> The regular expression options. </param>
        /// <returns> The newly created String </returns>
        /// <example>
        ///   <code>var s = "12345";
        ///     var replaced = s.ReplaceWith(@"\d", m => String.Concat(" -", m.Value, "- "));</code>
        /// </example>
        public static String Replace(this String str, String regexPattern, String replaceValue,
                                     RegexOptions options = RegexOptions.None)
        {
            return Regex.Replace(str, regexPattern, replaceValue, options);
        }

        /// <summary>
        ///   Uses regular expressions to replace parts of a String.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="regexPattern"> The regular expression pattern. </param>
        /// <param name="options"> The regular expression options. </param>
        /// <param name="evaluator"> The replacement method / lambda expression. </param>
        /// <returns> The newly created String </returns>
        /// <example>
        ///   <code>var s = "12345";
        ///     var replaced = s.ReplaceWith(@"\d", m => String.Concat(" -", m.Value, "- "));</code>
        /// </example>
        public static String Replace(this String str, String regexPattern, MatchEvaluator evaluator,
                                     RegexOptions options = RegexOptions.None)
        {
            return Regex.Replace(str, regexPattern, evaluator, options);
        }
        #endregion

        #region Remove
        /// <summary>
        ///   Remove any instance of the given character from the current String.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="arrChar"> The remove char. </param>
        public static String Remove(this String str, params char[] arrChar)
        {
            if (IsNullOrEmpty(str) || null == arrChar) return str;
            // --------------------------------------------
            //arrChar.ForEach((c) => str = str.Remove(c.ToString(CultureInfo.InvariantCulture)));
            // --------------------------------------------
            arrChar.Aggregate(str, (s, c) => s.Replace(c.ToString(CultureInfo.InvariantCulture), String.Empty));
            // --------------------------------------------
            return str;
        }

        /// <summary>
        ///   Remove any instance of the given String pattern from the current String.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="arrStr"> The strings. </param>
        /// <returns> </returns>
        public static String Remove(this String str, params String[] arrStr)
        {
            //return arrStr.Aggregate(str, (ss, s) => ss.Replace(s, String.Empty));
            return Replace(str, String.Empty, arrStr);
        }

        /// <summary>
        ///   Remove any instance of the given String pattern from the current String.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="values"> The strings. </param>
        /// <returns> </returns>
        public static String Remove(this String str, IEnumerable<String> values)
        {
            return Replace(str, String.Empty, values);
        }

        /// <summary>
        ///   Uses regular expressions to replace parts of a String.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="regexPattern"> The regular expression pattern. </param>
        /// <param name="options"> The regular expression options. </param>
        /// <returns> The newly created String </returns>
        /// <example>
        ///   <code>var s = "12345";
        ///     var replaced = s.ReplaceWith(@"\d", m => String.Concat(" -", m.Value, "- "));</code>
        /// </example>
        public static String Remove(this String str, String regexPattern, RegexOptions options = RegexOptions.None)
        {
            return Replace(str, regexPattern, String.Empty, options);
        }
        #endregion

        #region Contains
        public static bool Contains(this String str, String subString, int startIndex = 0)
        {
            //foreach (var c in subString)
            //{
            //    if (startIndex >= str.Length || str[startIndex] != c) return false;
            //    ++startIndex;
            //}
            //return true;
            return IsNotNullOrWhiteSpace(str) &&
                   (str.IndexOf(subString, startIndex, StringComparison.InvariantCulture) != -1);
        }

        /// <summary>
        ///   Determines whether the comparison value string is contained within the input value String
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="subString"> The Sub-String. </param>
        /// <param name="comparisonType"> Type of the comparison to allow case sensitive or insensitive comparison. </param>
        /// <returns> <c>true</c> if String contains the Sub-String, otherwise, <c>false</c> . </returns>
        public static bool Contains(this String str, String subString, StringComparison comparisonType)
        {
            return IsNotNullOrWhiteSpace(str) && (str.IndexOf(subString, comparisonType) != -1);
        }

        /// <summary>
        ///   Determines whether the comparison value String is contained within the input value String without any
        ///   consideration about the case (<see cref="StringComparison.InvariantCultureIgnoreCase" />).
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="subString"> The Sub-String. </param>
        /// <returns> <c>true</c> if String contains the Sub-String (case insensitive), otherwise, <c>false</c> . </returns>
        public static bool ContainsEquivalence(this String str, String subString)
        {
            return IsNullOrWhiteSpace(str) && IsNullOrWhiteSpace(subString)
                   || Contains(str, subString, StringComparison.InvariantCultureIgnoreCase);
        }
        #endregion

        /// <summary>
        ///   Returns true if strings are equals, without consideration to case (<see
        ///   cref="StringComparison.InvariantCultureIgnoreCase" />)
        /// </summary>
        public static bool Equivalent(this String str, String whateverCaseString)
        {
            return String.Equals(str, whateverCaseString, StringComparison.InvariantCultureIgnoreCase);
        }

        // Like linq take - takes the first x characters
        public static String Take(this String str, int count)
        {
            if (count < 0) throw new ArgumentException("value can't be less then zero", "count");
            return (IsNotNullOrEmpty(str) && str.Length > count) ? str = str.Remove(count) : str;
        }

        //like linq skip - skips the first x characters and returns the remaining String
        public static String Skip(this String str, int count)
        {
            if (count < 0) throw new ArgumentException("value can't be less then zero", "count");
            return (IsNotNullOrEmpty(str) && str.Length > count) ? str.Substring(count) : String.Empty;
        }

        /// <summary>
        ///   Concatenates the specified String value with the passed additional strings.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="values"> The additional String values to be concatenated. </param>
        /// <returns> The concatenated String. </returns>
        public static String ConcatWith(this String str, params String[] values)
        {
            return String.Concat(str, String.Concat(values));
        }

        /// <summary>
        ///   Formats the value with the parameters using String.Format.
        /// </summary>
        /// <param name="format"> The Format. </param>
        /// <param name="parameters"> The parameters. </param>
        /// <returns> </returns>
        public static String FormatWith(this String format, params object[] parameters)
        {
            return String.Format(format, parameters);
        }

        /// <summary>
        ///   Centers a charters in this String, padding in both, left and right, by specified Unicode character,
        ///   for a specified total lenght.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="width"> The number of characters in the resulting String, equal to the number of original characters plus any additional padding characters. </param>
        /// <param name="padChar"> A Unicode padding character. </param>
        /// <param name="truncate"> Should get only the substring of specified width if String width is more than the specified width. </param>
        /// <returns> A new String that is equivalent to this instance, but center-aligned with as many paddingChar characters as needed to create a length of width paramether. </returns>
        public static String PadBoth(this String str, int width, char padChar, bool truncate = false)
        {
            var diff = width - str.Length;
            return ((diff == 0) || (diff < 0 && !(truncate)))
                       ? str
                       : (diff < 0)
                             ? str.Substring(0, width)
                             : str.PadLeft(width - diff/2, padChar).PadRight(width, padChar);
        }

        /// <summary>
        ///   Repeats the specified String value as provided by the repeat count.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="repeatCount"> The repeat count. </param>
        /// <returns> The repeated String </returns>
        public static String Repeat(this String str, int repeatCount)
        {
            str.ExceptionIfNull("str");
            if (str.Length == 1) return new String(str[0], repeatCount);
            var sb = new StringBuilder(repeatCount*str.Length);
            //while (repeatCount-- > 0) sb.Append(str);
            //-----------------------------------------
            repeatCount.Times(() => sb.Append(str));
            return sb.ToString();
        }

        /// <summary>
        ///   Extracts all digits from a String.
        /// </summary>
        /// <param name="str"> String containing digits to extract </param>
        /// <returns> All digits contained within the input String </returns>
        /// <remarks>
        ///   Contributed by Kenneth Scott
        /// </remarks>
        public static String ExtractDigits(this String str)
        {
            str.ExceptionIfNull("string can't be null", "str");
            return str.Where(Char.IsDigit).Aggregate(new StringBuilder(str.Length), (sb, c) => sb.Append(c)).ToString();
        }

        #region Get String
        /// <summary>
        ///   Gets the String before the given String parameter.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="subString"> The given String parameter. </param>
        /// <returns> </returns>
        /// <remarks>
        ///   Unlike GetBetween and GetAfter, this does not Trim the result.
        /// </remarks>
        public static String GetBefore(this String str, String subString)
        {
            var index = str.IndexOf(subString, StringComparison.Ordinal);
            return (index == -1) ? String.Empty : str.Substring(0, index);
        }

        /// <summary>
        ///   Gets the String between the given String parameters.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="sub1"> The left String sentinel. </param>
        /// <param name="sub2"> The right String sentinel </param>
        /// <returns> </returns>
        /// <remarks>
        ///   Unlike GetBefore, this method trims the result
        /// </remarks>
        public static String GetBetween(this String str, String sub1, String sub2)
        {
            var index1 = str.IndexOf(sub1, StringComparison.Ordinal);
            var index2 = str.LastIndexOf(sub2, StringComparison.Ordinal);
            if (index1 == -1 || index1 == -1) return String.Empty;
            var startIndex = sub1.Length + index1;
            return (startIndex >= index2) ? String.Empty : str.Substring(startIndex, index2 - startIndex).Trim();
        }

        /// <summary>
        ///   Gets the String after the given String parameter.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="subString"> The given String parameter. </param>
        /// <returns> </returns>
        /// <remarks>
        ///   Unlike GetBefore, this method trims the result
        /// </remarks>
        public static String GetAfter(this String str, String subString)
        {
            var index = str.LastIndexOf(subString, StringComparison.Ordinal);
            if (index == -1) return String.Empty;
            var startIndex = subString.Length + index;
            return (startIndex >= str.Length) ? String.Empty : str.Substring(startIndex).Trim();
        }
        #endregion

        #region Substring
        /// <summary>
        ///   Returns the right part of the String from index.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="index"> The start index for substringing. </param>
        /// <returns> The right part. </returns>
        public static String SubstringFrom(this String str, int index)
        {
            return (index < 0) ? str : str.Substring(index, str.Length - index);
        }

        /// <summary>
        ///   Returns the right part of the String from index.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="index"> The start index for substringing. </param>
        /// <returns> The right part. </returns>
        public static String SubstringTo(this String str, int index)
        {
            return (index < 0) ? str : str.Substring(0, index + 1);
        }
        #endregion

        /// <summary>
        ///   Uppercase First Letter
        /// </summary>
        /// <param name="str"> The String. </param>
        public static String ToUpperFirstLetter(this String str)
        {
            if (IsNullOrWhiteSpace(str)) return String.Empty;
            var valueChars = str.ToCharArray();
            valueChars[0] = Char.ToUpper(valueChars[0]);
            return new String(valueChars);
        }

        public static String ToPlural(this String singular)
        {
            // Multiple words in the form A of B : Apply the plural to the first word only (A)
            var index = singular.LastIndexOf(" of ", StringComparison.Ordinal);
            if (index > 0) return (singular.Substring(0, index)) + singular.Remove(0, index).ToPlural();
            // single Word rules
            //sibilant ending rule
            if (singular.EndsWith("sh")) return singular + "es";
            if (singular.EndsWith("ch")) return singular + "es";
            if (singular.EndsWith("us")) return singular + "es";
            if (singular.EndsWith("ss")) return singular + "es";
            //-ies rule
            if (singular.EndsWith("y")) return singular.Remove(singular.Length - 1, 1) + "ies";
            // -oes rule
            if (singular.EndsWith("o")) return singular.Remove(singular.Length - 1, 1) + "oes";
            // -s suffix rule
            return singular + "s";
        }

        #region Chop
        //[ObsoleteAttribute("This method is obsolute. Use 3 parameter Chop method")]
        public static String Chop(this String str, int length, String ending = default(String))
        {
            if (length <= 0) throw new ArgumentException("value can't be less then one", "length");
            if (ending.IsNotNullOrEmpty() && length < ending.Length)
                throw new Exception("Failed to reduce to less then endings length.");
            if (str.Length > length)
            {
                var startIndex = length - (ending.IsNullOrEmpty() ? 0 : ending.Length);
                return str.Substring(0, startIndex) + (ending ?? String.Empty);
            }
            return str;
        }

        public static String Chop(this String str, int length, int maxLength = 5, String ending = default(String))
        {
            if (length <= 0) throw new ArgumentException("value can't be less then one", "length");
            if (ending.IsNotNullOrEmpty() && length < ending.Length)
                throw new Exception("Failed to reduce to less then endings length.");
            if (str.Length > length)
            {
                var strChop = String.Empty;
                foreach (var strItem in str.Split(' '))
                {
                    if (strChop.Length > length)
                        break;
                    var splitLength = strItem.Length;
                    if ((strChop.Length + splitLength) > length)
                    {
                        strChop += strItem.Substring(0, Math.Min(maxLength, splitLength));
                        break;
                    }
                    strChop += (strItem + ' ');
                }
                if (strChop.Length < str.Length)
                    return strChop += (ending ?? String.Empty);
            }
            return str;
        }

        //public static string Chop(this string s, int length, int singleWordMaximumLength)
        //{
        //    if (s.IsEmpty())
        //        return "";
        //    if (s.Length > length)
        //    {
        //        string[] split = s.Split(' ');
        //        int splitIndex = split[0].Length;
        //        string splitTemp = split[0];
        //        if (split[0].Length > singleWordMaximumLength)
        //            return split[0].Substring(0, singleWordMaximumLength) + "...";

        //        int i;
        //        for (i = 1; i < split.Length; i++)
        //        {
        //            if (splitIndex > length)
        //            {
        //                break;
        //            }
        //            if ((split[i]).Length > singleWordMaximumLength)
        //            {
        //                splitTemp += " " + split[i].Substring(0, singleWordMaximumLength);
        //                return splitTemp + "...";
        //            }
        //            splitTemp += " " + split[i];
        //            splitIndex += split[i].Length;
        //        }

        //        if (i == split.Length)
        //            return s;
        //        else
        //            return splitTemp + "...";
        //    }
        //    return s;
        //}
        #endregion

        /// <summary>
        ///   Uses regular expressions to determine if the String matches to a given regex pattern.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="regexPattern"> The regular expression pattern. </param>
        /// <param name="options"> The regular expression options. </param>
        /// <returns> <c>true</c> if the value is matching to the specified pattern; otherwise, <c>false</c> . </returns>
        /// <example>
        ///   <code>var s = "12345";
        ///     var isMatching = s.IsMatchingTo(@"^\d+$");</code>
        /// </example>
        public static bool IsMatchingTo(this String str, String regexPattern, RegexOptions options)
        {
            return Regex.IsMatch(str, regexPattern, options);
        }

        /// <summary>
        ///   Uses regular expressions to determine if the String matches to a given regex pattern.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="regexPattern"> The regular expression pattern. </param>
        /// <returns> <c>true</c> if the value is matching to the specified pattern; otherwise, <c>false</c> . </returns>
        /// <example>
        ///   <code>var s = "12345";
        ///     var isMatching = s.IsMatchingTo(@"^\d+$");</code>
        /// </example>
        public static bool IsMatchingTo(this String str, String regexPattern)
        {
            return IsMatchingTo(str, regexPattern, RegexOptions.None);
        }

        /// <summary>
        ///   Splits the given String into words and returns a String array.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <returns> The splitted String array </returns>
        public static String[] GetWords(this String str)
        {
            return
                //str.Split(new char[] { ' ', '.', '?' });
                str.Split(@"\W");
        }

        public static int GetWordsCount(this String str)
        {
            return str.GetWords().Length;
        }

        public static String SingleSpaces(this String str)
        {
            return Regex.Replace(str, @"\s+", " ");
        }

        public static String RemoveSpaces(this String str)
        {
            //return str.Replace(" ", String.Empty);
            return Regex.Replace(str, @"\s+", String.Empty);
        }

        public static String RemoveNonNumeric(this String str)
        {
            if (IsNotNullOrEmpty(str))
            {
                // -----------------------------------------
                //char[] chars = new char[str.Length];
                //int length = 0;
                //foreach (char c in str)
                //    if (Char.IsNumber(c))
                //        chars[length++] = c;
                //str = (length == 0) ?
                //        String.Empty :
                //        (str.Length == length) ?
                //            str :
                //            new String(chars, 0, length);
                // -----------------------------------------
                var sb = new StringBuilder();
                // --------------------------------------
                foreach (Match match in Regex.Matches(str, "[0-9]"))
                    sb.Append(match.Value);
                // --------------------------------------
                //foreach (char c in str)
                //{
                //    if (Char.IsNumber(c))
                //    {
                //        sb.Append(c);
                //    }
                //}
                // --------------------------------------
                return sb.ToString();
            }
            return str;
        }

        /// <summary>
        ///   Gets the nth "word" of a given String, where "words" are substrings separated by a given separator
        /// </summary>
        /// <param name="str"> The String from which the word should be retrieved. </param>
        /// <param name="index"> Index of the word (0-based). </param>
        /// <returns> The word at position n of the String. Trying to retrieve a word at a position lower than 0 or at a position where no word exists results in an exception. </returns>
        /// <remarks>
        ///   Originally contributed by MMathews
        /// </remarks>
        public static String GetWordByIndex(this String str, int index)
        {
            var words = str.GetWords();
            if ((index < 0) || (index > words.Length - 1))
                throw new IndexOutOfRangeException("The word number is out of range.");
            return words[index];
        }

        /// <summary>
        ///   Removed all special characters from the String.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <returns> The adjusted String. </returns>
        [Obsolete("Please use RemoveAllSpecialCharacters instead")]
        public static String AdjustString(this String str)
        {
            return String.Join(null, Regex.Split(str, "[^a-zA-Z0-9]"));
        }

        /// <summary>
        ///   Removed all special characters from the String.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <returns> The adjusted String. </returns>
        /// <remarks>
        ///   This implementation is roughly equal to the original in speed, and should be more robust, internationally.
        /// </remarks>
        public static String RemoveAllSpecialCharacters(this String str)
        {
            //var sb = new StringBuilder(str.Length);
            //foreach (var c in str.Where(Char.IsLetterOrDigit))
            //    sb.Append(c);
            //return sb.ToString();
            return
                str.Where(Char.IsLetterOrDigit).Aggregate(new StringBuilder(str.Length), (sb, c) => sb.Append(c)).
                    ToString();
        }

        /// <summary>
        ///   Add space on every upper character
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <returns> The adjusted String. </returns>
        public static String SpaceOnUpper(this String str)
        {
            return Regex.Replace(str, "([A-Z])(?=[a-z])|(?<=[a-z])([A-Z]|[0-9]+)", " $1$2").TrimStart();
        }

        /// <summary>
        ///   Joins  the values of a String array if the values are not null or empty.
        /// </summary>
        /// <param name="arrStr"> The String array used for joining. </param>
        /// <param name="separator"> The separator to use in the joined String. </param>
        /// <returns> </returns>
        public static String JoinNotNullOrEmpty(this String[] arrStr, String separator)
        {
            //var items = new List<String>();
            //foreach (var str in arrStr)
            //    if (str.IsNotEmpty())
            //        items.Add(str);
            //return String.Join(separator, items.ToArray());
            return String.Join(separator, arrStr.Where(IsNotNullOrEmpty).ToArray());
        }

        /// <summary>
        ///   Parses the commandline params.
        /// </summary>
        /// <param name="arrStr"> The value. </param>
        /// <returns> A StringDictionary type object of command line parameters. </returns>
        public static StringDictionary ParseCommandlineParams(this String[] arrStr)
        {
            var parameters = new StringDictionary();
            var spliter = new Regex(@"^-{1,2}|^/|=|:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var remover = new Regex(@"^['""]?(.*?)['""]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var parameter = default(String);
            // Valid parameters forms:
            // {-,/,--}param{ ,=,:}((",')value(",'))
            // Examples: -param1 value1 --param2 /param3:"Test-:-work" /param4=happy -param5 '--=nice=--'
            foreach (var parts in arrStr.Select(str => spliter.Split(str, 3)))
                switch (parts.Length)
                {
                        // Found a value (for the last parameter found (space separator))
                    case 1:
                        if (parameter.IsNotNullOrEmpty())
                        {
                            if (!parameters.ContainsKey(parameter))
                            {
                                parts[0] = remover.Replace(parts[0], "$1");
                                parameters.Add(parameter, parts[0]);
                            }
                            parameter = default(String);
                        }
                        // else Error: no parameter waiting for a value (skipped)
                        break;
                        // Found just a parameter
                    case 2:
                        // The last parameter is still waiting. With no value, set it to true.
                        if (parameter.IsNotNullOrEmpty())
                            if (!parameters.ContainsKey(parameter)) parameters.Add(parameter, "true");
                        parameter = parts[1];
                        break;
                        // Parameter with enclosed value
                    case 3:
                        // The last parameter is still waiting. With no value, set it to true.
                        if (parameter.IsNotNullOrEmpty())
                            if (!parameters.ContainsKey(parameter)) parameters.Add(parameter, "true");
                        parameter = parts[1];
                        // Remove possible enclosing characters (",')
                        if (!parameters.ContainsKey(parameter))
                        {
                            parts[2] = remover.Replace(parts[2], "$1");
                            parameters.Add(parameter, parts[2]);
                        }
                        parameter = default(String);
                        break;
                }
            // In case a parameter is still waiting
            if (parameter.IsNotNullOrEmpty() && !parameters.ContainsKey(parameter)) parameters.Add(parameter, "true");
            return parameters;
        }

        /// <summary>
        ///   Calculates the SHA1 hash of the supplied String and returns a base 64 String.
        /// </summary>
        /// <param name="stringToHash"> String that must be hashed. </param>
        /// <returns> The hashed String or null if hashing failed. </returns>
        /// <exception cref="ArgumentException">Occurs when stringToHash or key is null or empty.</exception>
        public static String GetSHA1Hash(this String stringToHash)
        {
            if (stringToHash.IsNullOrEmpty()) return null;
            //{
            //    throw new ArgumentException("An empty String value cannot be hashed.");
            //}
            var data = Encoding.UTF8.GetBytes(stringToHash);
            var hash = new SHA1CryptoServiceProvider().ComputeHash(data);
            return Convert.ToBase64String(hash);
        }

        public static String[] Explode(this String str, int size)
        {
            // Number of segments exploded to except last.
            var count = str.Length/size;
            // Determine if we need to store a final segment.
            // ... Sometimes we have a partial segment.
            var final = (size*count) < str.Length;
            // Allocate the array to return.
            // ... The size varies depending on if there is a final fragment.
            var result = final ? new String[count + 1] : new String[count];
            // Loop through each index and take a substring.
            // ... The starting index is computed with multiplication.
            for (var i = 0; i < count; i++)
                result[i] = str.Substring((i*size), size);
            // Sometimes we need to set the final String fragment.
            if (final) result[result.Length - 1] = str.Substring(count*size);
            return result;
        }

        public static int IndexOfOccurence(this String str, String subStr, int occurrence)
        {
            var countOcc = 0;
            var index = -1; // index of parsed String
            do
            {
                index = str.IndexOf(subStr, index + subStr.Length, StringComparison.Ordinal);
                if (index == -1) break;
                ++countOcc;
            } while (countOcc != occurrence);
            return index;
        }

        public static T As<T>(this String str)
        {
            return (T) TypeDescriptor.GetConverter(typeof (T)).ConvertFromString(str);
        }

        #region ExtractArguments extension
        /// <summary>
        ///   Options to match the template with the original String
        /// </summary>
        public enum ComparsionTemplateOptions
        {
            /// <summary>
            ///   Free template comparsion
            /// </summary>
            Default,

            /// <summary>
            ///   Template compared from beginning of input String
            /// </summary>
            FromStart,

            /// <summary>
            ///   Template compared with the end of input String
            /// </summary>
            AtTheEnd,

            /// <summary>
            ///   Template compared whole with input String
            /// </summary>
            Whole,
        }

        static readonly String[] _ReservedRegexOperators = new[] {@"\", "^", "$", "*", "+", "?", ".", "(", ")"};

        static String GetRegexPattern(String template, ComparsionTemplateOptions compareTemplateOptions)
        {
            template = template.Replace(_ReservedRegexOperators, v => String.Concat(@"\", v));
            var comparingFromStart = compareTemplateOptions == ComparsionTemplateOptions.FromStart ||
                                     compareTemplateOptions == ComparsionTemplateOptions.Whole;
            var comparingAtTheEnd = compareTemplateOptions == ComparsionTemplateOptions.AtTheEnd ||
                                    compareTemplateOptions == ComparsionTemplateOptions.Whole;
            var sbPattern = new StringBuilder();
            if (comparingFromStart) sbPattern.Append("^");
            sbPattern.Append(Regex.Replace(template, @"\{[0-9]+\}",
                                           (match) =>
                                               {
                                                   var argNum = match.ToString().Replace("{", "").Replace("}", "");
                                                   return String.Format("(?<{0}>.*?)", int.Parse(argNum) + 1);
                                               }));
            return comparingAtTheEnd ||
                   (template.LastOrDefault() == '}' && compareTemplateOptions == ComparsionTemplateOptions.Default)
                       ? sbPattern.Append("$").ToString()
                       : sbPattern.ToString();
        }

        /// <summary>
        ///   Extract arguments as regex groups from String by template
        /// </summary>
        /// <param name="value"> The String. For example, "My name is Aleksey". </param>
        /// <param name="template"> Template with arguments in the format {№}. For example, "My name is {0} {1}.". </param>
        /// <param name="compareTemplateOptions"> Template options for compare with input String. </param>
        /// <param name="regexOptions"> Regex options. </param>
        /// <returns> Returns parsed arguments as regex groups. </returns>
        /// <example>
        ///   <code>var str = "My name is Aleksey Nagovitsyn. I'm from Russia.";
        ///     var groupArgs = str.ExtractGroupArguments(@"My name is {1} {0}. I'm from {2}.");
        ///     // groupArgs[i].Value is [Nagovitsyn, Aleksey, Russia]</code>
        /// </example>
        public static IEnumerable<Group> ExtractGroupArguments(this String value,
                                                               String template,
                                                               ComparsionTemplateOptions compareTemplateOptions =
                                                                   ComparsionTemplateOptions.Default,
                                                               RegexOptions regexOptions = RegexOptions.None)
        {
            var pattern = GetRegexPattern(template, compareTemplateOptions);
            var regex = new Regex(pattern, regexOptions);
            var match = regex.Match(value);
            return Enumerable.Cast<Group>(match.Groups).Skip(1);
        }

        /// <summary>
        ///   Extract arguments from String by template
        /// </summary>
        /// <param name="str"> The String. For example, "My name is Aleksey". </param>
        /// <param name="template"> Template with arguments in the format {№}. For example, "My name is {0} {1}.". </param>
        /// <param name="compareTemplateOptions"> Template options for compare with input String. </param>
        /// <param name="regexOptions"> Regex options. </param>
        /// <returns> Returns parsed arguments. </returns>
        /// <example>
        ///   <code>var str = "My name is Aleksey Nagovitsyn. I'm from Russia.";
        ///     var args = str.ExtractArguments(@"My name is {1} {0}. I'm from {2}.");
        ///     // args[i] is [Nagovitsyn, Aleksey, Russia]</code>
        /// </example>
        public static IEnumerable<String> ExtractArguments(this String str,
                                                           String template,
                                                           ComparsionTemplateOptions compareTemplateOptions =
                                                               ComparsionTemplateOptions.Default,
                                                           RegexOptions regexOptions = RegexOptions.None)
        {
            return ExtractGroupArguments(str, template, compareTemplateOptions, regexOptions).Select(g => g.Value);
        }
        #endregion ExtractArguments extension

        #region Encode & Decode
        /// <summary>
        ///   Converts the String to a byte-array using the supplied encoding
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="encoding"> The encoding to be used. </param>
        /// <returns> The created byte array </returns>
        /// <example>
        ///   <code>var value = "Hello World";
        ///     var ansiBytes = value.ToBytes(Encoding.GetEncoding(1252)); // 1252 = ANSI
        ///     var utf8Bytes = value.ToBytes(Encoding.UTF8);</code>
        /// </example>
        public static byte[] ToBytes(this String str, Encoding encoding = default(Encoding))
        {
            return (encoding ?? Encoding.Default).GetBytes(str);
        }

        /// <summary>
        ///   Encodes the input value to a Base64 String using the supplied encoding.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="encoding"> The encoding. </param>
        /// <returns> The Base 64 encoded String </returns>
        public static String EncodeBase64(this String str, Encoding encoding = default(Encoding))
        {
            return Convert.ToBase64String((encoding ?? Encoding.UTF8).GetBytes(str));
        }

        /// <summary>
        ///   Decodes a Base 64 encoded value to a String using the supplied encoding.
        /// </summary>
        /// <param name="strEncoded"> The Base 64 encoded value. </param>
        /// <param name="encoding"> The encoding. </param>
        /// <returns> The decoded String </returns>
        public static String DecodeBase64(this String strEncoded, Encoding encoding = default(Encoding))
        {
            return (encoding ?? Encoding.UTF8).GetString(Convert.FromBase64String(strEncoded));
        }
        #endregion

        #region Guid
        /// <summary>
        ///   Convert the provided String to a Guid value and returns the provided default value if the conversion fails.
        /// </summary>
        /// <param name="value"> The original String value. </param>
        /// <param name="defaultValue"> The default value. </param>
        /// <returns> The Guid </returns>
        public static Guid ToGuidSave(this String value, Guid defaultValue)
        {
            if (value.IsNullOrEmpty()) return defaultValue;
            try
            {
                return value.ToGuid();
            }
            catch
            {
            }
            return defaultValue;
        }

        /// <summary>
        ///   Convert the provided String to a Guid value and returns Guid.Empty if conversion fails.
        /// </summary>
        /// <param name="value"> The original String value. </param>
        /// <returns> The Guid </returns>
        public static Guid ToGuidSave(this String value)
        {
            return value.ToGuidSave(Guid.Empty);
        }

        /// <summary>
        ///   Convert the provided String to a Guid value.
        /// </summary>
        /// <param name="value"> The original String value. </param>
        /// <returns> The Guid </returns>
        public static Guid ToGuid(this String value)
        {
            return new Guid(value);
        }
        #endregion

        #region XML
        /// <summary>
        ///   Loads the String into a LINQ to XML XDocument
        /// </summary>
        /// <param name="xml"> The XML String. </param>
        /// <returns> The XML document object model (XDocument) </returns>
        public static XDocument ToXDocument(this String xml)
        {
            return XDocument.Parse(xml);
        }

        /// <summary>
        ///   Loads the String into a XML DOM object (XmlDocument)
        /// </summary>
        /// <param name="xml"> The XML String. </param>
        /// <returns> The XML document object model (XmlDocument) </returns>
        public static XmlDocument ToXmlDOM(this String xml)
        {
            var document = new XmlDocument();
            document.LoadXml(xml);
            return document;
        }

        /// <summary>
        ///   Loads the String into a XML XPath DOM (XPathDocument)
        /// </summary>
        /// <param name="xml"> The XML String. </param>
        /// <returns> The XML XPath document object model (XPathNavigator) </returns>
        public static XPathNavigator ToXPath(this String xml)
        {
            var document = new XPathDocument(new StringReader(xml));
            return document.CreateNavigator();
        }

        /// <summary>
        ///   Loads the String into a LINQ to XML XElement
        /// </summary>
        /// <param name="xml"> The XML String. </param>
        /// <returns> The XML element object model (XElement) </returns>
        public static XElement ToXElement(this String xml)
        {
            return XElement.Parse(xml);
        }
        #endregion

        #region Html
        //ditches html tags - note it doesnt get rid of things like &nbsp;
        public static String StripHtml(this String html)
        {
            return html.IsNullOrEmpty() ? String.Empty : Regex.Replace(html, @"<[^>]*>", String.Empty);
        }

        /// <summary>
        ///   Encodes the email address so that the link is still valid, but the email address is useless for email harvsters.
        /// </summary>
        /// <param name="HtmlEncode"> The email address. </param>
        /// <returns> </returns>
        public static String EncodeEmailAddress(this String HtmlEncode)
        {
            HtmlEncode.ExceptionIfNull("HtmlEncode");
            for (var i = HtmlEncode.Length; i >= 1; i--)
            {
                var acode = Convert.ToInt32(HtmlEncode[i - 1]);
                var replace = String.Empty;
                switch (acode)
                {
                    case 32:
                        replace = " ";
                        break;
                    case 34:
                        replace = "\"";
                        break;
                    case 38:
                        replace = "&";
                        break;
                    case 60:
                        replace = "<";
                        break;
                    case 62:
                        replace = ">";
                        break;
                    default:
                        replace = (acode >= 32 && acode <= 127)
                                      ? Convert.ToString(acode) //String.Concat("&#", Convert.ToString(acode), ";")
                                      : String.Concat("&#", Convert.ToString(acode), ";");
                        break;
                }
                if (replace.IsNotNullOrEmpty())
                    HtmlEncode = String.Concat(HtmlEncode.Substring(0, i - 1), replace, HtmlEncode.Substring(i));
            }
            return HtmlEncode;
        }

        #region HtmlSafe
        /// <summary>
        ///   Makes the current instance HTML safe.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="all"> Whether to make all characters entities or just those needed. </param>
        /// <param name="replace"> Whether or not to encode spaces and line breaks. </param>
        /// <returns> An HTML safe String. </returns>
        public static String ToHtmlSafe(this String str, bool all = false, bool replace = false)
        {
            if (IsNullOrWhiteSpace(str)) return String.Empty;
            var entities = new[]
                               {
                                   0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23,
                                   24
                                   , 25, 26, 28, 29, 30, 31, 34, 39, 38, 60, 62, 123, 124, 125, 126, 127, 160, 161, 162,
                                   163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179,
                                   180
                                   , 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 215, 247, 192, 193, 194, 195
                                   ,
                                   196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212,
                                   213
                                   , 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230
                                   ,
                                   231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247,
                                   248
                                   , 249, 250, 251, 252, 253, 254, 255, 256, 8704, 8706, 8707, 8709, 8711, 8712, 8713,
                                   8715
                                   , 8719, 8721, 8722, 8727, 8730, 8733, 8734, 8736, 8743, 8744, 8745, 8746, 8747, 8756,
                                   8764, 8773, 8776, 8800, 8801, 8804, 8805, 8834, 8835, 8836, 8838, 8839, 8853, 8855,
                                   8869
                                   , 8901, 913, 914, 915, 916, 917, 918, 919, 920, 921, 922, 923, 924, 925, 926, 927,
                                   928,
                                   929, 931, 932, 933, 934, 935, 936, 937, 945, 946, 947, 948, 949, 950, 951, 952, 953,
                                   954
                                   , 955, 956, 957, 958, 959, 960, 961, 962, 963, 964, 965, 966, 967, 968, 969, 977, 978
                                   ,
                                   982, 338, 339, 352, 353, 376, 402, 710, 732, 8194, 8195, 8201, 8204, 8205, 8206, 8207
                                   ,
                                   8211, 8212, 8216, 8217, 8218, 8220, 8221, 8222, 8224, 8225, 8226, 8230, 8240, 8242,
                                   8243
                                   , 8249, 8250, 8254, 8364, 8482, 8592, 8593, 8594, 8595, 8596, 8629, 8968, 8969, 8970,
                                   8971, 9674, 9824, 9827, 9829, 9830
                               };
            var sb = new StringBuilder();
            //foreach (var c in str)
            //{
            //    sb.Append((all || entities.Contains(c))
            //                  ? String.Concat("&#", ((int) c), ";")
            //                  : c.ToString());
            //}
            str.ForEach((c) => sb.Append((all || entities.Contains(c))
                                             ? String.Concat("&#", ((int) c), ";")
                                             : c.ToString(CultureInfo.InvariantCulture)));
            return replace
                       ? sb.Replace("", "<br />").Replace("\n", "<br />").Replace(" ", "&nbsp;").ToString()
                       : sb.ToString();
        }
        #endregion

        #endregion

        #region SecureString
        /// <summary>
        ///   Converts a regular string into SecureString
        /// </summary>
        /// <param name="str"> String value. </param>
        /// <param name="makeReadOnly"> Makes the text value of this secure string read-only. </param>
        /// <returns> Returns a SecureString containing the value of a transformed object. </returns>
        public static SecureString ToSecureString(this String str, bool makeReadOnly = true)
        {
            if (str.IsNull()) return default(SecureString);
            var secureString = new SecureString();
            //foreach (var c in str)
            //    secureString.AppendChar(c);
            str.ForEach(secureString.AppendChar);
            if (makeReadOnly) secureString.MakeReadOnly();
            return secureString;
        }
        #endregion

        #region Enum
        /// <summary>
        ///   Parse a String to a enum item if that String exists in the enum otherwise return the default enum item.
        /// </summary>
        /// <typeparam name="T"> The Enum type. </typeparam>
        /// <param name="str"> The String will use to convert into give enum </param>
        /// <param name="ignorecase"> Whether the enum parser will ignore the given data's case or not. </param>
        /// <returns> Converted enum. </returns>
        /// <example>
        ///   <code>public enum EnumTwo {  None, One,}
        ///     object[] items = new object[] { "One".ParseStringToEnum
        ///     <EnumOne />
        ///     (), "Two".ParseStringToEnum
        ///     <EnumTwo />
        ///     () };</code>
        /// </example>
        public static T ParseStringToEnum<T>(this String str, bool ignorecase = default(bool))
            where T : struct
        {
            return str.IsItemInEnum<T>()() ? default(T) : (T) Enum.Parse(typeof (T), str, default(bool));
        }

        /// <summary>
        ///   To check whether the data is defined in the given enum.
        /// </summary>
        /// <typeparam name="T"> The enum will use to check, the data defined. </typeparam>
        /// <param name="str"> The String match against enum. </param>
        /// <returns> Anonoymous method for the condition. </returns>
        public static Func<bool> IsItemInEnum<T>(this String str)
            where T : struct
        {
            return () => (str.IsNotNullOrEmpty() || !Enum.IsDefined(typeof (T), str));
        }
        #endregion

        #region Encrypt Decrypt
        /// <summary>
        ///   Encrypt this String into a byte array.
        /// </summary>
        /// <param name="strPlain"> The String being extended and that will be encrypted. </param>
        /// <param name="password"> The password to use then encrypting the String. </param>
        /// <returns> </returns>
        /// <remarks>
        /// </remarks>
        public static byte[] EncryptToByteArray(this String strPlain, String password)
        {
            var asciiEncoder = new ASCIIEncoding();
            var bytesPlain = asciiEncoder.GetBytes(strPlain);
            return CryptBytes(password, bytesPlain, true);
        }

        /// <summary>
        ///   Encrypt this String and return the result as a String of hexadecimal characters.
        /// </summary>
        /// <param name="strPlain"> The String being extended and that will be encrypted. </param>
        /// <param name="password"> The password to use then encrypting the String. </param>
        /// <returns> </returns>
        /// <remarks>
        /// </remarks>
        public static String EncryptToString(this String strPlain, String password)
        {
            return EncryptToByteArray(strPlain, password).BytesToHexString();
        }

        /// <summary>
        ///   Decrypt the encryption stored in this String of hexadecimal values.
        /// </summary>
        /// <param name="strEncrypted"> The hexadecimal String to decrypt. </param>
        /// <param name="password"> The password to use then encrypting the String. </param>
        /// <returns> </returns>
        /// <remarks>
        /// </remarks>
        public static String DecryptFromString(this String strEncrypted, String password)
        {
            return HexStringToBytes(strEncrypted).DecryptFromByteArray(password);
        }

        /// <summary>
        ///   Convert this String containing hexadecimal into a byte array.
        /// </summary>
        /// <param name="str"> The hexadecimal String to convert. </param>
        /// <returns> </returns>
        /// <remarks>
        /// </remarks>
        public static byte[] HexStringToBytes(this String str)
        {
            str = str.Replace(" ", String.Empty);
            var maxBytes = str.Length/2 - 1;
            var bytes = new byte[maxBytes + 1];
            for (var i = 0; i <= maxBytes; ++i)
                bytes[i] = byte.Parse(str.Substring(2*i, 2), NumberStyles.AllowHexSpecifier);
            return bytes;
        }
        #endregion

        #region ReplaceWith
        #endregion

        #region GetMatches
        /// <summary>
        ///   Uses regular expressions to determine all matches of a given regex pattern.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="regexPattern"> The regular expression pattern. </param>
        /// <param name="options"> The regular expression options. </param>
        /// <returns> A collection of all matches </returns>
        public static MatchCollection GetMatches(this String str, String regexPattern,
                                                 RegexOptions options = RegexOptions.None)
        {
            return Regex.Matches(str, regexPattern, options);
        }

        ///// <summary>
        ///// 	Uses regular expressions to determine all matches of a given regex pattern.
        ///// </summary>
        ///// <param name = "str">The String.</param>
        ///// <param name = "regexPattern">The regular expression pattern.</param>
        ///// <returns>A collection of all matches</returns>
        //public static MatchCollection GetMatches(this String str, String regexPattern)
        //{
        //    return GetMatches(str, regexPattern, RegexOptions.None);
        //}

        /// <summary>
        ///   Uses regular expressions to determine all matches of a given regex pattern and returns them as String enumeration.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="regexPattern"> The regular expression pattern. </param>
        /// <param name="options"> The regular expression options. </param>
        /// <returns> An enumeration of matching strings </returns>
        /// <example>
        ///   <code>var s = "12345";
        ///     foreach(var number in s.GetMatchingValues(@"\d")) {
        ///     Console.WriteLine(number);
        ///     }</code>
        /// </example>
        public static IEnumerable<String> GetMatchingValues(this String str, String regexPattern,
                                                            RegexOptions options = RegexOptions.None)
        {
            //foreach (Match match in GetMatches(str, regexPattern, options))
            //{
            //    if (match.Success) yield return match.Value;
            //}
            return (GetMatches(str, regexPattern, options) as IEnumerable<Match>)
                .Where(match => match.Success)
                .Select(match => match.Value);
        }

        ///// <summary>
        ///// 	Uses regular expressions to determine all matches of a given regex pattern and returns them as String enumeration.
        ///// </summary>
        ///// <param name = "str">The String.</param>
        ///// <param name = "regexPattern">The regular expression pattern.</param>
        ///// <returns>An enumeration of matching strings</returns>
        ///// <example>
        ///// 	<code>
        ///// 		var s = "12345";
        ///// 		foreach(var number in s.GetMatchingValues(@"\d")) {
        ///// 		Console.WriteLine(number);
        ///// 		}
        ///// 	</code>
        ///// </example>
        //public static IEnumerable<String> GetMatchingValues(this String str, String regexPattern)
        //{
        //    return GetMatchingValues(str, regexPattern, RegexOptions.None);
        //}
        #endregion

        #region Split
        /// <summary>
        ///   Uses regular expressions to split a String into parts.
        /// </summary>
        /// <param name="str"> The String. </param>
        /// <param name="regexPattern"> The regular expression pattern. </param>
        /// <param name="options"> The regular expression options. </param>
        /// <returns> The splitted String array </returns>
        public static String[] Split(this String str, String regexPattern, RegexOptions options = RegexOptions.None)
        {
            return Regex.Split(str, regexPattern, options);
        }

        /*
        public static String[] Split(this String str, String separator)
        {
            if (!String.IsNullOrWhiteSpace(str) &&
                !String.IsNullOrEmpty(separator) &&
                str.IndexOf(separator) != -1
                )
            {
                List<String> lstSplit = new List<String>();
                //int count = 1;
                int index;
                while ((index = str.IndexOf(separator)) != -1)
                {
                    String split = str.Substring(0, index);
                    lstSplit.Add(split);
                    str = str.Remove(0, index + separator.Length);
                    //++count;
                }
                lstSplit.Add(str);
                return lstSplit.ToArray();
            }
            return new[] { str };
        }
        */
        #endregion

        #region Static Methods
        /// <summary>
        ///   A generic version of System.String.Join()
        /// </summary>
        /// <typeparam name="T"> The type of the array to join </typeparam>
        /// <param name="separator"> The separator to appear between each element </param>
        /// <param name="value"> An array of values </param>
        /// <returns> The join. </returns>
        public static String Join<T>(String separator, T[] value)
        {
            if (value.IsNullOrEmpty()) return String.Empty;
            if (separator.IsNull()) separator = String.Empty;
            var converter = new Converter<T, string>((obj) => obj.ToString());
            return String.Join(separator, Array.ConvertAll(value, converter));
        }

        /// <summary>
        ///   Encrypt or decrypt a byte array using the TripleDESCryptoServiceProvider crypto provider and Rfc2898DeriveBytes to build the key and initialization vector.
        /// </summary>
        /// <param name="password"> The password String to use in encrypting or decrypting. </param>
        /// <param name="in_bytes"> The array of bytes to encrypt. </param>
        /// <param name="encrypt"> True to encrypt, False to decrypt. </param>
        /// <returns> </returns>
        /// <remarks>
        /// </remarks>
        public static byte[] CryptBytes(String password, byte[] in_bytes, bool encrypt)
        {
            // Make a triple DES service provider.
            var desProvider = new TripleDESCryptoServiceProvider();
            // Find a valid key size for this provider.
            var keySize = 0;
            for (var i = 1024; i >= 1; --i)
                if (desProvider.ValidKeySize(i))
                {
                    keySize = i;
                    break;
                }
            // Get the block size for this provider.
            var blockSize = desProvider.BlockSize;
            // Generate the key and initialization vector.
            byte[] key = null;
            byte[] iv = null;
            var salt = new Byte[] {0x10, 0x20, 0x12, 0x23, 0x37, 0xA4, 0xC5, 0xA6, 0xF1, 0xF0, 0xEE, 0x21, 0x22, 0x45};
            MakeKeyAndIV(password, salt, keySize, blockSize, ref key, ref iv);
            // Make the encryptor or decryptor.
            var cryptoTransform = encrypt
                                      ? desProvider.CreateEncryptor(key, iv)
                                      : desProvider.CreateDecryptor(key, iv);
            byte[] result = null;
            // Create the output stream.
            using (var streamOut = new MemoryStream())
            {
                // Attach a crypto stream to the output stream.
                var streamCrypto = new CryptoStream(streamOut, cryptoTransform, CryptoStreamMode.Write);
                // Write the bytes into the CryptoStream.
                streamCrypto.Write(in_bytes, 0, in_bytes.Length);
                try
                {
                    streamCrypto.FlushFinalBlock();
                }
                catch (CryptographicException)
                {
                    // Ignore this one. The password is bad.
                }
                // Save the result.
                result = streamOut.ToArray();
                // Close the stream.
                try
                {
                    streamCrypto.Close();
                }
                catch (CryptographicException)
                {
                    // Ignore this one. The password is bad.
                }
                streamOut.Close();
            }
            return result;
        }

        /// <summary>
        ///   Use the password to generate key bytes and an initialization vector with Rfc2898DeriveBytes.
        /// </summary>
        /// <param name="password"> The input password to use in generating the bytes. </param>
        /// <param name="salt"> The input salt bytes to use in generating the bytes. </param>
        /// <param name="sizeKey"> The input size of the key to generate. </param>
        /// <param name="sizeBlock"> The input block size used by the crypto provider. </param>
        /// <param name="key"> The output key bytes to generate. </param>
        /// <param name="initVector"> The output initialization vector to generate. </param>
        /// <remarks>
        /// </remarks>
        public static void MakeKeyAndIV(String password, byte[] salt, int sizeKey, int sizeBlock,
                                        ref byte[] key, ref byte[] initVector)
        {
            using (var rfcDerive = new Rfc2898DeriveBytes(password, salt, 1234))
            {
                key = rfcDerive.GetBytes(sizeKey/8);
                initVector = rfcDerive.GetBytes(sizeBlock/8);
            }
        }
        #endregion
    }
}