using IndustrialAPS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IndustrialAPS.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets para cada entidade (tabelas)
        public DbSet<Product> Products { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<BOMItem> BOMItems { get; set; }
        public DbSet<IndustrialAPS.Domain.Entities.Machine> Machines { get; set; }
        public DbSet<MachineConnection> MachineConnections { get; set; }
        public DbSet<IndustrialAPS.Domain.Entities.Operation> Operations { get; set; }
        public DbSet<MachineOperation> MachineOperations { get; set; }
        public DbSet<ProductionOrder> ProductionOrders { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da chave composta para MachineOperation (N:N)
            modelBuilder.Entity<MachineOperation>()
                .HasKey(mo => new { mo.MachineId, mo.OperationId });

            // --- Configurações de Relacionamentos para evitar múltiplos caminhos em cascata ---

            // 1. Configuração do relacionamento Machine -> MachineConnection
            modelBuilder.Entity<MachineConnection>()
                .HasOne(mc => mc.FromMachine)
                .WithMany(m => m.Connections)
                .HasForeignKey(mc => mc.FromMachineId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MachineConnection>()
                .HasOne(mc => mc.ToMachine)
                .WithMany() // Não há navegação inversa para "ToMachine"
                .HasForeignKey(mc => mc.ToMachineId)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. 🔽 Configuração para evitar múltiplos caminhos em cascata na tabela Schedules
            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Order)
                .WithMany(o => o.Schedules)
                .HasForeignKey(s => s.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Operation)
                .WithMany(o => o.Schedules)
                .HasForeignKey(s => s.OperationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Machine)
                .WithMany(m => m.Schedules)
                .HasForeignKey(s => s.MachineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}