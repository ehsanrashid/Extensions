using System;

namespace Threading
{
    /// <summary>
    /// When event is raised from ThreadHandler the ThreadHandler itself will be passed to the delegate function.
    /// So controlling the thread, that invoked the event is possible.
    /// </summary>
    public class ThreadHandlerEventArgs : EventArgs
    {
        /// <summary>
        /// The ThreadHandler from where this event was generated.
        /// </summary>
        public ThreadHandler ThreadHandler { get; private set; }

        /// <summary>
        /// Constructor for ThreadHandlerEventArgs.
        /// </summary>
        /// <param name="threadHandler">ThreadHandler object</param>
        public ThreadHandlerEventArgs(ThreadHandler threadHandler)
        {
            ThreadHandler = threadHandler;
        }
    }
}