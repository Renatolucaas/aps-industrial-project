namespace IndustrialAPS.API.DTOs
{
    public class BOMItemDto
    {
        public int ProductId { get; set; }
        public int ComponentId { get; set; }
        public decimal Quantity { get; set; }
    }
}