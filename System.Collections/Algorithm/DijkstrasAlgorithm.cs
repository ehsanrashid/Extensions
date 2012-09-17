namespace System.Collections.Algorithm
{
    using Generic;

    /// <summary>
    ///   An implementation of Djikstras single source shortest path algorithm.
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    public static class DijkstrasAlgorithm<T>
    {
        #region Public Methods

        /// <summary>
        ///   Finds the shortest paths to all other vertices from the specified source vertex.
        /// </summary>
        /// <param name="weightedGraph"> The weighted graph. </param>
        /// <param name="fromVertex"> The source vertex. </param>
        /// <returns> A graph representing the shortest paths from the source node to all other nodes in the graph. </returns>
        public static Graph<T> FindShortestPaths(Graph<T> weightedGraph, Vertex<T> fromVertex)
        {
            #region Parameter Checks

            if (weightedGraph == null) throw new ArgumentNullException("weightedGraph");
            if (fromVertex == null) throw new ArgumentNullException("fromVertex");
            if (!weightedGraph.ContainsVertex(fromVertex)) throw new ArgumentException(Properties.Resources.VertexCouldNotBeFound);

            #endregion

            var heap =
                new Heap<Association<double, Vertex<T>>>(
                    HeapType.MinHeap,
                    new AssociationKeyComparer<double, Vertex<T>>());
            var vertexStatus = new Dictionary<Vertex<T>, VertexInfo<T>>();
            // Initialise the vertex distances to 
            using (var verticeEnumerator = weightedGraph.Vertices) while (verticeEnumerator.MoveNext()) vertexStatus.Add(verticeEnumerator.Current, new VertexInfo<T>(double.MaxValue, null, false));
            vertexStatus[fromVertex].Distance = 0;
            // Add the source vertex to the heap - we'll be branching out from it.		
            heap.Add(new Association<double, Vertex<T>>(0, fromVertex));
            while (heap.Count > 0)
            {
                var item = heap.RemoveRoot();
                var vertexInfo = vertexStatus[item.Value];
                if (!vertexInfo.IsFinalised)
                {
                    var edges = item.Value.EmanatingEdgeList;
                    vertexStatus[item.Value].IsFinalised = true;
                    // Enumerate through all the edges emanating from this node					
                    for (var i = 0; i < edges.Count; i++)
                    {
                        var partnerVertex = edges[i].GetPartnerVertex(item.Value);
                        // Calculate the new distance to this distance
                        var distance = vertexInfo.Distance + edges[i].Weight;
                        var newVertexInfo = vertexStatus[partnerVertex];
                        // Found a better path, update the vertex status and add the 
                        // vertex to the heap for further analysis
                        if (distance < newVertexInfo.Distance)
                        {
                            newVertexInfo.EdgeFollowed = edges[i];
                            newVertexInfo.Distance = distance;
                            heap.Add(new Association<double, Vertex<T>>(distance, partnerVertex));
                        }
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
                    enumerator.Current.Value.Distance
                    );
                vertexMap.Add(enumerator.Current.Key, newVertex);
                newGraph.AddVertex(newVertex);
            }
            enumerator = vertexStatus.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var info = enumerator.Current.Value;
                // Check if an edge has been included to this vertex
                if ((info.EdgeFollowed != null) && (enumerator.Current.Key != fromVertex))
                {
                    newGraph.AddEdge(
                        vertexMap[info.EdgeFollowed.GetPartnerVertex(enumerator.Current.Key)],
                        vertexMap[enumerator.Current.Key],
                        info.EdgeFollowed.Weight);
                }
            }
            return newGraph;
        }

        #endregion
    }
}