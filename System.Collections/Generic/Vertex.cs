using System.Diagnostics;
using System.Linq;

namespace System.Collections.Generic
{
    /// <summary>
    /// A class representing a vertex in a graph.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Vertex<T>
    {
        readonly List<Edge<T>> _incidentEdges;
        readonly List<Edge<T>> _emanatingEdges;

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="Vertex&lt;T&gt;"/> class.
        /// The weight is 0 by default.
        /// </summary>
        /// <param name="data">The data contained in the vertex.</param>
        public Vertex(T data)
        {
            Data = data;
            _incidentEdges = new List<Edge<T>>();
            _emanatingEdges = new List<Edge<T>>();
            Weight = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vertex&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="data">The data contained in the vertex</param>
        /// <param name="weight">The weight of the vertex.</param>
        public Vertex(T data, double weight)
        {
            Data = data;
            _incidentEdges = new List<Edge<T>>();
            _emanatingEdges = new List<Edge<T>>();
            Weight = weight;
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        /// <value>The weight.</value>
        public double Weight { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data contained in the vertex.</value>
        public T Data { get; set; }

        /// <summary>
        /// Gets the count of the incident edges on this vertex.
        /// </summary>
        /// <value>The count of the incident edges on this vertex.</value>
        public int IncidentEdgesCount
        {
            get { return _incidentEdges.Count; }
        }

        /// <summary>
        /// Gets the degree of this vertex (the number of emanating edges).
        /// </summary>
        /// <value>The degree.</value>
        public int Degree
        {
            get { return _emanatingEdges.Count; }
        }

        /// <summary>
        /// Gets the edges incident on this vertex.
        /// </summary>
        /// <value>The edges incident on this vertex.</value>
        public IEnumerator<Edge<T>> IncidentEdges
        {
            get { return _incidentEdges.GetEnumerator(); }
        }

        /// <summary>
        /// Gets the emanating edges on this vertex.
        /// </summary>
        /// <value>The emanating edges on this vertex.</value>
        public IEnumerator<Edge<T>> EmanatingEdges
        {
            get { return _emanatingEdges.GetEnumerator(); }
        }

        /// <summary>
        /// Determines whether this vertex has an emanating edge to the specified vertex.
        /// </summary>
        /// <param name="toVertex">The vertex to test connectivity to.</param>
        /// <returns>
        /// 	<c>true</c> if this vertex has an emanating edge to the specified vertex; otherwise, <c>false</c>.
        /// </returns>
        public bool HasEmanatingEdgeTo(Vertex<T> toVertex)
        {
            foreach (var edge in _emanatingEdges)
            {
                if (edge.IsDirected)
                {
                    if (edge.ToVertex == toVertex) return true;
                }
                else
                {
                    if ((edge.ToVertex == toVertex) || ((edge.FromVertex == toVertex))) return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether [has incident edge with] [the specified from vertex].
        /// </summary>
        /// <param name="fromVertex">From vertex.</param>
        /// <returns>
        /// 	<c>true</c> if [has incident edge with] [the specified from vertex]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasIncidentEdgeWith(Vertex<T> fromVertex)
        {
            //for (var i = 0; i < incidentEdges.Count; i++) 
            //    if ((incidentEdges[i].FromVertex == fromVertex) || (incidentEdges[i].ToVertex == fromVertex)) 
            //        return true;
            //return false;

            return _incidentEdges.Any(edge => (edge.FromVertex == fromVertex) || (edge.ToVertex == fromVertex));
        }

        /// <summary>
        /// Gets the emanating edge to the specified vertex.
        /// </summary>
        /// <param name="toVertex">To to vertex.</param>
        /// <returns>The emanating edge to the vertex specified if found, otherwise null.</returns>
        public Edge<T> GetEmanatingEdgeTo(Vertex<T> toVertex)
        {
            foreach (var edge in _emanatingEdges)
            {
                if (edge.IsDirected)
                {
                    if (edge.ToVertex == toVertex) return edge;
                }
                else
                {
                    if ((edge.FromVertex == toVertex) || (edge.ToVertex == toVertex)) return edge;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the incident edge to the specified vertex.
        /// </summary>
        /// <param name="toVertex">The to vertex.</param>
        /// <returns>The incident edge to the vertex specified if found, otherwise null.</returns>
        public Edge<T> GetIncidentEdgeWith(Vertex<T> toVertex)
        {
            //for (var i = 0; i < incidentEdges.Count; i++)
            //    if ((incidentEdges[i].ToVertex == toVertex) || (incidentEdges[i].FromVertex == toVertex))
            //        return incidentEdges[i];
            //return null;

            return _incidentEdges.FirstOrDefault(edge => (edge.ToVertex == toVertex) || (edge.FromVertex == toVertex));
        }

        #endregion

        #region Internal Members

        internal List<Edge<T>> IncidentEdgeList
        {
            get
            {
                return _incidentEdges;
            }
        }

        internal List<Edge<T>> EmanatingEdgeList
        {
            get
            {
                return _emanatingEdges;
            }
        }

        /// <summary>
        /// Removes the edge specified from the vertex.
        /// </summary>
        /// <param name="edge">The edge to be removed.</param>
        internal void RemoveEdge(Edge<T> edge)
        {
            #region Asserts

            Debug.Assert(edge != null);

            #endregion

            RemoveEdgeFromVertex(edge);
        }

        internal void AddEdge(Edge<T> edge)
        {
            #region Asserts

            Debug.Assert(edge != null);

            #endregion

            if (edge.IsDirected)
            {
                if (edge.FromVertex == this)
                {
                    _emanatingEdges.Add(edge);
                }
            }
            else
            {
                _emanatingEdges.Add(edge);
            }

            _incidentEdges.Add(edge);
        }

        #endregion

        #region Private Members

        private void RemoveEdgeFromVertex(Edge<T> edge)
        {
            #region Asserts

            Debug.Assert(_incidentEdges.Remove(edge), "Edge not found on vertex in RemoveEdgeFromVertex.");

            #endregion

            _incidentEdges.Remove(edge);

            if (edge.IsDirected)
            {
                if (edge.FromVertex == this)
                {
                    _emanatingEdges.Remove(edge);
                }
            }
            else
            {
                _emanatingEdges.Remove(edge);
            }
        }

        #endregion
    }
}
