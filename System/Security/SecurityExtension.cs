using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace System.Security
{
    /// <summary>
    ///   Provides extension methods that deal with
    ///   String encryption/decryption and
    ///   <see cref="SecureString" /> encapsulation.
    /// </summary>
    public static class SecurityExtension
    {

        /// <summary>
        ///   Specifies the data protection scope of the DPAPI.
        /// </summary>
        const DataProtectionScope Scope = DataProtectionScope.CurrentUser;

        /// <summary>
        ///   Encrypts a given password and returns the encrypted data
        ///   as a base64 String.
        /// </summary>
        /// <param name="strPlain"> An unencrypted String that needs to be secured. </param>
        /// <returns> A base64 encoded String that represents the encrypted binary data. </returns>
        /// <remarks>
        ///   This solution is not really secure as we are keeping strings in memory. If runtime protection is essential, <see
        ///    cref="SecureString" /> should be used.
        /// </remarks>
        /// <exception cref="ArgumentNullException">If
        ///   <paramref name="strPlain" />
        ///   is a null reference.</exception>
        public static String Encrypt(this String strPlain)
        {
            if (default(String) == strPlain) throw new ArgumentNullException("strPlain");
            //encrypt data
            var byteData = Encoding.Unicode.GetBytes(strPlain);
            var byteEncrypted = ProtectedData.Protect(byteData, null, Scope);
            //return as base64 String
            return Convert.ToBase64String(byteEncrypted);
        }

        /// <summary>
        ///   Decrypts a given String.
        /// </summary>
        /// <param name="strCipher"> A base64 encoded String that was created through the <see cref="Encrypt(String)" /> or <see
        ///    cref="EncryptSecure(SecureString)" /> extension methods. </param>
        /// <returns> The decrypted String. </returns>
        /// <remarks>
        ///   Keep in mind that the decrypted String remains in memory and makes your application vulnerable per se. If runtime protection is essential, <see
        ///    cref="SecureString" /> should be used.
        /// </remarks>
        /// <exception cref="ArgumentNullException">If
        ///   <paramref name="strCipher" />
        ///   is a null reference.</exception>
        public static String Decrypt(this String strCipher)
        {
            if (default(String) == strCipher) throw new ArgumentNullException("strCipher");
            //parse base64 String
            var byteData = Convert.FromBase64String(strCipher);
            //decrypt data
            var byteDecrypted = ProtectedData.Unprotect(byteData, null, Scope);
            return Encoding.Unicode.GetString(byteDecrypted);
        }

        /// <summary>
        ///   Encrypts the contents of a secure String.
        /// </summary>
        /// <param name="value"> An unencrypted String that needs to be secured. </param>
        /// <returns> A base64 encoded String that represents the encrypted binary data. </returns>
        /// <exception cref="ArgumentNullException">If
        ///   <paramref name="value" />
        ///   is a null reference.</exception>
        public static String Encrypt(this SecureString value)
        {
            if (default(SecureString) == value) throw new ArgumentNullException("value");
            var ptr = Marshal.SecureStringToCoTaskMemUnicode(value);
            try
            {
                var chBuffer = new char[value.Length];
                Marshal.Copy(ptr, chBuffer, 0, value.Length);
                var byteData = Encoding.Unicode.GetBytes(chBuffer);
                var byteEncrypted = ProtectedData.Protect(byteData, null, Scope);
                //return as base64 String
                return Convert.ToBase64String(byteEncrypted);
            }
            finally
            {
                Marshal.ZeroFreeCoTaskMemUnicode(ptr);
            }
        }

        /// <summary>
        ///   Decrypts the contents of a secure String.
        /// </summary>
        /// <param name="value"> A base64 encoded String </param>
        /// <returns> The decrypted String. </returns>
        public static String Decrypt(this SecureString value)
        {
            return null;
        }

        ///<summary>
        ///</summary>
        ///<param name="strPlain"> </param>
        ///<returns> </returns>
        public static SecureString EncryptSecure(this String strPlain)
        {
            return null;
        }

        /// <summary>
        ///   Decrypts a base64 encrypted String and returns the decrpyted data
        ///   wrapped into a <see cref="SecureString" /> instance.
        /// </summary>
        /// <param name="strCipher"> A base64 encoded String that was created through the <see cref="Encrypt(String)" /> or <see
        ///    cref="EncryptSecure(SecureString)" /> extension methods. </param>
        /// <returns> The decrypted String, wrapped into a <see cref="SecureString" /> instance. </returns>
        /// <exception cref="ArgumentNullException">If
        ///   <paramref name="strCipher" />
        ///   is a null reference.</exception>
        public static SecureString DecryptSecure(this String strCipher)
        {
            if (default(String) == strCipher) throw new ArgumentNullException("strCipher");
            //parse base64 String
            var byteData = Convert.FromBase64String(strCipher);
            //decrypt data
            var byteDecrypted = ProtectedData.Unprotect(byteData, null, Scope);
            var secureStr = new SecureString();
            //parse characters one by one - doesn't change the fact that
            //we have them in memory however...
            var countChar = Encoding.Unicode.GetCharCount(byteDecrypted);
            var countByte = byteDecrypted.Length/countChar;
            for (var i = 0; i < countChar; ++i)
                secureStr.AppendChar(Encoding.Unicode.GetChars(byteDecrypted, i*countByte, countByte)[0]);
            //mark as read-only
            secureStr.MakeReadOnly();
            return secureStr;
        }
    }
}