namespace System.Text
{
    using Collections;
    using Collections.Generic;
    using IO;

    /// <summary>
    ///   Extensions for StringBuilder
    /// </summary>
    public static class StringBuilderExtension
    {
        /// <summary>
        ///   Append version with format String parameters.
        /// </summary>
        public static void Append(this StringBuilder sb, String format, params Object[] args)
        {
            sb.AppendLine(String.Format(format, args));
        }

        /// <summary>
        ///   AppendLine version with format String parameters.
        /// </summary>
        public static void AppendLine(this StringBuilder sb, String format, params Object[] args)
        {
            sb.AppendLine(String.Format(format, args));
        }

        /// <summary>
        /// </summary>
        /// <param name="sb"> </param>
        /// <param name="startIndex"> </param>
        /// <param name="length"> </param>
        /// <returns> </returns>
        public static String Substring(this StringBuilder sb, int startIndex, int length)
        {
            return sb.ToString(startIndex, length);
        }

        public static StringBuilder Remove(this StringBuilder sb, char ch)
        {
            for (var i = 0; i < sb.Length;)
                if (sb[i] == ch)
                    sb.Remove(i, 1);
                else
                    ++i;
            return sb;
        }

        public static StringBuilder RemoveFromEnd(this StringBuilder sb, int num)
        {
            return sb.Remove(sb.Length - num, num);
        }

        //public static void Clear(this StringBuilder sb)
        //{
        //    //sb.Length = 0;
        //    sb.Clear();
        //}

        /// <summary>
        ///   Trim left spaces of String
        /// </summary>
        /// <param name="sb"> </param>
        /// <returns> </returns>
        public static StringBuilder LTrim(this StringBuilder sb)
        {
            if (0 != sb.Length)
            {
                var length = 0;
                while ((sb[length] == ' ') && (length < sb.Length))
                    ++length;
                if (length > 0) sb.Remove(0, length);
            }
            return sb;
        }

        /// <summary>
        ///   Trim right spaces of String
        /// </summary>
        /// <param name="sb"> </param>
        /// <returns> </returns>
        public static StringBuilder RTrim(this StringBuilder sb)
        {
            if (0 != sb.Length)
            {
                var length = sb.Length - 1;
                while ((sb[length] == ' ') && (length >= 0))
                    --length;
                if (length < (sb.Length - 1))
                    sb.Remove(length + 1, (sb.Length - length) - 1);
            }
            return sb;
        }

        /// <summary>
        ///   Trim spaces around String
        /// </summary>
        /// <param name="sb"> </param>
        /// <returns> </returns>
        public static StringBuilder Trim_(this StringBuilder sb)
        {
            if (0 != sb.Length)
            {
                var length = 0;
                while ((sb[length] == ' ') && (length < sb.Length))
                    ++length;
                if (length > 0)
                    sb.Remove(0, length);
                length = sb.Length - 1;
                while ((sb[length] == ' ') && (length > -1))
                    --length;
                if (length < (sb.Length - 1))
                    sb.Remove(length + 1, (sb.Length - length) - 1);
            }
            return sb;
        }

        /// <summary>
        ///   Trim spaces around string
        /// </summary>
        /// <param name="sb"> </param>
        /// <returns> </returns>
        public static StringBuilder Trim(this StringBuilder sb)
        {
            sb = LTrim(sb);
            sb = RTrim(sb);
            return sb;
        }

        /// <summary>
        ///   Get index of a char starting from a given index
        /// </summary>
        /// <param name="sb"> </param>
        /// <param name="ch"> </param>
        /// <param name="startIndex"> </param>
        /// <returns> </returns>
        public static int IndexOf(this StringBuilder sb, char ch, int startIndex = 0)
        {
            for (var i = startIndex; i < sb.Length; i++)
                if (sb[i] == ch)
                    return i;
            return -1;
        }

