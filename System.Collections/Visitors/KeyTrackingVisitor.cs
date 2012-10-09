namespace System.Collections.Visitors
{
    using Generic;

    /// <summary>
    /// A visitor that tracks (stores) keys from KeyValuePAirs in the order they were visited.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public sealed class KeyTrackingVisitor<TKey, TValue> : IVisitor<KeyValuePair<TKey, TValue>>
    {
        #region Globals

        readonly VisitableList<TKey> _tracks;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingVisitor&lt;T&gt;"/> class.
        /// </summary>
        public KeyTrackingVisitor()
        {
            _tracks = new VisitableList<TKey>();
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Gets the tracking list.
        /// </summary>
        /// <value>The tracking list.</value>
        public VisitableList<TKey> TrackingList
        {
            get
            {
                return _tracks;
            }
        }

        #endregion

        #region IVisitor<KeyValuePair<TKey,TValue>> Members


        /// <summary>
        /// Visits the specified object.
        /// </summary>
        /// <param name="obj">The object to visit.</param>
        public void Visit(KeyValuePair<TKey, TValue> obj)
        {
            _tracks.Add(obj.Key);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is done performing it's work..
        /// </summary>
        /// <value><c>true</c> if this instance is done; otherwise, <c>false</c>.</value>
        /// <returns><c>true</c> if this instance is done; otherwise, <c>false</c>.</returns>
        public bool HasCompleted
        {
            get
            {
                return false;
            }
        }

        #endregion
    }
}
