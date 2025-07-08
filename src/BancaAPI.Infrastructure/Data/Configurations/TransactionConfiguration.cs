using BancaAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BancaAPI.Infrastructure.Data.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaccion>
    {
        public void Configure(EntityTypeBuilder<Transaccion> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Monto)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(t => t.SaldoDespues)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(t => t.Fecha)
                   .IsRequired();

            // Relación con CuentaBancaria (many-to-one)
            builder.HasOne(t => t.Cuenta)
                   .WithMany(c => c.Transacciones)
                   .HasForeignKey(t => t.CuentaId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Relación con TipoTransaccion (many-to-one)
            builder.HasOne(t => t.TipoTransaccion)
                   .WithMany(tt => tt.Transacciones)
                   .HasForeignKey(t => t.TipoTransaccionId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
