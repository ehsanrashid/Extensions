using System.Collections.Generic;
using System.Linq;

namespace System.IO
{
    /// <summary>
    ///   Extension methods for the DirectoryInfo class
    /// </summary>
    public static class DirectoryInfoExtension
    {
        #region Find Files

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
        public static FileInfo[] FindFiles(this DirectoryInfo directory, params String[] patterns)
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
        ///     var file = directory.FindFile(f => f.Extension == ".ini");</code>
        /// </example>
        public static FileInfo FindFile(this DirectoryInfo directory, Func<FileInfo, bool> predicate)
        {
            //foreach (var file in directory.GetFiles())
            //{
            //    if (predicate(file))
            //        return file;
            //}

            // --- return First() file found;
            foreach (var file in directory.GetFiles().Where(predicate)) return file;

            //foreach (var subDirectory in directory.GetDirectories())
            //{
            //    var foundFile = subDirectory.FindFile(predicate);
            //    if (foundFile != null)
            //        return foundFile;
            //}
            //return null;

            return
                directory.GetDirectories()
                    .Select(subDir => subDir.FindFile(predicate))
                    .FirstOrDefault((fileInfo) => null != fileInfo);
        }

        /// <summary>
        ///   Searches the provided directory recursively and returns the first file matching the provided pattern.
        /// </summary>
        /// <param name="directory"> The directory. </param>
        /// <param name="pattern"> The pattern. </param>
        /// <returns> The found file </returns>
        /// <example>
        ///   <code>var directory = new DirectoryInfo(@"c:\");
        ///     var file = directory.FindFile("win.ini");</code>
        /// </example>
        public static FileInfo FindFile(this DirectoryInfo directory, String pattern)
        {
            var files = directory.GetFiles(pattern);

            return (files.Length > 0)
                       ? files[0]
                       : directory.GetDirectories()
                             .Select(subDirectory => subDirectory.FindFile(pattern))
                             .FirstOrDefault((fileInfo) => null != fileInfo);
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
        ///     var files = directory.FindFiles(f => f.Extension == ".ini");</code>
        /// </example>
        public static FileInfo[] FindFiles(this DirectoryInfo directory, Func<FileInfo, bool> predicate)
        {
            var listFiles = new List<FileInfo>();
            FindFiles(directory, predicate, listFiles);
            return listFiles.ToArray();
        }

        static void FindFiles(DirectoryInfo directory, Func<FileInfo, bool> predicate,
                              List<FileInfo> listFiles)
        {
            listFiles.AddRange(directory.GetFiles().Where(predicate));
            directory.GetDirectories().ForEach(d => FindFiles(d, predicate, listFiles));
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
        ///     var files = directory.FindFiles("*.ini");</code>
        /// </example>
        public static FileInfo[] FindFiles(this DirectoryInfo directory, String pattern)
        {
            var listFiles = new List<FileInfo>();
            FindFiles(directory, pattern, listFiles);
            return listFiles.ToArray();
        }

        static void FindFiles(DirectoryInfo directory, String pattern, List<FileInfo> listFiles)
        {
            //listFiles.AddRange(directory.GetFiles(pattern));
            //directory.GetDirectories().ForEach(dir => FindFiles(dir, pattern, listFiles));
            //---------------------------
            listFiles.AddRange(directory.GetFiles(pattern, SearchOption.AllDirectories));
        }

        #endregion

        #region Find SubDirectories

        public static DirectoryInfo FindSubDirectory(this DirectoryInfo directory, Func<DirectoryInfo, bool> predicate)
        {
            //foreach (var dir in directory.GetDirectories())
            //{
            //    if (predicate(dir))
            //        return dir;
            //}

            // --- return First() file found;
            foreach (var dir in directory.GetDirectories().Where(predicate)) return dir;

            //foreach (var subDir in directory.GetDirectories())
            //{
            //    var foundDir = subDir.FindSubDirectory(predicate);
            //    if (foundDir != null)
            //        return foundDir;
            //}
            //return null;

            return
                directory.GetDirectories()
                    .Select(subDir => subDir.FindSubDirectory(predicate))
                    .FirstOrDefault((dirInfo) => null != dirInfo);
        }

        public static DirectoryInfo FindSubDirectory(this DirectoryInfo directory, String pattern)
        {
            var dirs = directory.GetDirectories(pattern);

            return (dirs.Length > 0)
                       ? dirs[0]
                       : directory.GetDirectories()
                             .Select(subDir => subDir.FindSubDirectory(pattern))
                             .FirstOrDefault((dirInfo) => null != dirInfo);
        }

        public static DirectoryInfo[] FindSubDirectories(this DirectoryInfo directory, Func<DirectoryInfo, bool> predicate)
        {
            var listDirs = new List<DirectoryInfo>();
            FindSubDirectories(directory, predicate, listDirs);
            return listDirs.ToArray();
        }

        static void FindSubDirectories(DirectoryInfo directory, Func<DirectoryInfo, bool> predicate,
                                       List<DirectoryInfo> listDirs)
        {
            listDirs.AddRange(directory.GetDirectories().Where(predicate));
            directory.GetDirectories().ForEach(subDir => FindSubDirectories(subDir, predicate, listDirs));
        }

        public static DirectoryInfo[] FindSubDirectories(this DirectoryInfo directory, String pattern)
        {
            var listDirs = new List<DirectoryInfo>();
            FindSubDirectories(directory, pattern, listDirs);
            return listDirs.ToArray();
        }

        static void FindSubDirectories(DirectoryInfo directory, String pattern, List<DirectoryInfo> listDirs)
        {
            //listDirs.AddRange(directory.FindSubDirectories(pattern));
            //directory.GetDirectories()
            //    .ForEach(subDir => FindSubDirectories(subDir, pattern, listDirs));
            //---------------------------
            listDirs.AddRange(directory.GetDirectories(pattern, SearchOption.AllDirectories));
        }

        #endregion


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


        #region extra

        //public static void RemoveSubDirectories(String directory, String searchPattern)
        //{
        //    if (!Directory.Exists(directory)) return;

        //    var directories = Directory.GetDirectories(directory, searchPattern, SearchOption.AllDirectories);
        //    foreach (var dir in directories)
        //        Delete(dir);
        //}

        //public static void RemoveSubDirectories(String directory, params String[] searchPatterns)
        //{
        //    foreach (var pattern in searchPatterns)
        //        RemoveSubDirectories(directory, pattern);
        //}

        //public static void RemoveSubDirectories(String directory, IEnumerable<String> searchPatterns)
        //{
        //    foreach (var pattern in searchPatterns)
        //        RemoveSubDirectories(directory, pattern);
        //}


        //public static void CopyDirectory(String source, String dest, bool subdirs, bool removeIfExists)
        //{
        //    var dir = new DirectoryInfo(source);
        //    // If the source directory does not exist, throw an exception.
        //    if (!dir.Exists) throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + source);
        //    // Removes the directory if it already exists
        //    if (removeIfExists) Delete(dest);

        //    // If the destination directory does not exist, create it.
        //    if (!Directory.Exists(dest)) Directory.CreateDirectory(dest);

        //    // Get the file contents of the directory to copy.
        //    var files = dir.GetFiles();

        //    foreach (var file in files)
        //    {
        //        // Create the path to the new copy of the file.
        //        var temppath = Path.Combine(dest, file.Name);
        //        // Copy the file.
        //        file.CopyTo(temppath, false);
        //    }

        //    // If subdirs is true, copy the subdirectories.
        //    if (subdirs)
        //    {
        //        var dirs = dir.GetDirectories();
        //        foreach (var subdir in dirs)
        //        {
        //            // Create the subdirectory.
        //            var temppath = Path.Combine(dest, subdir.Name);

        //            // Copy the subdirectories.
        //            CopyDirectory(subdir.FullName, temppath, true, removeIfExists);
        //        }
        //    }
        //}


        //public static void Delete(String directory)
        //{
        //    if (!Directory.Exists(directory)) return;

        //    try
        //    {
        //        FileHelper.DeleteFiles(directory);
        //        var retry = 0;

        //        // Sometimes you encounter a directory is not empty error immediately after deleting all the files.
        //        // This loop will retry 3 times
        //        while (retry < 3)
        //        {
        //            try
        //            {
        //                Directory.Delete(directory, true);
        //                break;
        //            }
        //            catch (IOException)
        //            {
        //                retry++;
        //                if (retry > 3)
        //                {
        //                    throw;
        //                }
        //            }
        //        }
        //    }
        //    catch (IOException ioException)
        //    {
        //        throw new ApplicationException(String.Format("Error removing directory {0}: {1}", directory, ioException.Message));
        //    }
        //}

        
        #endregion
    
    
    }

    public class RecursiveSearchHelper
    {
        readonly List<string> excludeList;

        readonly List<string> fileList;

        public RecursiveSearchHelper()
        {
            fileList = new List<string>();
            excludeList = new List<string>();
        }

        public string[] GetFiles(string initialDirectory, string filePattern)
        {
            fileList.Clear();

            Search(initialDirectory, filePattern);

            return fileList.ToArray();
        }

        public string[] GetFiles(string initialDirectory, string[] filePatterns, string[] excludePatterns)
        {
            fileList.Clear();
            excludeList.Clear();

            foreach (var filePattern in filePatterns) Search(initialDirectory, filePattern);

            if (excludePatterns != null) foreach (var excludePattern in excludePatterns) SearchExclude(initialDirectory, excludePattern);

            foreach (var file in excludeList) fileList.RemoveAll(s => s == file);

            return fileList.ToArray();
        }

        void Search(string initialDirectory, string filePattern)
        {
            foreach (var file in Directory.GetFiles(initialDirectory, filePattern).Where(file => !fileList.Contains(file))) fileList.Add(file);

            foreach (var item in Directory.GetDirectories(initialDirectory)) Search(item, filePattern);
        }

        void SearchExclude(string initialDirectory, string excludePattern)
        {
            foreach (
                var file in Directory.GetFiles(initialDirectory, excludePattern).Where(file => !excludeList.Contains(file))) excludeList.Add(file);

            foreach (var item in Directory.GetDirectories(initialDirectory)) SearchExclude(item, excludePattern);
        }

    }
}