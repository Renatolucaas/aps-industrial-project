using Azure;
using IndustrialAPS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

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
        public DbSet<Machine> Machines { get; set; }
        public DbSet<MachineConnection> MachineConnections { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<MachineOperation> MachineOperations { get; set; }
        public DbSet<ProductionOrder> ProductionOrders { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da chave composta para MachineOperation (N:N)
            modelBuilder.Entity<MachineOperation>()
                .HasKey(mo => new { mo.MachineId, mo.OperationId });
        }
    }
}