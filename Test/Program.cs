using System;
using System.IO;

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Threading;
using System.Security.AccessControl;


namespace Test
{

    using System;
    using System.Threading;
    using System.Collections.Generic;

    public class PCQueue
    {
        readonly object _locker = new object();
        Thread[] _workers;
        Queue<Action> _itemQ = new Queue<Action>();

        public PCQueue(int workerCount)
        {
            _workers = new Thread[workerCount];

            // Create and start a separate thread for each worker
            for (int i = 0; i < workerCount; ++i)
            {
                (_workers[i] = new Thread(new ThreadStart(Consume))).Start();
            }
        }

        public void Shutdown(bool waitForWorkers)
        {
            // Enqueue one null item per worker to make each exit.
            foreach (Thread worker in _workers)
                Produce(null);

            // Wait for workers to finish
            if (waitForWorkers)
            {
                foreach (Thread worker in _workers)
                    worker.Join();
            }
        }

        // Enqueue
        public void Produce(Action item)
        {
            lock (_locker)
            {
                _itemQ.Enqueue(item);           // We must pulse because we're
                Monitor.Pulse(_locker);         // changing a blocking condition.
            }
        }

        // Dequeue
        void Consume()
        {
            while (true)                        // Keep consuming until
            {                                   // told otherwise.
                Action item = null;
                lock (_locker)
                {
                    while (_itemQ.Count == 0) Monitor.Wait(_locker);
                    item = _itemQ.Dequeue();
                }
                if (item != null) item();        // Execute item.
            }
        }
    }


    static class Program
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

            PCQueue q = new PCQueue(2);

            Console.WriteLine("Enqueuing 10 items...");

            for (int i = 0; i < 10; i++)
            {
                int itemNumber = i;      // To avoid the captured variable trap
                q.Produce(() =>
                {
                    Thread.Sleep(1000);          // Simulate time-consuming work
                    Console.Write(" Task" + itemNumber);
                });
            }

            q.Shutdown(true);
            Console.WriteLine();
            Console.WriteLine("Workers complete!");

            Console.Read();

        }



        public static void ExecuteThread<T>(Action<T> action, T parameters, int maxStackSize = 0)
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
