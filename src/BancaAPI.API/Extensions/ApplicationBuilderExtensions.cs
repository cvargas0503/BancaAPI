using Serilog;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionMiddleware>();
    }

    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
    {
        return app.UseMiddleware<SecurityHeadersMiddleware>();
    }

    public static IApplicationBuilder UseNotFoundLogging(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            await next();
            if (context.Response.StatusCode == 404)
            {
                Log.Warning("404 Not Found: {Path}", context.Request.Path);
            }
        });
    }
}
