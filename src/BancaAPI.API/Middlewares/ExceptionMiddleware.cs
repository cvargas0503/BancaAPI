using BancaAPI.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                InvalidTransactionAmountException => (int)HttpStatusCode.BadRequest,
                DuplicateAccountException => (int)HttpStatusCode.Conflict,
                InsufficientFundsException => (int)HttpStatusCode.BadRequest,
                CustomerWithoutAccountException => (int)HttpStatusCode.BadRequest,
                BusinessRuleException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var message = GetUserFriendlyMessage(ex);

            var response = new
            {
                error = message,
                statusCode = context.Response.StatusCode
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }

    private string GetUserFriendlyMessage(Exception ex) => ex switch
    {
        NotFoundException nf => $"No se encontró el recurso '{nf.Message}'",
        InvalidTransactionAmountException it => "El monto de la transacción debe ser mayor a cero.",
        DuplicateAccountException da => "Ya existe una cuenta con ese número.",
        InsufficientFundsException ife => "Fondos insuficientes para completar el retiro.",
        CustomerWithoutAccountException ca => "El cliente no tiene cuentas registradas.",
        BusinessRuleException br => br.Message,
        _ => "Ha ocurrido un error inesperado. Por favor, contacte al administrador."
    };
}
