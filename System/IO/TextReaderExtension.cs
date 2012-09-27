using System.Collections.Generic;

namespace System.IO
{
    /// <summary>
    ///   Extension methods for the TextReader class and its sub classes (StreamReader, StringReader)
    /// </summary>
    public static class TextReaderExtension
    {
        public static bool TryReadLine(this TextReader reader, out string line)
        {
            line = reader.ReadLine();
            return null != line;
        }

        /// <summary>
        ///   The method provides an iterator through all lines of the text reader.
        /// </summary>
        /// <param name="reader"> The text reader. </param>
        /// <returns> The iterator </returns>
        /// <example>
        ///   <code>using(var reader = fileInfo.OpenText()) {
        ///     foreach(var line in reader.IterateLines()) {
        ///     // ...
        ///     }
        ///     }</code>
        /// </example>
        /// <remarks>
        ///   Contributed by OlivierJ
        /// </remarks>
        public static IEnumerable<String> ReadLines(this TextReader reader)
        {
            //if (default(TextReader) != reader)
            //{
            //    String line;
            //    while (default(String) != (line = reader.ReadLine())) yield return line;
            //}
            //else yield return default(String);

            if (null != reader)
            {
                String line;
                while (reader.TryReadLine(out line)) 
                    yield return line;
            }
        }



        /// <summary>
        ///   The method executes the passed delegate /lambda expression) for all lines of the text reader.
        /// </summary>
        /// <param name="reader"> The text reader. </param>
        /// <param name="action"> The action. </param>
        /// <example>
        ///   <code>using(var reader = fileInfo.OpenText()) {
        ///     reader.IterateLines(l => Console.WriteLine(l));
        ///     }</code>
        /// </example>
        /// <remarks>
        ///   Contributed by OlivierJ
        /// </remarks>
        public static void ForEachReadLines(this TextReader reader, Action<String> action)
        {
            if (default(TextReader) != reader
             && default(Action<String>) != action)
            {
                //foreach (var line in reader.ReadLines())
                //    action(line);
                // -------------------------------
                reader.ReadLines().ForEach(action);
            }
        }
    }
}