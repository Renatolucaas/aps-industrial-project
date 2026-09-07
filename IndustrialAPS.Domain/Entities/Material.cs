namespace IndustrialAPS.Domain.Entities
{
    public class Material
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public decimal Stock { get; set; }

        public ICollection<BOMItem> BOMItems { get; set; } = new List<BOMItem>();
    }
}