        /// <summary>
        ///   Get index of a String from a given index with case option
        /// </summary>
        /// <param name="sb"> </param>
        /// <param name="str"> </param>
        /// <param name="startIndex"> </param>
        /// <param name="ignoreCase"> </param>
        /// <returns> </returns>
        public static int IndexOf(this StringBuilder sb, String str, int startIndex = 0, bool ignoreCase = false)
        {
            var length = str.Length;
            if (ignoreCase)
            {
                for (var j = startIndex; j < (sb.Length - length) + 1; j++)
                    if (char.ToLower(sb[j]) == char.ToLower(str[0]))
                    {
                        var len = 1;
                        while ((len < length) && (char.ToLower(sb[j + len]) == char.ToLower(str[len])))
                            len++;
                        if (len == length)
                            return j;
                    }
            }
            else
            {
                for (var i = startIndex; i < (sb.Length - length) + 1; i++)
                    if (sb[i] == str[0])
                    {
                        var len = 1;
                        while ((len < length) && (sb[i + len] == str[len]))
                            len++;
                        if (len == length)
                            return i;
                    }
            }
            return -1;
        }

        /// <summary>
        ///   Determine whether a String is begin with a given text
        /// </summary>
        /// <param name="sb"> </param>
        /// <param name="str"> </param>
        /// <param name="startIndex"> </param>
        /// <param name="ignoreCase"> </param>
        /// <returns> </returns>
        public static bool StartsWith(this StringBuilder sb, String str, int startIndex = 0, bool ignoreCase = false)
        {
            var length = str.Length;
            if (ignoreCase)
            {
                for (var j = startIndex; j < startIndex + length; j++)
                    if (char.ToLower(sb[j]) != char.ToLower(str[j - startIndex]))
                        return false;
            }
            else
            {
                for (var i = startIndex; i < startIndex + length; i++)
                    if (sb[i] != str[i - startIndex])
                        return false;
            }
            return true;
        }

        public static void CopyToFile(this StringBuilder sb, String path)
        {
            File.WriteAllText(path, sb.ToString());
        }

        #region AppendCollection
        public static StringBuilder AppendCollection<T>(this StringBuilder sb, IEnumerable<T> sequence,
                                                        Func<T, String> func)
        {
            //foreach (var item in sequence)
            //    sb.AppendLine(func(item));
            sequence.ForEach((item) => sb.AppendLine(func(item)));
            return sb;
        }

        public static StringBuilder AppendCollection<T>(this StringBuilder sb, IEnumerable<T> sequence)
        {
            return AppendCollection(sb, sequence, (x) => x.ToString());
        }

        public static StringBuilder AppendCollection<T>(this StringBuilder sb, List<T> list, Func<T, String> func)
        {
            list.ForEach((item) => sb.AppendLine(func(item)));
            return sb;
        }

        public static StringBuilder AppendCollection<T>(this StringBuilder sb, List<T> list)
        {
            return AppendCollection(sb, list, (x) => x.ToString());
        }

        public static StringBuilder AppendCollection(this StringBuilder sb, ICollection collection)
        {
            foreach (var item in collection)
                sb.AppendLine(Convert.ToString(item));
            return sb;
        }
        #endregion

        #region Html-specific

        #region Table
        public static void BeginTable(this StringBuilder sb)
        {
            sb.Append("<table>");
        }

        public static void BeginTable(this StringBuilder sb, String className)
        {
            sb.AppendFormat("<table class=\"{0}\">", className);
        }

        public static void EndTable(this StringBuilder sb)
        {
            sb.Append("</table>");
        }
        #endregion

        #region Row
        public static void BeginRow(this StringBuilder sb)
        {
            sb.Append("<tr>");
        }

        public static void BeginRow(this StringBuilder sb, String className)
        {
            sb.AppendFormat("<tr class=\"{0}\">", className);
        }

        public static void EndRow(this StringBuilder sb)
        {
            sb.Append("</tr>");
        }
        #endregion

