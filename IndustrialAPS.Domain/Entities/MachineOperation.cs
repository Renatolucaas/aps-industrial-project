namespace IndustrialAPS.Domain.Entities
{
    public class MachineOperation
    {
        public int MachineId { get; set; }
        public int OperationId { get; set; }

        public Machine Machine { get; set; } = null!;
        public Operation Operation { get; set; } = null!;
    }
}