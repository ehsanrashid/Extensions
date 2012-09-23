using System;

namespace Threading
{
    /// <summary>
    /// When unhandled exception occurs in the event delegate functions, the exception will be cathed by the thread!
    /// The ThreadHandler will raise an event, the Exception along with the ThreadHandler will be passed via these EventArgs.
    /// The thread will be aborted, after the function returns from the OnException delegate function!
    /// </summary>
    public class ThreadHandlerExceptionArgs : EventArgs
    {
        /// <summary>
        /// The ThreadHandler, where the exception occured.
        /// </summary>
        public ThreadHandler ThreadHandler { get; private set; }

        /// <summary>
        /// The exception that was catched.
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Constructor for ThreadHandlerExceptionArgs.
        /// </summary>
        /// <param name="threadHandler">ThreadHandler object</param>
        /// <param name="exception">Exception object</param>
        /// <param name="stateAtException">Thread's state at the moment of the exception.</param>
        public ThreadHandlerExceptionArgs(ThreadHandler threadHandler, Exception exception)
        {
            ThreadHandler = threadHandler;
            Exception = exception;
        }
    }
}