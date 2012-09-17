using System.Collections.Generic;
using System.IO;

namespace System.Drawing
{
    public static class IconExtension
    {
        /// <summary>
        ///   Split an Icon (that contains multiple icons) into an array of Icon each rapresenting a single icons.
        /// </summary>
        /// <param name="icon"> Instance value. </param>
        /// <returns> An array of <see cref="System.Drawing.Icon" /> objects. </returns>
        public static Icon[] SplitIcon(this Icon icon)
        {
            icon.ExceptionIfNull("Can't split the icon. Icon is null.", "icon");
            // Get multiple .ico file image.
            byte[] srcBuf = null;
            using (var stream = new MemoryStream())
            {
                icon.Save(stream);
                srcBuf = stream.ToArray();
            }
            var splitIcons = new List<Icon>();
            const int sizeIconDir = 6; // sizeof(IconDir) 
            const int sizeIconDirEntry = 16; // sizeof(IconDirEntry)
            var count = BitConverter.ToInt16(srcBuf, 4); // ICONDIR.idCount
            for (var i = 0; i < count; ++i)
                using (var stream = new MemoryStream())
                using (var writer = new BinaryWriter(stream))
                {
                    // Copy IconDir and IconDirEntry.
                    writer.Write(srcBuf, 0, sizeIconDir - 2);
                    writer.Write((short) 1); // IconDir.idCount == 1;
                    writer.Write(srcBuf, sizeIconDir + sizeIconDirEntry*i, sizeIconDirEntry - 4);
                    writer.Write(sizeIconDir + sizeIconDirEntry);
                    // IconDirEntry.dwImageOffset = sizeof(IconDir) + sizeof(IconDirEntry)
                    // Copy picture and mask data.
                    var sizeImg = BitConverter.ToInt32(srcBuf, sizeIconDir + sizeIconDirEntry*i + 8);
                    // IconDirEntry.dwBytesInRes
                    var offsetImg = BitConverter.ToInt32(srcBuf, sizeIconDir + sizeIconDirEntry*i + 12);
                    // IconDirEntry.dwImageOffset
                    writer.Write(srcBuf, offsetImg, sizeImg);
                    // Create new icon.
                    stream.Seek(0, SeekOrigin.Begin);
                    splitIcons.Add(new Icon(stream));
                }
            return splitIcons.ToArray();
        }
    }
}