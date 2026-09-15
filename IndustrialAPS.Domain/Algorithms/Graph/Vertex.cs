namespace IndustrialAPS.Domain.Algorithms.Graph
{
    public class Vertex
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<Edge> Edges { get; set; } = new List<Edge>();

        public Vertex(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void AddEdge(Vertex destination, decimal weight)
        {
            Edges.Add(new Edge(this, destination, weight));
        }
    }
}