namespace IndustrialAPS.Domain.Entities
{
    public class MachineConnection
    {
        public int Id { get; set; }
        public int FromMachineId { get; set; }
        public int ToMachineId { get; set; }
        public decimal Weight { get; set; } // Distância, tempo ou custo

        public Machine FromMachine { get; set; } = null!;
        public Machine ToMachine { get; set; } = null!;
    }
}