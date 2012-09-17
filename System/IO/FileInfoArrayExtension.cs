using System.Collections.Generic;
using System.Linq;

namespace System.IO
{
    /// <summary>
    ///   Extension methods for the FileInfo-Array class
    /// </summary>
    public static class FileInfoArrayExtension
    {
        /// <summary>
        ///   Deletes several arrFileInfo at once and consolidates any exceptions.
        /// </summary>
        /// <param name="arrFileInfo"> The arrFileInfo. </param>
        /// <example>
        ///   <code>var arrFileInfo = directory.GetFiles("*.txt", "*.xml");
        ///     arrFileInfo.Delete()</code>
        /// </example>
        public static void Delete(this FileInfo[] arrFileInfo) { arrFileInfo.Delete(true); }

        /// <summary>
        ///   Deletes several arrFileInfo at once and optionally consolidates any exceptions.
        /// </summary>
        /// <param name="arrFileInfo"> The arrFileInfo. </param>
        /// <param name="consolidateExceptions"> if set to <c>true</c> exceptions are consolidated and the processing is not interrupted. </param>
        /// <example>
        ///   <code>var arrFileInfo = directory.GetFiles("*.txt", "*.xml");
        ///     arrFileInfo.Delete()</code>
        /// </example>
        public static void Delete(this FileInfo[] arrFileInfo, bool consolidateExceptions)
        {
            if (consolidateExceptions)
            {
                var exceptions = new List<Exception>();
                foreach (var file in arrFileInfo)
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception e)
                    {
                        exceptions.Add(e);
                    }
                if (exceptions.Any())
                    throw CombinedException.Combine(
                        "Error while deleting one or several arrFileInfo, see InnerExceptions array for details.",
                        exceptions);
            }
            else foreach (var file in arrFileInfo) file.Delete();
        }

        /// <summary>
        ///   Copies several arrFileInfo to a new folder at once and consolidates any exceptions.
        /// </summary>
        /// <param name="arrFileInfo"> The arrFileInfo. </param>
        /// <param name="targetPath"> The target path. </param>
        /// <returns> The newly created file copies </returns>
        /// <example>
        ///   <code>var arrFileInfo = directory.GetFiles("*.txt", "*.xml");
        ///     var copiedFiles = arrFileInfo.CopyTo(@"c:\temp\");</code>
        /// </example>
        public static FileInfo[] CopyTo(this FileInfo[] arrFileInfo, String targetPath) { return arrFileInfo.CopyTo(targetPath, true); }

        /// <summary>
        ///   Copies several arrFileInfo to a new folder at once and optionally consolidates any exceptions.
        /// </summary>
        /// <param name="arrFileInfo"> The arrFileInfo. </param>
        /// <param name="targetPath"> The target path. </param>
        /// <param name="consolidateExceptions"> if set to <c>true</c> exceptions are consolidated and the processing is not interrupted. </param>
        /// <returns> The newly created file copies </returns>
        /// <example>
        ///   <code>var arrFileInfo = directory.GetFiles("*.txt", "*.xml");
        ///     var copiedFiles = arrFileInfo.CopyTo(@"c:\temp\");</code>
        /// </example>
        public static FileInfo[] CopyTo(this FileInfo[] arrFileInfo, String targetPath, bool consolidateExceptions)
        {
            var copiedfiles = new List<FileInfo>();
            List<Exception> exceptions = null;
            foreach (var file in arrFileInfo)
                try
                {
                    var fileName = Path.Combine(targetPath, file.Name);
                    copiedfiles.Add(file.CopyTo(fileName));
                }
                catch (Exception e)
                {
                    if (consolidateExceptions)
                    {
                        if (exceptions == null) exceptions = new List<Exception>();
                        exceptions.Add(e);
                    }
                    else throw;
                }
            if ((exceptions != null) && (exceptions.Count > 0))
                throw new CombinedException(
                    "Error while copying one or several arrFileInfo, see InnerExceptions array for details.",
                    exceptions.ToArray());
            return copiedfiles.ToArray();
        }

