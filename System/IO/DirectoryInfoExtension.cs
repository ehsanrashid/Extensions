using System.Collections.Generic;
using System.Linq;

namespace System.IO
{
    /// <summary>
    ///   Extension methods for the DirectoryInfo class
    /// </summary>
    public static class DirectoryInfoExtension
    {
        /// <summary>
        ///   Gets all files in the directory matching one of the several (!) supplied patterns (instead of just one in the regular implementation).
        /// </summary>
        /// <param name="directory"> The directory. </param>
        /// <param name="patterns"> The patterns. </param>
        /// <returns> The matching files </returns>
        /// <remarks>
        ///   This methods is quite perfect to be used in conjunction with the newly created FileInfo-Array extension methods.
        /// </remarks>
        /// <example>
        ///   <code>var files = directory.GetFiles("*.txt", "*.xml");</code>
        /// </example>
        public static FileInfo[] GetFiles(this DirectoryInfo directory, params String[] patterns)
        {
            var files = new List<FileInfo>();
            foreach (var pattern in patterns) files.AddRange(directory.GetFiles(pattern));
            return files.ToArray();
        }

        /// <summary>
        ///   Searches the provided directory recursively and returns the first file matching to the provided predicate.
        /// </summary>
        /// <param name="directory"> The directory. </param>
        /// <param name="predicate"> The predicate. </param>
        /// <returns> The found file </returns>
        /// <example>
        ///   <code>var directory = new DirectoryInfo(@"c:\");
        ///     var file = directory.FindFileRecursive(f => f.Extension == ".ini");</code>
        /// </example>
        public static FileInfo FindFileRecursive(this DirectoryInfo directory, Func<FileInfo, bool> predicate)
        {
            //foreach (var file in directory.GetFiles())
            //{
            //    if (predicate(file))
            //        return file;
            //}
            foreach (var file in directory.GetFiles().Where(predicate)) return file;
            //foreach (var subDirectory in directory.GetDirectories())
            //{
            //    var foundFile = subDirectory.FindFileRecursive(predicate);
            //    if (foundFile != null)
            //        return foundFile;
            //}
            //return null;
            return
                directory.GetDirectories().Select(subDir => subDir.FindFileRecursive(predicate)).FirstOrDefault(
                    fileInfo => fileInfo != null);
        }

        /// <summary>
        ///   Searches the provided directory recursively and returns the first file matching the provided pattern.
        /// </summary>
        /// <param name="directory"> The directory. </param>
        /// <param name="pattern"> The pattern. </param>
        /// <returns> The found file </returns>
        /// <example>
        ///   <code>var directory = new DirectoryInfo(@"c:\");
        ///     var file = directory.FindFileRecursive("win.ini");</code>
        /// </example>
        public static FileInfo FindFileRecursive(this DirectoryInfo directory, String pattern)
        {
            var files = directory.GetFiles(pattern);
            return (files.Length > 0)
                       ? files[0]
                       : directory.GetDirectories().Select(subDirectory => subDirectory.FindFileRecursive(pattern)).
                             FirstOrDefault(foundFile => foundFile != null);
        }

        /// <summary>
        ///   Searches the provided directory recursively and returns the all files matching to the provided predicate.
        /// </summary>
        /// <param name="directory"> The directory. </param>
        /// <param name="predicate"> The predicate. </param>
        /// <returns> The found files </returns>
        /// <remarks>
        ///   This methods is quite perfect to be used in conjunction with the newly created FileInfo-Array extension methods.
        /// </remarks>
        /// <example>
        ///   <code>var directory = new DirectoryInfo(@"c:\");
        ///     var files = directory.FindFilesRecursive(f => f.Extension == ".ini");</code>
        /// </example>
        public static FileInfo[] FindFilesRecursive(this DirectoryInfo directory, Func<FileInfo, bool> predicate)
        {
            var foundFiles = new List<FileInfo>();
            FindFilesRecursive(directory, predicate, foundFiles);
            return foundFiles.ToArray();
        }

        static void FindFilesRecursive(DirectoryInfo directory, Func<FileInfo, bool> predicate,
                                       List<FileInfo> foundFiles)
        {
            foundFiles.AddRange(directory.GetFiles().Where(predicate));
            directory.GetDirectories().ForEach(d => FindFilesRecursive(d, predicate, foundFiles));
        }

        /// <summary>
        ///   Searches the provided directory recursively and returns the all files matching the provided pattern.
        /// </summary>
        /// <param name="directory"> The directory. </param>
        /// <param name="pattern"> The pattern. </param>
        /// <remarks>
        ///   This methods is quite perfect to be used in conjunction with the newly created FileInfo-Array extension methods.
        /// </remarks>
        /// <returns> The found files </returns>
        /// <example>
        ///   <code>var directory = new DirectoryInfo(@"c:\");
        ///     var files = directory.FindFilesRecursive("*.ini");</code>
        /// </example>
        public static FileInfo[] FindFilesRecursive(this DirectoryInfo directory, String pattern)
        {
            var foundFiles = new List<FileInfo>();
            FindFilesRecursive(directory, pattern, foundFiles);
            return foundFiles.ToArray();
        }

        static void FindFilesRecursive(DirectoryInfo directory, String pattern, List<FileInfo> foundFiles)
        {
            foundFiles.AddRange(directory.GetFiles(pattern));
            directory.GetDirectories().ForEach(d => FindFilesRecursive(d, pattern, foundFiles));
        }

        /// <summary>
        ///   Copies the entire directory to another one
        /// </summary>
        /// <param name="dirSource"> The source directory. </param>
        /// <param name="pathDirTarget"> The target directory path. </param>
        /// <returns> </returns>
        public static DirectoryInfo CopyTo(this DirectoryInfo dirSource, String pathDirTarget)
        {
            var targetDirectory = new DirectoryInfo(pathDirTarget);
            CopyTo(dirSource, targetDirectory);
            return targetDirectory;
        }

        /// <summary>
        ///   Copies the entire directory to another one
        /// </summary>
        /// <param name="dirSource"> The source directory. </param>
        /// <param name="dirTarget"> The target directory. </param>
        public static void CopyTo(this DirectoryInfo dirSource, DirectoryInfo dirTarget)
        {
            if (!dirTarget.Exists) dirTarget.Create();
            foreach (var childDirectory in dirSource.GetDirectories()) CopyTo(childDirectory, Path.Combine(dirTarget.FullName, childDirectory.Name));
            foreach (var file in dirSource.GetFiles()) file.CopyTo(Path.Combine(dirTarget.FullName, file.Name));
        }
    }
}