namespace IndustrialAPS.API.DTOs
{
    public class OperationDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Duration { get; set; }
    }
}