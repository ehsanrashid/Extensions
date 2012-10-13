namespace System.Drawing
{
    public static class ColorExtension
    {
        /// <summary>
        /// returns the RGB Value of a color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>String</returns>
        /// <remarks></remarks>
        public static String ToHtmlColor(this Color color)
        {
            return ColorTranslator.ToHtml(color);
        }

        /// <summary>
        /// returns the OLE Value of the color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int ToOleColor(this Color color)
        {
            return ColorTranslator.ToOle(color);
        }

        /// <summary>
        /// returns the Win32 value of the color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int ToWin32Color(this Color color)
        {
            return ColorTranslator.ToWin32(color);
        }
    }
}