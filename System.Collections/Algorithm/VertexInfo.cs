namespace System.Collections.Algorithm
{
    using Generic;

    public class VertexInfo<T>
    {
        public double Distance { get; set; }

        public Edge<T> EdgeFollowed { get; set; }

        public bool IsFinalised { get; set; }

        #region Construction
        public VertexInfo(double d, Edge<T> edgeFollowed, bool isFinalised)
        {
            Distance = d;
            this.EdgeFollowed = edgeFollowed;
            this.IsFinalised = isFinalised;
        }
        #endregion

    }
}