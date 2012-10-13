namespace System.Windows.Media.Imaging
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using IO;

    /// <summary>
    ///   Extension methods for the System.Windows.Media.Imaging.BitmapSource class
    /// </summary>
    public static class BitmapSourceExtension
    {
        /// <summary>
        ///   Create a System.Drawing.Bitmap from the passed WPF BitmapSource instance
        /// </summary>
        /// <param name = "bitmapSource">The bitmap source.</param>
        /// <returns>The generated bitmap</returns>
        public static Bitmap ToBitmap(this BitmapSource bitmapSource)
        {
            var encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                // Nested construction required to prevent issues from closing the underlying stream
                return new Bitmap(stream);
            }
        }

    }
}