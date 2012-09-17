namespace System
{
    using Collections.Generic;
    using Linq;

    /// <summary>
    ///   Extension methods for all kinds of exceptions.
    /// </summary>
    public static class ExceptionExtension
    {
        /// <summary>
        ///   Gets the original exception which is most inner exception.
        /// </summary>
        /// <param name = "exception">The exeption</param>
        /// <returns>The original exception</returns>
        [Obsolete("Use GetBaseException instead")]
        public static Exception GetOriginalException(this Exception exception)
        {
            if (default(Exception) == exception) throw new ArgumentNullException("exception");
            return default(Exception) == exception.InnerException ? exception : exception.InnerException.GetOriginalException();
        }

        ///<summary>
        /// Gets all the error messages
        ///</summary>
        ///<param name="exception">The exception</param>
        ///<returns>IEnumerable of message</returns>
        /// <note>
        /// The most inner exception message is first in the list, and the most outer exception message is last in the list
        /// </note>
        public static IEnumerable<String> Messages(this Exception exception)
        {
            return default(Exception) != exception
                ? new List<String>(exception.InnerException.Messages()) { exception.Message }
                : Enumerable.Empty<String>();
        }

        ///<summary>
        /// Gets all the errors
        ///</summary>
        ///<param name="exception">The exception</param>
        ///<returns>IEnumerable of message</returns>
        /// <remarks>
        /// 	Contributed by Michael T, http://about.me/MichaelTran
        /// </remarks>
        /// <note>
        /// The most inner exception is first in the list, and the most outer exception is last in the list
        /// </note>
        public static IEnumerable<Exception> Exceptions(this Exception exception)
        {
            return default(Exception) != exception
                ? new List<Exception>(exception.InnerException.Exceptions()) { exception }
                : Enumerable.Empty<Exception>();
        }

    }
}