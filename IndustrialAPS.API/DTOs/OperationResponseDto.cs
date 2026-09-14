namespace IndustrialAPS.API.DTOs
{
    public class OperationResponseDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Duration { get; set; }
    }
}