namespace System.Threading
{
    public abstract class Threads
    {
        protected Threads() { }

        /// <summary>
        /// Represents the method that will handle the OnJob event.
        /// </summary>
        public abstract void OnJob(ThreadHandlerEventArgs arg);

        /// <summary>
        /// Represents the method that will handle the OnAbort event.
        /// </summary>
        public virtual void OnAbort(ThreadHandlerEventArgs arg) { }

        /// <summary>
        /// Represents the method that will handle the OnTerminate event.
        /// </summary>
        public virtual void OnTerminate(ThreadHandlerEventArgs arg) { }

        /// <summary>
        /// Represents the method that will handle the OnFinish event.
        /// </summary>
        public virtual void OnFinish(ThreadHandlerEventArgs arg) { }

        /// <summary>
        /// Represents the method that will handle the OnException event.
        /// </summary>
        public virtual void OnException(ThreadHandlerExceptionArgs arg)
        {
            throw arg.Exception;
        }
    }
}