using System.Collections.Generic;

namespace System.Collections.Visitors
{
    /// <summary>
    ///   A visitor that tracks (stores) objects in the order they were visited.
    ///   Handy for demonstrating and testing different ordered visits impementations on
    ///   datastructures.
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    public sealed class TrackingVisitor<T> : IVisitor<T>
    {
        private readonly VisitableList<T> tracks;

        #region Construction
        /// <summary>
        ///   Initializes a new instance of the <see cref="TrackingVisitor&lt;T&gt;" /> class.
        /// </summary>
        public TrackingVisitor()
        {
            tracks = new VisitableList<T>();
        }
        #endregion

        #region IVisitor<T> Members
        /// <summary>
        ///   Visits the specified object.
        /// </summary>
        /// <param name="obj"> The object. </param>
        public void Visit(T obj)
        {
            tracks.Add(obj);
        }

        /// <summary>
        ///   Gets a value indicating whether this instance is done performing it's work..
        /// </summary>
        /// <returns> <c>true</c> if this instance is done; otherwise, <c>false</c> . </returns>
        /// <value> <c>true</c> if this instance is done; otherwise, <c>false</c> . </value>
        public bool HasCompleted
        {
            get { return false; }
        }
        #endregion

        #region Public Members
        /// <summary>
        ///   Gets the tracking list.
        /// </summary>
        /// <value> The tracking list. </value>
        public VisitableList<T> TrackingList
        {
            get { return tracks; }
        }
        #endregion
    }
}