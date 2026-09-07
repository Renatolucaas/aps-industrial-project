namespace IndustrialAPS.Domain.Entities
{
    public class Machine
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public MachineStatus Status { get; set; }
        public decimal Capacity { get; set; }

        // Conexões do Grafo
        public ICollection<MachineConnection> Connections { get; set; } = new List<MachineConnection>();
        public ICollection<MachineOperation> Operations { get; set; } = new List<MachineOperation>();
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }

    // Enum (Status) precisa ficar fora da classe, mas dentro do mesmo arquivo
    public enum MachineStatus
    {
        Available,
        InMaintenance,
        Busy,
        Offline
    }
}