using IndustrialAPS.Domain.Algorithms.Graph;
using IndustrialAPS.Domain.Entities;

namespace IndustrialAPS.Domain.Services
{
    public class GraphService
    {
        // Monta o grafo a partir da lista de máquinas e conexões
        public Graph BuildGraph(List<Machine> machines, List<MachineConnection> connections)
        {
            var graph = new Graph();

            // 1. Adiciona todos os vértices (máquinas)
            var vertexMap = new Dictionary<int, Vertex>();
            foreach (var machine in machines)
            {
                var vertex = graph.AddVertex(machine.Id, machine.Name);
                vertexMap[machine.Id] = vertex;
            }

            // 2. Adiciona as arestas (conexões)
            foreach (var connection in connections)
            {
                if (vertexMap.ContainsKey(connection.FromMachineId) && vertexMap.ContainsKey(connection.ToMachineId))
                {
                    var source = vertexMap[connection.FromMachineId];
                    var destination = vertexMap[connection.ToMachineId];
                    graph.AddEdge(source, destination, connection.Weight);
                }
            }

            return graph;
        }

        // Retorna o menor caminho entre duas máquinas
        public (List<Vertex> Path, decimal TotalWeight) FindShortestPath(Graph graph, int fromMachineId, int toMachineId)
        {
            var start = graph.GetVertexById(fromMachineId);
            var end = graph.GetVertexById(toMachineId);

            if (start == null || end == null)
                return (new List<Vertex>(), -1);

            return GraphAlgorithms.Dijkstra(graph, start, end);
        }

        // Retorna todas as máquinas alcançáveis a partir de uma máquina (BFS)
        public List<Vertex> GetReachableMachines(Graph graph, int startMachineId)
        {
            var start = graph.GetVertexById(startMachineId);
            if (start == null)
                return new List<Vertex>();

            return GraphAlgorithms.BFS(graph, start);
        }

        // Retorna o percurso em profundidade a partir de uma máquina (DFS)
        public List<Vertex> GetDepthFirstTraversal(Graph graph, int startMachineId)
        {
            var start = graph.GetVertexById(startMachineId);
            if (start == null)
                return new List<Vertex>();

            return GraphAlgorithms.DFS(graph, start);
        }
    }
}