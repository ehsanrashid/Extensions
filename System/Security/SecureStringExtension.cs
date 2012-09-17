using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Security
{
    public static class SecureStringExtension
    {
        /// <summary>
        ///   Checks whether a <see cref="SecureString" /> is either null or has a <see cref="SecureString.Length" /> of 0.
        /// </summary>
        /// <param name="secureStr"> The secure string to be inspected. </param>
        /// <returns> True if the string is either null or empty. </returns>
        public static bool IsNullOrEmpty(this SecureString secureStr)
        {
            return (default(SecureString) == secureStr) || (0 == secureStr.Length);
        }

        /// <summary>
        ///   Wraps a managed string into a <see cref="SecureString" /> instance.
        /// </summary>
        /// <param name="seqChar"> A string or char sequence that should be encapsulated. </param>
        /// <returns> A <see cref="SecureString" /> that encapsulates the submitted sequence of char. </returns>
        /// <exception cref="ArgumentNullException">If
        ///   <paramref name="seqChar" />
        ///   is a null reference.</exception>
        public static SecureString ToSecureString(this IEnumerable<char> seqChar)
        {
            if (default(IEnumerable<char>) == seqChar) throw new ArgumentNullException("seqChar");
            var secureStr = default(SecureString);
            // -------------------------------------------
            //secureStr = new SecureString();
            //foreach (var c in str)
            //    secureStr.AppendChar(c);
            // -------------------------------------------
            unsafe
            {
                var str = new String(seqChar.ToArray());
                fixed (char* pChar = str)
                    secureStr = new SecureString(pChar, str.Length);
            }
            // -------------------------------------------
            secureStr.MakeReadOnly();
            return secureStr;
        }

        /// <summary>
        ///   Unwraps the contents of a secured string and returns the contained value.
        /// </summary>
        /// <param name="secureStr"> </param>
        /// <returns> </returns>
        /// <remarks>
        ///   Be aware that the unwrapped managed string can be extracted from memory.
        /// </remarks>
        /// <exception cref="ArgumentNullException">If
        ///   <paramref name="secureStr" />
        ///   is a null reference.</exception>
        public static IEnumerable<char> UnWrap(this SecureString secureStr)
        {
            if (default(SecureString) == secureStr) throw new ArgumentNullException("secureStr");
            // --------------------------------------------------
            //var ptr = IntPtr.Zero;
            //try
            //{
            //    ptr = Marshal.SecureStringToBSTR(secureStr);
            //    //return Marshal.PtrToStringBSTR(ptr);
            //    var length = secureStr.Length;
            //    var chChar = new char[length];
            //    Marshal.Copy(bStr, chChar, 0, length);
            //    return chChar;
            //}
            //finally
            //{
            //    Marshal.ZeroFreeBSTR(ptr);
            //}
            // --------------------------------------------------
            var ptr = IntPtr.Zero; // unmanaged pointer
            try
            {
                //ptr = Marshal.SecureStringToCoTaskMemUnicode(secureStr);
                ptr = Marshal.SecureStringToGlobalAllocUnicode(secureStr);
                //return Marshal.PtrToStringUni(ptr);
                var length = secureStr.Length;
                var chChar = new char[length];
                Marshal.Copy(ptr, chChar, 0, length);
                return chChar;
            }
            finally
            {
                //Marshal.ZeroFreeCoTaskMemUnicode(ptr);
                Marshal.ZeroFreeGlobalAllocUnicode(ptr);
            }
            // --------------------------------------------------
        }

        /// <summary>
        ///   Performs bytewise comparison of two secure strings.
        /// </summary>
        /// <param name="valueA"> </param>
        /// <param name="valueB"> </param>
        /// <returns> True if the strings are equal. </returns>
        public static bool Matches(this SecureString valueA, SecureString valueB)
        {
            if (default(SecureString) == valueA && default(SecureString) == valueB) return true;
            if (default(SecureString) == valueA || default(SecureString) == valueB) return false;
            if (valueA.Length != valueB.Length) return false;
            if (valueA.Length == 0 && valueB.Length == 0) return true;
            var ptrA = Marshal.SecureStringToCoTaskMemUnicode(valueA);
            var ptrB = Marshal.SecureStringToCoTaskMemUnicode(valueB);
            try
            {
                //parse characters one by one - doesn't change the fact that we have them in memory however...
                byte byteA = 1;
                byte byteB = 1;
                var index = 0;
                while (((char) byteA) != '\0' && ((char) byteB) != '\0')
                {
                    byteA = Marshal.ReadByte(ptrA, index);
                    byteB = Marshal.ReadByte(ptrB, index);
                    if (byteA != byteB)
                        return false;
                    index += 2;
                }
                return true;
            }
            finally
            {
                Marshal.ZeroFreeCoTaskMemUnicode(ptrA);
                Marshal.ZeroFreeCoTaskMemUnicode(ptrB);
            }
        }

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
            foreach (var c in str)
                secureString.AppendChar(c);
            if (makeReadOnly) secureString.MakeReadOnly();
            return secureString;
        }

        /// <summary>
        ///   Coverts the SecureString to a regular string.
        /// </summary>
        /// <param name="secureString"> Object value. </param>
        /// <returns> Content of secured string. </returns>
        public static String ToUnsecureString(this SecureString secureString)
        {
            if (secureString.IsNull()) return default(String);
            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}