using System;
using System.IO;

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Threading;
using System.Security.AccessControl;


namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //String s1 = "hello world";
            //String s2 = "1234.7";

            //Console.WriteLine(s1);

            //bool b1 = s1.IsNumber(true);
            //Console.WriteLine(b1);
            //bool b2 = s2.IsNumber(true);
            //Console.WriteLine(b2);

            //var s = s1.Replace(new[] { "e", "o", "d" }, (item) => "-");
            //var s = s1.Replace(new[] { "e", "o", "d" }, "-");

            //Console.WriteLine(s);

            //var ss1 = s1.Remove(new[] { "e", "o", "d" });
            //var ss2 = s1.Remove(new[] { 'e', 'o', 'd' });

            //var s = s1.ToSecureString();
            //Console.WriteLine(ss1);
            //Console.WriteLine(ss2);

            //var name = @"D:\test.txt";
            //FileInfo file = new FileInfo(name);

            //DirectoryInfo dir = new DirectoryInfo(@"D:\temp");
            //dir.Delete();

            //file.TurnOnReadOnlyFlag();
            ///file.SetAttributes(FileAttributes.Temporary| FileAttributes.Normal);
            //file.RenameWithoutExtension("test2");

            //var e = new[] { 1, 3, 5, 6, 7, 4, 5, 3, 6, 8, 3 };
            //var e = Enumerable.Range(2, 19);
            //PagedList<int> pl = new PagedList<int>(e, 7, 3);

            //Console.WriteLine(pl.HasPrevPage);
            //Console.WriteLine(pl.HasNextPage);
            //Console.WriteLine(pl.NoOfItems);
            //Console.WriteLine(pl.NoOfPages);

			//Graph<int> graph;


			//Tree<int> tree = new Tree<int>(3);
			

            Console.Read();

        }

        protected void ExecuteThread<T>(Action<T> action, T parameters, int maxStackSize = 0)
        {
            var thread = new Thread(() => action(parameters), maxStackSize);
            thread.Start();
            thread.Join();
        }




        // Adds an ACL entry on the specified file for the specified account.
        public static void AddFileSecurity(String fileName, String account, FileSystemRights rights, AccessControlType controlType)
        {
            // Get a FileSecurity object that represents the
            // current security settings.
            FileSecurity fSecurity = File.GetAccessControl(fileName);

            // Add the FileSystemAccessRule to the security settings.
            fSecurity.AddAccessRule(new FileSystemAccessRule(account, rights, controlType));

            // Set the new access settings.
            File.SetAccessControl(fileName, fSecurity);
        }

        // Removes an ACL entry on the specified file for the specified account.
        public static void RemoveFileSecurity(String fileName, String account, FileSystemRights rights, AccessControlType controlType)
        {
            // Get a FileSecurity object that represents the
            // current security settings.
            FileSecurity fSecurity = File.GetAccessControl(fileName);

            // Remove the FileSystemAccessRule from the security settings.
            fSecurity.RemoveAccessRule(new FileSystemAccessRule(account, rights, controlType));

            // Set the new access settings.
            File.SetAccessControl(fileName, fSecurity);
        }

    }
}
