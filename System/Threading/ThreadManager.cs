/*
    How to use Thread Classs
    * 
    * ==============
    * public ELMService()
       {
           InitializeComponent();
           etm.ClalThreadPool("EmailThreads", (uint)ApplicationInfo.EmailParsingThreads);
       } 
    * //queue items in thread for processing.
    * etm.QueueUserEmailWorkItem(new WaitCallback(ELMService_UpdateLogHandler), objSupplier);
      public void ELMService_UpdateLogHandler(Object objmsg)
    * {
    *      return;
    * }
    * 
    */

namespace System.Threading
{
    using Collections;

    public delegate void ThreadActivationCallback();

    public delegate void JobFinishCallback(Object state);

    public delegate bool OnErrorCallback(Exception exp);

    public class ThreadManager
    {
        public event ThreadStart ThreadStarted;

        public event ThreadStart ThreadFinished;

        public event WaitCallback JobStarted;

        public event WaitCallback JobFinished;

        public event OnErrorCallback OnError;

        public event ThreadStart Idle;

        public uint MaxThreads;
        internal int activeThreads = 0;
        internal Stack workItems;
        internal int threadTimeout = 5000; // 5 seconds
        Timer timer;
        public String Name;

        public void CallThreadPool(String name, uint maxThreads)
        {
            Name = name;
            MaxThreads = maxThreads;
            workItems = Stack.Synchronized(new Stack((int) (10*maxThreads)));
            timer = new Timer(OnTimer, null, 0, 2000);
        }

        public bool QueueUserWorkItem(WaitCallback callback, Object state)
        {
            if (null == callback) return false;
            lock (this)
            {
                workItems.Push(new WorkItem(callback, state));
                if (activeThreads == 0) WorkThread.AddWorker(this);
                return true;
            }
        }

        public void OnTimer(Object state)
        {
            lock (this)
            {
                if (workItems.Count == 0) return;

                if (activeThreads < MaxThreads) WorkThread.AddWorker(this);
            }
        }

        internal void ThreadStartedEvent()
        {
            if (ThreadStarted != null) ThreadStarted();
        }

        internal void ThreadFinishedEvent()
        {
            if (ThreadFinished != null) ThreadFinished();
        }

        internal void JobStartedEvent(Object parameter)
        {
            if (JobStarted != null) JobStarted(parameter);
        }

        internal void JobFinishedEvent(Object parameter)
        {
            if (JobFinished != null) JobFinished(parameter);
        }

        internal void IdleEvent()
        {
            if (Idle != null) Idle();
        }

        internal bool OnErrorEvent(Exception exp)
        {
            return (null != OnError) && OnError(exp);
        }

        public void Stop()
        {
            lock (this)
            {
                timer.Change(Timeout.Infinite, Timeout.Infinite);
                workItems.Clear();
                MaxThreads = 0;
            }
        }
    }
}