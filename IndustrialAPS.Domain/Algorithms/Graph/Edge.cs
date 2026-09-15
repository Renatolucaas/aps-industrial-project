namespace IndustrialAPS.Domain.Algorithms.Graph
{
    public class Edge
    {
        public Vertex Source { get; set; }
        public Vertex Destination { get; set; }
        public decimal Weight { get; set; } // Distância, tempo ou custo

        public Edge(Vertex source, Vertex destination, decimal weight)
        {
            Source = source;
            Destination = destination;
            Weight = weight;
        }
    }
}