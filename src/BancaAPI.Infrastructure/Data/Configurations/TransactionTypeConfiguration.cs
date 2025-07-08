using BancaAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BancaAPI.Infrastructure.Data.Configurations
{
    public class TransactionTypeConfiguration : IEntityTypeConfiguration<TipoTransaccion>
    {
        public void Configure(EntityTypeBuilder<TipoTransaccion> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Descripcion)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.HasIndex(t => t.Descripcion)
                   .IsUnique();

            builder.HasMany(t => t.Transacciones)
                   .WithOne(tr => tr.TipoTransaccion)
                   .HasForeignKey(tr => tr.TipoTransaccionId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
