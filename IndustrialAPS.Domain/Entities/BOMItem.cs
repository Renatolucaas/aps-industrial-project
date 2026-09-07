namespace IndustrialAPS.Domain.Entities
{
    public class BOMItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ComponentId { get; set; } // Pode ser um Material ou Subproduto
        public decimal Quantity { get; set; }

        // Navegação
        public Product Product { get; set; } = null!;
        public Material Material { get; set; } = null!;
    }
}