using BancaAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BancaAPI.Infrastructure.Data.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Sexo).IsRequired().HasMaxLength(20);
            builder.Property(c => c.Ingresos).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(c => c.FechaNacimiento)
                   .IsRequired()
                   .HasColumnType("date");

            builder.HasMany(c => c.Cuentas)
                   .WithOne(c => c.Cliente)
                   .HasForeignKey(c => c.ClienteId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
