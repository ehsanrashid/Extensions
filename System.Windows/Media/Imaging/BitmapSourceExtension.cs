using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace System.Windows.Media.Imaging
{

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

            using (Stream stream = new MemoryStream())
            {
                encoder.Save(stream);
                // Nested construction required to prevent issues from closing the underlying stream
                return new Bitmap(new Bitmap(stream));
            }
        }

        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            var memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Png);

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.EndInit();

            return bitmapImage;
        }


        public static BitmapImage ToBitmapImage(this Byte[] byteArray)
        {
            var memoryStream = new MemoryStream(byteArray);

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.EndInit();

            return bitmapImage;
        }

        public static BitmapSource ToBitmapSource(this Byte[] byteArray)
        {
            var memoryStream = new MemoryStream(byteArray);
            var decoder = new PngBitmapDecoder(memoryStream,
                                                BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            var bitmapSource = decoder.Frames[0];

            return bitmapSource;
        }
    

    
    }
}