namespace MedicalApi.Middleware;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public ApiKeyMiddleware(
        RequestDelegate next,
        IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger") || context.Request.Path.StartsWithSegments("/openapi"))
        {
            await _next(context);
            return;
        }
        if (HttpMethods.IsOptions(context.Request.Method))
        {
            context.Response.StatusCode = 200;
            return;
        }
        if (!context.Request.Headers.TryGetValue(
            "X-API-KEY",
            out var extractedKey))
        {
            context.Response.StatusCode = 401;
            return;
        }

        var apiKey = _configuration["ApiKey"];

        if (apiKey != extractedKey)
        {
            context.Response.StatusCode = 401;
            return;
        }

        await _next(context);
    }
}