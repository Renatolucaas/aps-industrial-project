namespace IndustrialAPS.Domain.Entities
{
    public class Operation
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Duration { get; set; } // em minutos

        public Product Product { get; set; } = null!;
        public ICollection<MachineOperation> Machines { get; set; } = new List<MachineOperation>();
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}