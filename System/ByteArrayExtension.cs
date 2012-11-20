using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Media.Imaging;

namespace System
{
    /// <summary>
    /// Extension methods for byte-Arrays
    /// </summary>
    public static class ByteArrayExtension
    {
        public static byte[] ConcatBuf(this byte[] buff1, byte[] buff2)
        {
            if (null == buff1 || 0 == buff1.Length) return buff2;
            if (null == buff2 || 0 == buff2.Length) return buff1;
            var buffer = new byte[buff1.Length + buff2.Length];
            Buffer.BlockCopy(buff1, 0, buffer, 0, buff1.Length);
            Buffer.BlockCopy(buff2, 0, buffer, buff1.Length, buff2.Length);
            return buffer;
        }

        //public static IEnumerable<byte[]> Split(this byte[] buffer, byte seperator, bool includeSeperator)
        //{
        //    return Split(buffer, new byte[] { seperator }, includeSeperator);
        //}

        public static IEnumerable<byte[]> Split(this byte[] buffer, int blockSize)
        {
            if (blockSize < 1) throw new ArgumentOutOfRangeException("blockSize");

            if (null == buffer) yield break;

            if (buffer.Length <= blockSize) yield return buffer;
            else
            {
                // Split source into blocks. Last block may be smaller then blockSize.
                var pos = 0;
                while (pos < buffer.Length)
                {
                    var length = Math.Min(blockSize, buffer.Length - pos);
                    var part = new byte[length];
                    Array.Copy(buffer, pos, part, 0, length);
                    pos += length;
                    yield return part;
                }
            }
        }

        public static IEnumerable<byte[]> Packetize(IEnumerable<byte> stream)
        {
            var buffer = new List<byte>();
            foreach (var b in stream)
            {
                var index = buffer.Count;
                buffer.Add(b);

                if (b != 0x03 || index < 2 || buffer[index - 1] != 0x10
                    || (index != 2 && buffer[index - 2] == 0x10)) continue;

                yield return buffer.ToArray();
                buffer.Clear();
            }
            if (buffer.Count > 0) yield return buffer.ToArray();
        }

        #region Encrypt Decrypt

        /// <summary>
        /// Convert a byte array into a hexadecimal String representation.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static String BytesToHexString(this Byte[] bytes)
        {
            //var result = String.Empty;
            //foreach (byte b in bytes)
            //    result += " " + b.ToString("X").PadLeft(2, '0');
            var result = bytes.Aggregate(String.Empty,
                                         (aggregate, b) => aggregate + (" " + b.ToString("X").PadLeft(2, '0')));
            if (result.Length > 0) result = result.Substring(1);
            return result;
        }

        /// <summary>
        /// Decrypt the encryption stored in this byte array.
        /// </summary>
        /// <param name="encryptedBytes">The byte array to decrypt.</param>
        /// <param name="password">The password to use when decrypting.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static String DecryptFromByteArray(this Byte[] encryptedBytes, String password)
        {
            var decryptedBytes = CryptBytes(password, encryptedBytes, false);
            var asciiEncoder = new ASCIIEncoding();
            return new String(asciiEncoder.GetChars(decryptedBytes));
        }

        /// <summary>
        /// Encrypt or decrypt a byte array using the TripleDESCryptoServiceProvider crypto provider and Rfc2898DeriveBytes to build the key and initialization vector.
        /// </summary>
        /// <param name="password">The password String to use in encrypting or decrypting.</param>
        /// <param name="inBytes">The array of bytes to encrypt.</param>
        /// <param name="encrypt">True to encrypt, False to decrypt.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte[] CryptBytes(String password, byte[] inBytes, bool encrypt)
        {
            // Make a triple DES service provider.
            var desProvider = new TripleDESCryptoServiceProvider();
            // Find a valid key size for this provider.
            var keySize = 0;
            for (var i = 1024; i >= 1; i--)
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
            byte[] salt = { 0x10, 0x20, 0x12, 0x23, 0x37, 0xA4, 0xC5, 0xA6, 0xF1, 0xF0, 0xEE, 0x21, 0x22, 0x45 };
            MakeKeyAndIv(password, salt, keySize, blockSize, ref key, ref iv);
            // Make the encryptor or decryptor.
            var cryptoTransform = encrypt
                                      ? desProvider.CreateEncryptor(key, iv)
                                      : desProvider.CreateDecryptor(key, iv);
            byte[] result;
            // Create the output stream.
            using (var streamOut = new MemoryStream())
            {
                // Attach a crypto stream to the output stream.
                var streamCrypto = new CryptoStream(streamOut, cryptoTransform, CryptoStreamMode.Write);
                // Write the bytes into the CryptoStream.
                streamCrypto.Write(inBytes, 0, inBytes.Length);
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
        /// Use the password to generate key bytes and an initialization vector with Rfc2898DeriveBytes.
        /// </summary>
        /// <param name="password">The input password to use in generating the bytes.</param>
        /// <param name="salt">The input salt bytes to use in generating the bytes.</param>
        /// <param name="keySize">The input size of the key to generate.</param>
        /// <param name="blockSize">The input block size used by the crypto provider.</param>
        /// <param name="key">The output key bytes to generate.</param>
        /// <param name="iv">The output initialization vector to generate.</param>
        /// <remarks></remarks>
        public static void MakeKeyAndIv(String password, byte[] salt, int keySize, int blockSize,
                                        ref byte[] key, ref byte[] iv)
        {
            if (password == null) throw new ArgumentNullException("password");
            var deriveBytes = new Rfc2898DeriveBytes(password, salt, 1234);
            key = deriveBytes.GetBytes(keySize / 8);
            iv = deriveBytes.GetBytes(blockSize / 8);
        }

        #endregion

        /// <summary>
        /// Find the first occurence of an byte[] in another byte[]
        /// </summary>
        /// <param name = "buffer1">the byte[] to search in</param>
        /// <param name = "buffer2">the byte[] to find</param>
        /// <returns>the first position of the found byte[] or -1 if not found</returns>
        public static int FindArrayInArray(this Byte[] buffer1, Byte[] buffer2)
        {
            if (default(Byte[]) == buffer1) throw new ArgumentNullException("buffer1");
            if (default(Byte[]) == buffer2) throw new ArgumentNullException("buffer2");

            if (buffer2.Length == 0) return 0; // by definition empty sets match immediately
            var j = -1;
            var end = buffer1.Length - buffer2.Length;
            while ((j = Array.IndexOf(buffer1, buffer2[0], j + 1)) <= end && j != -1)
            {
                var i = 1;
                while (buffer1[j + i] == buffer2[i]) if (++i == buffer2.Length) return j;
            }
            return -1;
        }

        /// <summary>
        /// Converts to image.
        /// </summary>
        /// <param name="byteArray">The byte array in.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Image ConvertToImage(this Byte[] byteArray)
        {
            return Image.FromStream(new MemoryStream(byteArray));
        }

        public static BitmapImage ToBitmapImage(this Byte[] byteArray)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            using (var stream = new MemoryStream(byteArray))
            {
                bitmapImage.StreamSource = stream;
            }
            bitmapImage.EndInit();
            return bitmapImage;
        }

        public static BitmapSource ToBitmapSource(this Byte[] byteArray)
        {
            using (var stream = new MemoryStream(byteArray))
            {
                var decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                var bitmapSource = decoder.Frames[0];
                return bitmapSource;
            }
        }


    }
}