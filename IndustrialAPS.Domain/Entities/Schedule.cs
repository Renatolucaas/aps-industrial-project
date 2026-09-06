namespace IndustrialAPS.Domain.Entities
{
    public class Schedule
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int OperationId { get; set; }
        public int MachineId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public ProductionOrder Order { get; set; } = null!;
        public Operation Operation { get; set; } = null!;
        public Machine Machine { get; set; } = null!;
    }
}