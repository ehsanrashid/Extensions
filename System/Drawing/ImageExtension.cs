using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace System.Drawing
{
    public static class ImageExtension
    {
        /// <summary>
        ///   Serializes the image in an byte array
        /// </summary>
        /// <param name="image"> Instance value. </param>
        /// <param name="format"> Specifies the format of the image. </param>
        /// <returns> The image serialized as byte array. </returns>
        public static byte[] ToBytes(this Image image, ImageFormat format)
        {
            if (null == image) throw new ArgumentNullException("image");
            if (null == format) throw new ArgumentNullException("format");
            using (var stream = new MemoryStream())
            {
                image.Save(stream, format);
                return stream.ToArray();
            }
        }

        /// <summary>
        ///   Gets the bounds of the image in pixels
        /// </summary>
        /// <param name="image"> Instance value. </param>
        /// <returns> A rectangle that has the same hight and width as given image. </returns>
        public static Rectangle GetBounds(this Image image)
        {
            return new Rectangle(0, 0, image.Width, image.Height);
        }

        /// <summary>
        ///   Gets the Image as a Byte[]
        /// </summary>
        /// <param name="image"> The img. </param>
        /// <param name="format"> ImageFormat </param>
        /// <returns> A Byte[] of the Image </returns>
        /// <remarks>
        /// </remarks>
        public static byte[] GetImageInBytes(this Image image, ImageFormat format)
        {
            using (var stream = new MemoryStream())
            {
                if (null != format)
                {
                    image.Save(stream, format);
                    return stream.ToArray();
                }
                image.Save(stream, image.RawFormat);
                return stream.ToArray();
            }
        }

        /// <summary>
        ///   Gets the Image in Base64 format for storage or transfer
        /// </summary>
        /// <param name="image"> The img. </param>
        /// <param name="format"> ImageFormat </param>
        /// <returns> Base64 String of the Image </returns>
        /// <remarks>
        /// </remarks>
        public static String GetImageInBase64(this Image image, ImageFormat format)
        {
            using (var stream = new MemoryStream())
            {
                if (null != format)
                {
                    image.Save(stream, format);
                    return Convert.ToBase64String(stream.ToArray());
                }
                image.Save(stream, image.RawFormat);
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        /// <summary>
        ///   Scales the image.
        /// </summary>
        /// <param name="image"> The img. </param>
        /// <param name="height"> The height as int. </param>
        /// <param name="width"> The width as int. </param>
        /// <returns> </returns>
        /// <remarks>
        /// </remarks>
        public static Image ScaleImage(this Image image, int height, int width)
        {
            if (null == image || height <= 0 || width <= 0) return null;
            var newWidth = (image.Width*height)/(image.Height);
            var newHeight = (image.Height*width)/(image.Width);
            var bitmap = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                // use this when debugging.
                //g.FillRectangle(Brushes.Aqua, 0, 0, bmp.Width - 1, bmp.Height - 1);
                if (newWidth > width)
                {
                    // use new height
                    var x = (bitmap.Width - width)/2;
                    var y = (bitmap.Height - newHeight)/2;
                    graphics.DrawImage(image, x, y, width, newHeight);
                }
                else
                {
                    // use new width
                    var x = (bitmap.Width/2) - (newWidth/2);
                    var y = (bitmap.Height/2) - (height/2);
                    graphics.DrawImage(image, x, y, newWidth, height);
                }
                // use this when debugging.
                //g.DrawRectangle(new Pen(Color.Red, 1), 0, 0, bmp.Width - 1, bmp.Height - 1);
            }
            return bitmap;
        }

        /// <summary>
        ///   Resizes the image to fit new size.
        /// </summary>
        /// <param name="image"> The image. </param>
        /// <param name="newSize"> The new size. </param>
        /// <returns> </returns>
        /// <remarks>
        /// </remarks>
        public static Image ResizeAndFit(this Image image, Size newSize)
        {
            var sourceIsLandscape = image.Width > image.Height;
            var targetIsLandscape = newSize.Width > newSize.Height;
            var ratioWidth = newSize.Width/(double) image.Width;
            var ratioHeight = newSize.Height/(double) image.Height;
            var ratio = ratioWidth > ratioHeight && sourceIsLandscape == targetIsLandscape ? ratioWidth : ratioHeight;
            var targetWidth = (int) (image.Width*ratio);
            var targetHeight = (int) (image.Height*ratio);
            var bitmap = new Bitmap(newSize.Width, newSize.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var offsetX = ((double) (newSize.Width - targetWidth))/2;
                var offsetY = ((double) (newSize.Height - targetHeight))/2;
                graphics.DrawImage(image, (int) offsetX, (int) offsetY, targetWidth, targetHeight);
                graphics.Dispose();
            }
            return bitmap;
        }


    }
}