using AspNetCoreRateLimit;
using BancaAPI.API.Configuration;
using BancaAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureSerilog();

builder.Services.AddDbContext<BancaDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddApplicationServices()
    .AddRateLimiting(builder.Configuration)
    .AddSwaggerDocs();

builder.Services.AddControllers();

var app = builder.Build();

await app.Services.InitializeAsync();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCustomExceptionHandler();
app.UseSecurityHeaders();
app.UseIpRateLimiting();
app.UseNotFoundLogging();

app.MapControllers();
app.Run();