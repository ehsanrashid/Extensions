namespace System.IO
{
    /// <summary>
    ///   Extension methods for the FileInfo class
    /// </summary>
    public static class FileInfoExtension
    {
        /// <summary>
        ///   Renames a file.
        /// </summary>
        /// <param name="fileInfo"> The file. </param>
        /// <param name="newName_ext"> The new name. </param>
        /// <returns> The renamed file </returns>
        /// <example>
        ///   <code>var file = new FileInfo(@"c:\test.txt");
        ///     file.Rename("test2.txt");</code>
        /// </example>
        public static FileInfo Rename(this FileInfo fileInfo, String newName_ext)
        {
            if (default(FileInfo) != fileInfo)
            {
                var pathFile = Path.Combine(Path.GetDirectoryName(fileInfo.FullName), newName_ext);
                fileInfo.MoveTo(pathFile);
            }
            return fileInfo;
        }

        /// <summary>
        ///   Renames a without changing its extension.
        /// </summary>
        /// <param name="fileInfo"> The file. </param>
        /// <param name="newName"> The new name. </param>
        /// <returns> The renamed file </returns>
        /// <example>
        ///   <code>
        ///     var file = new FileInfo(@"c:\test.txt");
        ///     file.RenameFileWithoutExtension("test3");
        ///   </code>
        /// </example>
        public static FileInfo RenameWithoutExtension(this FileInfo fileInfo, String newName)
        {
            if (default(FileInfo) != fileInfo)
            {
                var newFileName = String.Concat(newName, fileInfo.Extension);
                fileInfo.Rename(newFileName);
            }
            return fileInfo;
        }

        /// <summary>
        ///   Changes the files extension.
        /// </summary>
        /// <param name="fileInfo"> The file. </param>
        /// <param name="newExt"> The new extension. </param>
        /// <returns> The renamed file </returns>
        /// <example>
        ///   <code>
        ///     var file = new FileInfo(@"c:\test.txt");
        ///     file.ChangeExtension("xml");
        ///   </code>
        /// </example>
        public static FileInfo ChangeExtension(this FileInfo fileInfo, String newExt)
        {
            if (default(FileInfo) != fileInfo)
            {
                newExt = newExt.EnsureStartsWith(".");
                var newFileName = String.Concat(Path.GetFileNameWithoutExtension(fileInfo.FullName), newExt);
                fileInfo.Rename(newFileName);
            }
            return fileInfo;
        }


        public static void SetAttributes(this FileInfo fileInfo, FileAttributes attribs)
        {
            if (default(FileInfo) != fileInfo) File.SetAttributes(fileInfo.FullName, attribs);
        }


        /// <summary> Turns off the read only flag on a file. </summary>
        /// <param name = "fileInfo"> The file. </param>
        /// <returns> Returns true if the read only flag was set. </returns>
        public static bool TurnOffReadOnly(this FileInfo fileInfo)
        {
            var attribs = fileInfo.Attributes;
            if (FileAttributes.ReadOnly == (attribs & FileAttributes.ReadOnly))
            {
                File.SetAttributes(fileInfo.FullName, attribs & ~FileAttributes.ReadOnly);
                return true;
            }
            return false;
        }

        /// <summary>
        ///   Turns on the read only flag for a file.
        /// </summary>
        /// <param name = "fileInfo"> The file. </param>
        public static bool TurnOnReadOnly(this FileInfo fileInfo)
        {
            var attribs = fileInfo.Attributes;
            if (FileAttributes.ReadOnly != (attribs & FileAttributes.ReadOnly))
            {
                File.SetAttributes(fileInfo.FullName, attribs | FileAttributes.ReadOnly);
                return true;
            }
            return false;
        }

    }
}