namespace System.Collections.Generic
{
    using Threading;
    using Diagnostics;
    using Runtime.InteropServices;

    #region Summary
    /*
    Usage example, based on a typical producer-consumer scenario.
    Note that a <tt>BlockingQueue</tt> can safely be used with multiple
    producers and multiple consumers.
    <pre>
    class Producer implements Runnable {
      private final BlockingQueue queue;
      Producer(BlockingQueue q) { queue = q; }
      public void run() {
        try {
          while (true) { queue.Enqueue(produce()); }
        } catch (InterruptedException ex) { ... handle ...}
      }
      Object produce() { ... }
    }
    
    class Consumer implements Runnable {
      private final BlockingQueue queue;
      Consumer(BlockingQueue q) { queue = q; }
      public void run() {
        try {
          while (true) { consume(queue.Dequeue()); }
        } catch (InterruptedException ex) { ... handle ...}
      }
      void consume(Object x) { ... }
    }
    
    class Setup {
      void main() {
        BlockingQueue q = new SomeQueueImplementation();
        Producer p = new Producer(q);
        Consumer c1 = new Consumer(q);
        Consumer c2 = new Consumer(q);
        new Thread(p).start();
        new Thread(c1).start();
        new Thread(c2).start();
      }
    }
    */
    #endregion

    [Serializable, ComVisible(false), DebuggerDisplay("Count = {Count}")]
    public sealed class BlockingQueue<T> : Queue<T>
    {
        public const int DefaultTimeout = 5000; //msec

        public TimeSpan Timeout = TimeSpan.FromMilliseconds(DefaultTimeout);
        public bool ExitContext = false;

        readonly Object _locker = new Object();

        #region Constructors
        public BlockingQueue(IEnumerable<T> collection, TimeSpan timeout, bool exitContext = false)
            : base(collection)
        {
            Timeout = timeout;
            ExitContext = exitContext;
        }

        public BlockingQueue(IEnumerable<T> collection, int msecTimeout = DefaultTimeout, bool exitContext = false)
            : this(collection, TimeSpan.FromMilliseconds(msecTimeout), exitContext)
        {
        }

        public BlockingQueue(int capacity, TimeSpan timeout, bool exitContext = false)
            : base(capacity)
        {
            Timeout = timeout;
            ExitContext = exitContext;
        }

        public BlockingQueue(int capacity, int msecTimeout = DefaultTimeout, bool exitContext = false)
            : this(capacity, TimeSpan.FromMilliseconds(msecTimeout), exitContext)
        {
        }

        public BlockingQueue(TimeSpan timeout, bool exitContext = false)
        {
            Timeout = timeout;
            ExitContext = exitContext;
        }

        public BlockingQueue(int msecTimeout = DefaultTimeout, bool exitContext = false)
            : this(TimeSpan.FromMilliseconds(msecTimeout), exitContext)
        {
        }
        #endregion

        public new void Enqueue(T item)
        {
            // List is unlimited so no check for Overflow
            //if (null == item) throw new ArgumentNullException("item");
            lock (_locker)
            {
                base.Enqueue(item);
                Monitor.Pulse(_locker);
            }
        }

        public new T Dequeue()
        {
            lock (_locker)
            {
                while (Count == 0)
                {
                    Monitor.Wait(_locker, Timeout);
                    if (ExitContext)
                    {
                        if (Count == 0) return default(T);
                        break;
                    }
                }
                return base.Dequeue();
            }
        }

        public void Enqueues(IEnumerable<T> enumerable)
        {
            //foreach (var item in enumerable)
            //    Enqueue(item);
            //-------------------------------
            enumerable.ForEach(Enqueue);
        }

        //public int Dequeues(int count, out IList<T> collection)
        //{
        //    collection = new List<T>(count);
        //    for (var i = 0; i < count; i++)
        //    {
        //        lock (locker)
        //        {
        //            if (Count == 0)
        //                return i;
        //            collection.Add(base.Dequeue());
        //        }
        //    }
        //    return count;
        //}

        public IEnumerable<T> Dequeues(int times)
        {
            lock (_locker)
            {
                for (var i = 0; i < times; ++i)
                {
                    if (Count == 0) break;
                    yield return base.Dequeue();
                }
            }
        }

        public IEnumerable<T> Dequeues()
        {
            lock (_locker)
            {
                while (Count != 0) yield return base.Dequeue();
            }
        }

    }
}