        #region Cell
        public static void BeginCell(this StringBuilder sb)
        {
            sb.Append("<td>");
        }

        public static void BeginCell(this StringBuilder sb, int colSpan)
        {
            sb.AppendFormat("<td colspan=\"{0}\">", colSpan);
        }

        public static void BeginCell(this StringBuilder sb, String className)
        {
            sb.AppendFormat("<td class=\"{0}\">", className);
        }

        public static void BeginCell(this StringBuilder sb, String className, int colSpan)
        {
            sb.AppendFormat("<td class=\"{0}\" colspan=\"{1}\">", className, colSpan);
        }

        public static void EndCell(this StringBuilder sb)
        {
            sb.Append("</td>");
        }
        #endregion

        #region Heading
        public static void BeginH1(this StringBuilder sb)
        {
            sb.Append("<h1>");
        }

        public static void EndH1(this StringBuilder sb)
        {
            sb.Append("</h1>");
        }

        public static void BeginH2(this StringBuilder sb)
        {
            sb.Append("<h2>");
        }

        public static void EndH2(this StringBuilder sb)
        {
            sb.Append("</h2>");
        }

        public static void BeginH3(this StringBuilder sb)
        {
            sb.Append("<h3>");
        }

        public static void EndH3(this StringBuilder sb)
        {
            sb.Append("</h3>");
        }

        public static void BeginH4(this StringBuilder sb)
        {
            sb.Append("<h4>");
        }

        public static void EndH4(this StringBuilder sb)
        {
            sb.Append("</h4>");
        }

        public static void BeginH5(this StringBuilder sb)
        {
            sb.Append("<h5>");
        }

        public static void EndH5(this StringBuilder sb)
        {
            sb.Append("</h5>");
        }

        public static void BeginH6(this StringBuilder sb)
        {
            sb.Append("<h6>");
        }

        public static void EndH6(this StringBuilder sb)
        {
            sb.Append("</h6>");
        }
        #endregion

        public static void BeginBold(this StringBuilder sb)
        {
            sb.Append("<b>");
        }

        public static void EndBold(this StringBuilder sb)
        {
            sb.Append("</b>");
        }

        public static void BeginUnderline(this StringBuilder sb)
        {
            sb.Append("<u>");
        }

        public static void EndUnderline(this StringBuilder sb)
        {
            sb.Append("</u>");
        }

        public static void LineBreak(this StringBuilder sb)
        {
            sb.Append("<br/>");
        }

        public static void HorizontalRule(this StringBuilder sb)
        {
            sb.Append("<hr/>");
        }

        public static void BeginParagraph(this StringBuilder sb)
        {
            sb.Append("<p>");
        }

        public static void EndParagraph(this StringBuilder sb)
        {
            sb.Append("</p>");
        }

        public static void BeginHtml(this StringBuilder sb)
        {
            sb.Append("<html>");
        }

        public static void EndHtml(this StringBuilder sb)
        {
            sb.Append("</html>");
        }

        public static void BeginHead(this StringBuilder sb)
        {
            sb.Append("<head>");
        }

        public static void EndHead(this StringBuilder sb)
        {
            sb.Append("</head>");
        }

        public static void BeginTHead(this StringBuilder sb)
        {
            sb.Append("<thead>");
        }

        public static void EndTHead(this StringBuilder sb)
        {
            sb.Append("</thead>");
        }

        public static void BeginBody(this StringBuilder sb)
        {
            sb.Append("<body>");
        }

        public static void BeginBody(this StringBuilder sb, String className)
        {
            sb.AppendFormat("<body class=\"{0}\">", className);
        }

        public static void EndBody(this StringBuilder sb)
        {
            sb.Append("</body>");
        }

        public static void BeginItalics(this StringBuilder sb)
        {
            sb.Append("<i>");
        }

        public static void EndItalics(this StringBuilder sb)
        {
            sb.Append("</i>");
        }
        #endregion
    }
}