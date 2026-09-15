namespace IndustrialAPS.Domain.Algorithms.Graph
{
    public class Graph
    {
        public List<Vertex> Vertices { get; set; } = new List<Vertex>();

        public Vertex AddVertex(int id, string name)
        {
            var vertex = new Vertex(id, name);
            Vertices.Add(vertex);
            return vertex;
        }

        public void AddEdge(Vertex source, Vertex destination, decimal weight)
        {
            source.AddEdge(destination, weight);
            // Se o grafo for não direcionado, descomente a linha abaixo:
            // destination.AddEdge(source, weight);
        }

        public Vertex? GetVertexById(int id)
        {
            return Vertices.FirstOrDefault(v => v.Id == id);
        }
    }
}