        /// <summary>
        ///   Moves several arrFileInfo to a new folder at once and optionally consolidates any exceptions.
        /// </summary>
        /// <param name="arrFileInfo"> The arrFileInfo. </param>
        /// <param name="targetPath"> The target path. </param>
        /// <returns> The moved arrFileInfo </returns>
        /// <example>
        ///   <code>var arrFileInfo = directory.GetFiles("*.txt", "*.xml");
        ///     arrFileInfo.MoveTo(@"c:\temp\");</code>
        /// </example>
        public static FileInfo[] MoveTo(this FileInfo[] arrFileInfo, String targetPath)
        {
            return arrFileInfo.MoveTo(targetPath, true);
        }

        /// <summary>
        ///   Movies several arrFileInfo to a new folder at once and optionally consolidates any exceptions.
        /// </summary>
        /// <param name="arrFileInfo"> The arrFileInfo. </param>
        /// <param name="targetPath"> The target path. </param>
        /// <param name="consolidateExceptions"> if set to <c>true</c> exceptions are consolidated and the processing is not interrupted. </param>
        /// <returns> The moved arrFileInfo </returns>
        /// <example>
        ///   <code>var arrFileInfo = directory.GetFiles("*.txt", "*.xml");
        ///     arrFileInfo.MoveTo(@"c:\temp\");</code>
        /// </example>
        public static FileInfo[] MoveTo(this FileInfo[] arrFileInfo, String targetPath, bool consolidateExceptions)
        {
            List<Exception> exceptions = null;
            foreach (var file in arrFileInfo)
                try
                {
                    var fileName = Path.Combine(targetPath, file.Name);
                    file.MoveTo(fileName);
                }
                catch (Exception e)
                {
                    if (consolidateExceptions)
                    {
                        if (exceptions == null) exceptions = new List<Exception>();
                        exceptions.Add(e);
                    }
                    else throw;
                }
            if ((exceptions != null) && (exceptions.Count > 0))
                throw new CombinedException(
                    "Error while moving one or several arrFileInfo, see InnerExceptions array for details.",
                    exceptions.ToArray());
            return arrFileInfo;
        }

        /// <summary>
        ///   Sets file attributes for several arrFileInfo at once
        /// </summary>
        /// <param name="arrFileInfo"> The arrFileInfo. </param>
        /// <param name="attributes"> The attributes to be set. </param>
        /// <returns> The changed arrFileInfo </returns>
        /// <example>
        ///   <code>var arrFileInfo = directory.GetFiles("*.txt", "*.xml");
        ///     arrFileInfo.SetAttributes(FileAttributes.Archive);</code>
        /// </example>
        public static FileInfo[] SetAttributes(this FileInfo[] arrFileInfo, FileAttributes attributes)
        {
            foreach (var file in arrFileInfo) file.Attributes = attributes;
            return arrFileInfo;
        }

        /// <summary>
        ///   Appends file attributes for several arrFileInfo at once (additive to any existing attributes)
        /// </summary>
        /// <param name="arrFileInfo"> The arrFileInfo. </param>
        /// <param name="attributes"> The attributes to be set. </param>
        /// <returns> The changed arrFileInfo </returns>
        /// <example>
        ///   <code>var arrFileInfo = directory.GetFiles("*.txt", "*.xml");
        ///     arrFileInfo.SetAttributesAdditive(FileAttributes.Archive);</code>
        /// </example>
        public static FileInfo[] SetAttributesAdditive(this FileInfo[] arrFileInfo, FileAttributes attributes)
        {
            foreach (var file in arrFileInfo) file.Attributes = (file.Attributes | attributes);
            return arrFileInfo;
        }

        /// <summary>
        ///   Changes the extensions of several arrFileInfo at once.
        /// </summary>
        /// <param name="arrFileInfo"> The arrFileInfo. </param>
        /// <param name="newExtension"> The new extension. </param>
        /// <returns> The renamed arrFileInfo </returns>
        /// <example>
        ///   <code>var arrFileInfo = directory.GetFiles("*.txt", "*.xml");
        ///     arrFileInfo.ChangeExtensions("tmp");</code>
        /// </example>
        public static FileInfo[] ChangeExtensions(this FileInfo[] arrFileInfo, String newExtension)
        {
            arrFileInfo.ForEach(f => f.ChangeExtension(newExtension));
            return arrFileInfo;
        }
    }
}