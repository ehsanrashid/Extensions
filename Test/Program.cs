using System;
using System.IO;

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

            //var ss1 = s1.Remove(new[] {"e", "o", "d"});
            //var ss2 = s1.Remove(new[] { 'e', 'o', 'd' });

            //var s = s1.ToSecureString();
            //Console.WriteLine(ss1);
            //Console.WriteLine(ss2);

            var name = @"D:\test.txt";
            FileInfo file = new FileInfo(name);

            DirectoryInfo dir = new DirectoryInfo(@"D:\temp");
            dir.Delete();

            //file.TurnOnReadOnlyFlag();
            ///file.SetAttributes(FileAttributes.Temporary| FileAttributes.Normal);
            //file.RenameWithoutExtension("test2");

            Console.Read();

        }
    }
}
