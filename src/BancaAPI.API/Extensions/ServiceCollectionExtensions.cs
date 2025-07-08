using AspNetCoreRateLimit;
using BancaAPI.API.Filters;
using BancaAPI.Application.Interfaces.Common;
using BancaAPI.Application.Interfaces.Repositories;
using BancaAPI.Application.Interfaces.Services;
using BancaAPI.Application.Services;
using BancaAPI.Application.Validators;
using BancaAPI.Infrastructure.Common;
using BancaAPI.Infrastructure.Data;
using BancaAPI.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IClientRepository, ClienteRepository>();
        services.AddScoped<ICuentaRepository, CuentaRepository>();
        services.AddScoped<ITransaccionRepository, TransaccionRepository>();

        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<ICuentaService, CuentaService>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();


        services.AddValidatorsFromAssemblyContaining<CrearCuentaRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<CrearClienteRequestValidator>();

        services.AddScoped<AntiSpamFilter>();


        return services;
    }

    public static IServiceCollection AddRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
        services.AddInMemoryRateLimiting();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        return services;
    }

    public static IServiceCollection AddSwaggerDocs(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }
}
