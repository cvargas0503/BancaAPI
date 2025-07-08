using BancaAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BancaAPI.Infrastructure.Data.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<CuentaBancaria>
    {
        public void Configure(EntityTypeBuilder<CuentaBancaria> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.NumeroCuenta).IsRequired().HasMaxLength(20);
            builder.Property(c => c.Saldo).HasColumnType("decimal(18,2)");

            builder.HasIndex(c => c.NumeroCuenta).IsUnique();

            builder.HasMany(c => c.Transacciones)
                   .WithOne(t => t.Cuenta)
                   .HasForeignKey(t => t.CuentaId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
