namespace IndustrialAPS.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Relacionamentos (ligações com outras tabelas)
        public ICollection<BOMItem> BOMItems { get; set; } = new List<BOMItem>();
        public ICollection<ProductionOrder> ProductionOrders { get; set; } = new List<ProductionOrder>();
        public ICollection<Operation> Operations { get; set; } = new List<Operation>();
    }
}