namespace System.Helper
{
    using Text;
    using Security;
    using Security.Cryptography;

    public static class Reader
    {

        public static String ReadMaskString(char? passwordChar = default(char?))
        {
            var maskStr = String.Empty;
            while (true)
            {
                var keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                case ConsoleKey.Backspace:
                    if (maskStr.Length > 0)
                    {
                        maskStr = maskStr.Remove(maskStr.Length - 1);
                        if (passwordChar != null) Console.Write("\b \b");
                    }
                    break;
                case ConsoleKey.Enter:
                    goto end;

                case ConsoleKey.A:
                case ConsoleKey.B:
                case ConsoleKey.C:
                case ConsoleKey.D:
                case ConsoleKey.E:
                case ConsoleKey.F:
                case ConsoleKey.G:
                case ConsoleKey.H:
                case ConsoleKey.I:
                case ConsoleKey.J:
                case ConsoleKey.K:
                case ConsoleKey.L:
                case ConsoleKey.M:
                case ConsoleKey.N:
                case ConsoleKey.O:
                case ConsoleKey.P:
                case ConsoleKey.Q:
                case ConsoleKey.R:
                case ConsoleKey.S:
                case ConsoleKey.T:
                case ConsoleKey.U:
                case ConsoleKey.V:
                case ConsoleKey.W:
                case ConsoleKey.X:
                case ConsoleKey.Y:
                case ConsoleKey.Z:
                case ConsoleKey.NumPad0:
                case ConsoleKey.NumPad1:
                case ConsoleKey.NumPad2:
                case ConsoleKey.NumPad3:
                case ConsoleKey.NumPad4:
                case ConsoleKey.NumPad5:
                case ConsoleKey.NumPad6:
                case ConsoleKey.NumPad7:
                case ConsoleKey.NumPad8:
                case ConsoleKey.NumPad9:
                case ConsoleKey.Add:
                case ConsoleKey.Subtract:
                case ConsoleKey.Multiply:
                case ConsoleKey.Divide:
                case ConsoleKey.Oem1:
                    maskStr += keyInfo.KeyChar;
                    Console.Write(passwordChar ?? keyInfo.KeyChar);
                    break;
                }
            }
        end:
            Console.WriteLine();
            return maskStr;
        }

        /// <summary>
        /// Read a SecureString from the Console
        /// </summary>
        /// <param name="passwordChar">char to display instead of real char. (char)0 : nothing, null : inputchar</param>
        /// <returns>ReadOnly SecureString</returns>
        public static SecureString ReadSecureString(char? passwordChar = default(char?))
        {
            var secureStr = new SecureString();
            while (true)
            {
                var keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                case ConsoleKey.Backspace:
                    if (secureStr.Length > 0)
                    {
                        secureStr.RemoveAt(secureStr.Length - 1);
                        if (passwordChar != null) Console.Write("\b \b");
                    }
                    break;
                case ConsoleKey.Enter:
                    goto end;

                case ConsoleKey.A:
                case ConsoleKey.B:
                case ConsoleKey.C:
                case ConsoleKey.D:
                case ConsoleKey.E:
                case ConsoleKey.F:
                case ConsoleKey.G:
                case ConsoleKey.H:
                case ConsoleKey.I:
                case ConsoleKey.J:
                case ConsoleKey.K:
                case ConsoleKey.L:
                case ConsoleKey.M:
                case ConsoleKey.N:
                case ConsoleKey.O:
                case ConsoleKey.P:
                case ConsoleKey.Q:
                case ConsoleKey.R:
                case ConsoleKey.S:
                case ConsoleKey.T:
                case ConsoleKey.U:
                case ConsoleKey.V:
                case ConsoleKey.W:
                case ConsoleKey.X:
                case ConsoleKey.Y:
                case ConsoleKey.Z:
                case ConsoleKey.NumPad0:
                case ConsoleKey.NumPad1:
                case ConsoleKey.NumPad2:
                case ConsoleKey.NumPad3:
                case ConsoleKey.NumPad4:
                case ConsoleKey.NumPad5:
                case ConsoleKey.NumPad6:
                case ConsoleKey.NumPad7:
                case ConsoleKey.NumPad8:
                case ConsoleKey.NumPad9:
                case ConsoleKey.Add:
                case ConsoleKey.Subtract:
                case ConsoleKey.Multiply:
                case ConsoleKey.Divide:
                case ConsoleKey.Oem1:
                    secureStr.AppendChar(keyInfo.KeyChar);
                    Console.Write(passwordChar ?? keyInfo.KeyChar);
                    break;
                }
            }
        end:
            Console.WriteLine();
            secureStr.MakeReadOnly();

            return secureStr;
        }

        /// <summary>
        /// Reads a password securely and returns a hash
        /// </summary>
        /// <param name="passwordChar">the char to show over the password</param>
        /// <param name="salt">optional salt</param>
        /// <param name="hashAlgorithm">hash algorithm to use, default is Sha256 </param>
        /// <param name="encoding">encoding to use before hashing, default is unicode</param>
        /// <returns>password has in byte[] format</returns>
        public static String ReadStringHashed(
            char? passwordChar = default(char?),
            byte[] salt = default(byte[]),
            HashAlgorithm hashAlgorithm = default(HashAlgorithm),
            Encoding encoding = default(Encoding))
        {
            // set default to Sha256
            if (null == hashAlgorithm) hashAlgorithm = SHA256.Create();
            // set default to unicode
            if (null == encoding) encoding = Encoding.Unicode;

            //SecureString secureStr = ReadSecureString(passwordChar);
            var unsecureStr =
                //secureStr.ToUnSecureString();
                ReadMaskString(passwordChar);

            var bHashed = Cryptor.ComputeHash(encoding.GetBytes(unsecureStr), salt, hashAlgorithm);
            return BitConverter.ToString(bHashed);
        }

    }
}
