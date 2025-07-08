// src/BancaAPI.Infrastructure/Persistence/SeedData.cs

using BancaAPI.Domain.Entities;
using BancaAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BancaAPI.Infrastructure.Data.Seed
{
    public static class SeedData
    {
        public static async Task InicializarAsync(BancaDbContext context)
        {
            if (!context.TiposTransaccion.Any())
            {
                context.TiposTransaccion.AddRange(new[]
                {
                    new TipoTransaccion { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Descripcion = "Depósito" },
                    new TipoTransaccion { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Descripcion = "Retiro" }
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
