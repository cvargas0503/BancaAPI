using BancaAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BancaAPI.Infrastructure.Data
{
    public class BancaDbContext : DbContext
    {
        public BancaDbContext(DbContextOptions<BancaDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<CuentaBancaria> Cuentas { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }
        public DbSet<TipoTransaccion> TiposTransaccion { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all IEntityTypeConfiguration<T> from this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BancaDbContext).Assembly);
        }
    }
}
