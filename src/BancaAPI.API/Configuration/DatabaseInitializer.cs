using BancaAPI.Infrastructure.Data;
using BancaAPI.Infrastructure.Data.Seed;
using Microsoft.EntityFrameworkCore;

namespace BancaAPI.API.Configuration
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BancaDbContext>();

            await dbContext.Database.MigrateAsync();
            await SeedData.InicializarAsync(dbContext);
        }
    }
}
