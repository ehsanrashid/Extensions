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
        /// <param name="newName"> The new name. </param>
        /// <returns> The renamed file </returns>
        /// <example>
        ///   <code>var file = new FileInfo(@"c:\test.txt");
        ///     file.Rename("test2.txt");</code>
        /// </example>
        public static FileInfo Rename(this FileInfo fileInfo, String newName)
        {
            if (default(FileInfo) != fileInfo)
            {
                var pathFile = Path.Combine(Path.GetDirectoryName(fileInfo.FullName), newName);
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
        ///   <code>var file = new FileInfo(@"c:\test.txt");
        ///     file.RenameFileWithoutExtension("test3");</code>
        /// </example>
        public static FileInfo RenameFileWithoutExtension(this FileInfo fileInfo, String newName)
        {
            var fileName = String.Concat(newName, fileInfo.Extension);
            fileInfo.Rename(fileName);
            return fileInfo;
        }

        /// <summary>
        ///   Changes the files extension.
        /// </summary>
        /// <param name="fileInfo"> The file. </param>
        /// <param name="newExtension"> The new extension. </param>
        /// <returns> The renamed file </returns>
        /// <example>
        ///   <code>var file = new FileInfo(@"c:\test.txt");
        ///     file.ChangeExtension("xml");</code>
        /// </example>
        public static FileInfo ChangeExtension(this FileInfo fileInfo, String newExtension)
        {
            newExtension = newExtension.EnsureStartsWith(".");
            var fileName = String.Concat(Path.GetFileNameWithoutExtension(fileInfo.FullName), newExtension);
            fileInfo.Rename(fileName);
            return fileInfo;
        }
    }
}