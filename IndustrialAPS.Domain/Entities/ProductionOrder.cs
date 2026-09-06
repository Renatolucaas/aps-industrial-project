namespace IndustrialAPS.Domain.Entities
{
    public class ProductionOrder
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public DateTime DueDate { get; set; }
        public OrderStatus Status { get; set; }

        public Product Product { get; set; } = null!;
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }

    public enum OrderStatus
    {
        Created,
        Planned,
        InProduction,
        Completed,
        Delayed
    }
}