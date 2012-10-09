namespace System.Collections.Algorithm
{
    using Generic;
    using Properties;

    /// <summary>
    ///   An implementation of Prim's Minimal Spanning Tree Algorithm.
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    public static class PrimsAlgorithm<T>
    {
        #region Public Methods
        /// <summary>
        ///   Finds the minimal spanning tree of the graph supplied.
        /// </summary>
        /// <param name="weightedGraph"> The weighted graph. </param>
        /// <param name="fromVertex"> The vertex to start from. </param>
        /// <returns> A graph representing the minimal spanning tree of the graph supplied. </returns>
        public static Graph<T> FindMinimalSpanningTree(Graph<T> weightedGraph, Vertex<T> fromVertex)
        {
            #region Parameter Checks
            if (weightedGraph == null)
                throw new ArgumentNullException("weightedGraph");
            if (fromVertex == null)
                throw new ArgumentNullException("fromVertex");
            if (!weightedGraph.ContainsVertex(fromVertex))
                throw new ArgumentException(Resources.VertexCouldNotBeFound);
            #endregion

            var heap =
                new Heap<Association<double, Vertex<T>>>(
                    HeapType.MinHeap,
                    new AssociationKeyComparer<double, Vertex<T>>());
            var vertexStatus = new Dictionary<Vertex<T>, VertexInfo<T>>();
            // Initialise the vertex distances to 
            using (var verticeEnumerator = weightedGraph.Vertices)
                while (verticeEnumerator.MoveNext())
                    vertexStatus.Add(verticeEnumerator.Current, new VertexInfo<T>(double.MaxValue, null, false));
            vertexStatus[fromVertex].Distance = 0;
            // Add the source vertex to the heap - we'll be branching out from it.		
            heap.Add(new Association<double, Vertex<T>>(0, fromVertex));
            while (heap.Count > 0)
            {
                var item = heap.RemoveRoot();
                var vertexInfo = vertexStatus[item.Value];
                var edges = item.Value.IncidentEdgeList;
                vertexStatus[item.Value].IsFinalised = true;
                // Enumerate through all the edges emanating from this node					
                foreach (var edge in edges) 
                {
                    var partnerVertex = edge.GetPartnerVertex(item.Value);
                    var newVertexInfo = vertexStatus[partnerVertex];
                    if (!newVertexInfo.IsFinalised)
                        if (edge.Weight < newVertexInfo.Distance)
                        {
                            newVertexInfo.EdgeFollowed = edge;
                            newVertexInfo.Distance = edge.Weight;
                            heap.Add(new Association<double, Vertex<T>>(edge.Weight, partnerVertex));
                        }
                }
            }
            // Now build the new graph
            var newGraph = new Graph<T>(weightedGraph.IsDirected);
            var enumerator = vertexStatus.GetEnumerator();
            // This dictionary is used for mapping between the old vertices and the new vertices put into the graph
            var vertexMap = new Dictionary<Vertex<T>, Vertex<T>>(vertexStatus.Count);
            var newVertices = new Vertex<T>[vertexStatus.Count];
            while (enumerator.MoveNext())
            {
                var newVertex = new Vertex<T>(
                    enumerator.Current.Key.Data,
                    enumerator.Current.Key.Weight
                    );
                vertexMap.Add(enumerator.Current.Key, newVertex);
                newGraph.AddVertex(newVertex);
            }
            enumerator = vertexStatus.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var info = enumerator.Current.Value;
                // Check if an edge has been included to this vertex
                if ((info.EdgeFollowed != null))
                    newGraph.AddEdge(
                        vertexMap[info.EdgeFollowed.GetPartnerVertex(enumerator.Current.Key)],
                        vertexMap[enumerator.Current.Key], info.Distance);
            }
            return newGraph;
        }
        #endregion
    }
}