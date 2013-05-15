namespace System.Threading
{
    /// <summary>
    /// References the method that will handle the OnJob event.
    /// </summary>
    public delegate void OnJobHandler(ThreadHandlerEventArgs arg);

    /// <summary>
    /// References the method that will handle the OnAbort event.
    /// </summary>
    public delegate void OnAbortHandler(ThreadHandlerEventArgs arg);

    /// <summary>
    /// References the method that will handle the OnFinish event.
    /// </summary>
    public delegate void OnFinishHandler(ThreadHandlerEventArgs arg);

    /// <summary>
    /// References the method that will handle the OnTerminate event.
    /// NOTE: This is the ThreadHandler's cleanup-event. Exceptions here are not handled, and are not taken as thread exceptions.
    /// </summary>
    public delegate void OnTerminateHandler(ThreadHandlerEventArgs arg);

    /// <summary>
    /// References the method that will handle the OnException event.
    /// </summary>
    public delegate void OnExceptionHandler(ThreadHandlerExceptionArgs arg);

    /// <summary>
    /// Summary description for ThreadHandler.
    /// </summary>
    public class ThreadHandler
    {
        /// <summary>
        /// The thread, which will perform the calculations.
        /// </summary>
        readonly Thread _thread;

        /// <summary>
        /// Event is raised once, when the processing starts.
        /// </summary>
        public event OnJobHandler OnJob;

        /// <summary>
        /// On aborting this event's delegate function may do extra calculations.
        /// </summary>
        public event OnAbortHandler OnAbort;

        /// <summary>
        /// If thread is finished SUCCESFULLY, this event will be raised.
        /// </summary>
        public event OnFinishHandler OnFinish;

        /// <summary>
        /// If thread is aborted, just before leawing the ThreadHandler's working function this event will be triggered.
        /// </summary>
        public event OnTerminateHandler OnTerminate;

        /// <summary>
        /// When the event delegates exceptions are not handled, the thread will catch the exception!
        /// </summary>
        public event OnExceptionHandler OnException;

        /// <summary>
        /// Starting the thread.
        /// </summary>
        public void Start()
        {
            lock (this)
            {
                _thread.Start();
            }
        }


        /// <summary>
        /// Joinging the thread.
        /// </summary>
        public void Join()
        {
            lock (this)
            {
                _thread.Join();
            }
        }

        public bool IsAlive { get { lock (this) { return _thread.IsAlive; } } }

        /// <summary>
        /// Aborting the thread...
        /// </summary>
        public void Abort()
        {
            lock (this)
            {
                _thread.Abort();
            }
        }

        public void Sleep(int millisecondsTimeout)
        {
            lock (this)
            {
                Thread.Sleep(millisecondsTimeout);
            }
        }
        public void Sleep(TimeSpan timeout)
        {
            lock (this)
            {
                Thread.Sleep(timeout);
            }
        }

        /// <summary>
        /// The thread responsible for the working process...
        /// </summary>
        public Thread Thread
        {
            get { return _thread; }
        }

        public ThreadHandler()
        {
            _thread = new Thread(Work);
        }

        public ThreadHandler(Threads thread)
        {
            _thread = new Thread(Work);

            OnJob += thread.OnJob;
            OnFinish += thread.OnFinish;
            OnAbort += thread.OnAbort;
            OnTerminate += thread.OnTerminate;
            OnException += thread.OnException;
        }

        /// <summary>
        /// Threads main function.
        /// </summary>
        void Work()
        {
            // Create the event-arg-object which will be used in each event.
            ThreadHandlerEventArgs evArgs = null;

            lock (this)
            {
                evArgs = new ThreadHandlerEventArgs(this);
            }

            // NOTE:
            // Locking is not necessary if you assume, that all the events are assigned before starting the thread.
            // ALSO even if locking is implemented, there can a PROBLEM still arise if the last delegate function is removed after the lock on the thread is released and before the event is raised.
            try
            {
                try
                {
                    // MAIN FUNCTION
                    Monitor.Enter(this);
                    if (OnJob != null)
                    {
                        Monitor.Exit(this);
                        OnJob(evArgs);
                        Monitor.Enter(this);
                    }
                    Monitor.Exit(this);

                    // NOTE: It is also possible to abort the finish event !!!
                    Monitor.Enter(this);
                    if (OnFinish != null)
                    {
                        Monitor.Exit(this);
                        OnFinish(evArgs);
                        Monitor.Enter(this);
                    }
                    Monitor.Exit(this);
                }
                catch (ThreadAbortException)
                {
                    // WHEN ABORTING

                    // NOTE: If you call ResetAbort the ThreadAbortException will not be re-throw-ed after each catch block... Code after block finally will also be executed...
                    // System.Threading.Thread.ResetAbort();

                    Monitor.Enter(this);
                    if (OnAbort != null)
                    {
                        Monitor.Exit(this);
                        OnAbort(evArgs);
                        Monitor.Enter(this);
                    }
                    Monitor.Exit(this);
                }
            }
            catch (ThreadAbortException)
            {
                // Ignore this king of exception, it was already handled...but if not reseted, it is automatically re-thrown.
            }
            catch (Exception ex)
            {
                // HANDLING EXCEPTIONS
                Monitor.Enter(this);
                if (OnException != null)
                {
                    var exArgs = new ThreadHandlerExceptionArgs(this, ex);

                    Monitor.Exit(this);
                    OnException(exArgs);
                    Monitor.Enter(this);
                }
                Monitor.Exit(this);
            }
            finally
            {
                // CLEAN-UP - NOTE: Exceptions are not handled here, so be careful!
                Monitor.Enter(this);
                if (OnTerminate != null)
                {
                    Monitor.Exit(this);
                    OnTerminate(evArgs);
                    Monitor.Enter(this);
                }
                Monitor.Exit(this);
            }

            // This line executes only, when ThreadAbortException is Reset-ed...
            Console.WriteLine("HALO...");
        }
    }
}