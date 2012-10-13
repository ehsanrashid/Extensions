
namespace System.Helper
{
    using Linq;
    using Text;
    using Security.Cryptography;

    public static class Cryptor
    {
        /// <summary>
        /// The actual hash calculation is shared by both GetHashAndSalt and the VerifyHash functions
        /// </summary>
        /// <param name="bDdata">A byte array of the Data to Hash</param>
        /// <param name="bSalt">A byte array of the Salt to add to the Hash</param>
        /// <param name="hashAlgorithm"> </param>
        /// <returns>A byte array with the calculated hash</returns>
        public static byte[] ComputeHash(
            byte[] bDdata,
            byte[] bSalt = default(byte[]),
            HashAlgorithm hashAlgorithm = default(HashAlgorithm))
        {
            // set default to Sha256
            if (null == hashAlgorithm) hashAlgorithm = SHA256.Create();

            // Combine salt and input bytes
            if (default(byte[]) != bSalt)
            {
                var byteSaltedData = new byte[bDdata.Length + bSalt.Length];
                MergeArray(bDdata, bSalt, byteSaltedData);
                bDdata = byteSaltedData;
            }

            var bHashed = hashAlgorithm.ComputeHash(bDdata);
            return bHashed;
        }

        /// <summary>
        /// The routine provides a wrapper around the ComputeHash function providing conversion
        /// from the required byte arrays to strings. Hash is returned as Base-64 encoded strings.
        /// </summary>
        /// <param name="sData">
        /// A <see cref="System.String"/> String containing the data to hash
        /// </param>
        /// <param name="sSalt">
        /// A <see cref="System.String"/> base64 encoded String containing the generated salt
        /// </param>
        /// <param name="encoding"> </param>
        public static String ComputeHashString(String sData, String sSalt, Encoding encoding = default(Encoding))
        {
            if (null == encoding) encoding = Encoding.UTF8;
            // Obtain the Hash for the given String
            var bHash = ComputeHash(encoding.GetBytes(sData), encoding.GetBytes(sSalt));
            // Transform the byte[] to Base-64 encoded strings
            return Convert.ToBase64String(bHash);
        }

        /// <summary>
        /// This routine verifies whether the data generates the same hash as we had stored previously
        /// </summary>
        /// <param name="bData">The data to verify </param>
        /// <param name="bHash">The hash we had stored previously</param>
        /// <param name="bSalt">The salt we had stored previously</param>
        /// <returns>True on a succesfull match</returns>
        public static bool VerifyHash(byte[] bData, byte[] bHash, byte[] bSalt)
        {
            var bNewHash = ComputeHash(bData, bSalt);
            if (bNewHash.Length != bHash.Length) return false;

            //for (var Lp = 0; Lp < bHash.Length; ++Lp)
            //    if (bHash[Lp] != bNewHash[Lp])
            //        return false;
            //return true;

            return !bHash.Where((b, index) => b != bNewHash[index]).Any();
        }

        public static bool VerifyHashString(String sData, String sHash, String sSalt, Encoding encoding = default(Encoding))
        {
            if (null == encoding) encoding = Encoding.UTF8; //Encoding.Unicode;

            var bData = encoding.GetBytes(sData);
            var bHash = Convert.FromBase64String(sHash);
            var bSalt = Convert.FromBase64String(sSalt);
            return VerifyHash(bData, bHash, bSalt);
        }

        public static byte[] GetRandomSaltByte(int length)
        {
            // Create and populate random byte array
            var bSalt = new byte[length];
            //var rnd = new Random();
            //rnd.NextBytes(bSalt);
            using (var cryptoRNG = new RNGCryptoServiceProvider())
            {
                cryptoRNG.GetNonZeroBytes(bSalt);
                //cryptoRNG.GetBytes(bSalt);
            }
            
            return bSalt;
        }

        public static String GetRandomSaltString(int length)
        {
            return Convert.ToBase64String(GetRandomSaltByte(length));
        }


        private static void MergeArray<T>(T[] arrData, T[] arrSalt, T[] arrSaltedData)
        {
            // --------------------------------------
            //var length = arrData.Length;
            //for (var i = 0; i < length; ++i)
            //    arrSaltedData[i] = arrData[i];
            //for (var i = 0; i < arrSalt.Length; ++i)
            //    arrSaltedData[length + i] = arrSalt[i];
            // --------------------------------------
            //Array.Copy(arrData, arrSaltedData, arrData.Length);
            //Array.Copy(arrSalt, 0, arrSaltedData, arrData.Length, arrSalt.Length);
            // --------------------------------------
            arrData.CopyTo(arrSaltedData, 0);
            arrSalt.CopyTo(arrSaltedData, arrData.Length);
            // --------------------------------------
        }


        //static MD5CryptoServiceProvider s_md5 = null;
        //public static String MD5(String str)
        //{
        //    if (s_md5 == null) //creating only when needed
        //        s_md5 = new MD5CryptoServiceProvider();
        //    byte[] byteNewdata = Encoding.Default.GetBytes(str);
        //    byte[] byteEncrypted = s_md5.ComputeHash(byteNewdata);
        //    return BitConverter.ToString(byteEncrypted).Replace("-", String.Empty).ToLower();
        //}

    }
}
