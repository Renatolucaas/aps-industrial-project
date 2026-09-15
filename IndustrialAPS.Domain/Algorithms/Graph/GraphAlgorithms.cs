namespace IndustrialAPS.Domain.Algorithms.Graph
{
    public static class GraphAlgorithms
    {
        // BFS - Busca em Largura
        public static List<Vertex> BFS(Graph graph, Vertex start)
        {
            var visited = new HashSet<Vertex>();
            var queue = new Queue<Vertex>();
            var result = new List<Vertex>();

            queue.Enqueue(start);
            visited.Add(start);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                result.Add(current);

                foreach (var edge in current.Edges)
                {
                    if (!visited.Contains(edge.Destination))
                    {
                        visited.Add(edge.Destination);
                        queue.Enqueue(edge.Destination);
                    }
                }
            }

            return result;
        }

        // DFS - Busca em Profundidade
        public static List<Vertex> DFS(Graph graph, Vertex start)
        {
            var visited = new HashSet<Vertex>();
            var result = new List<Vertex>();
            DFSRecursive(start, visited, result);
            return result;
        }

        private static void DFSRecursive(Vertex current, HashSet<Vertex> visited, List<Vertex> result)
        {
            visited.Add(current);
            result.Add(current);

            foreach (var edge in current.Edges)
            {
                if (!visited.Contains(edge.Destination))
                {
                    DFSRecursive(edge.Destination, visited, result);
                }
            }
        }

        // Dijkstra - Menor Caminho
        public static (List<Vertex> Path, decimal TotalWeight) Dijkstra(Graph graph, Vertex start, Vertex end)
        {
            var distances = new Dictionary<Vertex, decimal>();
            var previous = new Dictionary<Vertex, Vertex>();
            var unvisited = new HashSet<Vertex>(graph.Vertices);

            foreach (var vertex in graph.Vertices)
            {
                distances[vertex] = decimal.MaxValue;
                previous[vertex] = null;
            }

            distances[start] = 0;

            while (unvisited.Count > 0)
            {
                // Encontra o vértice não visitado com a menor distância
                var current = unvisited.OrderBy(v => distances[v]).First();

                if (current == end)
                    break;

                unvisited.Remove(current);

                foreach (var edge in current.Edges)
                {
                    if (!unvisited.Contains(edge.Destination))
                        continue;

                    var newDistance = distances[current] + edge.Weight;

                    if (newDistance < distances[edge.Destination])
                    {
                        distances[edge.Destination] = newDistance;
                        previous[edge.Destination] = current;
                    }
                }
            }

            // Reconstrói o caminho
            var path = new List<Vertex>();
            var currentVertex = end;

            while (currentVertex != null)
            {
                path.Insert(0, currentVertex);
                currentVertex = previous[currentVertex];
            }

            // Se não houver caminho, retorna lista vazia
            if (path.Count == 0 || path[0] != start)
                return (new List<Vertex>(), -1);

            return (path, distances[end]);
        }
    }
}