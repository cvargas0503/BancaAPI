using Serilog;

namespace BancaAPI.API.Configuration
{
    public static class LoggingConfiguration
    {
        public static void ConfigureSerilog(this ConfigureHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((context, services, configuration) =>
            {
                configuration
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.Async(a => a.File("Logs/api-log-.txt", rollingInterval: RollingInterval.Day));
            });
        }
    }
}