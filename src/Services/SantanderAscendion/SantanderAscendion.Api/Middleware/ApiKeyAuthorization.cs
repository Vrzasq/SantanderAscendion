using Microsoft.Extensions.Primitives;

namespace SantanderAscendion.Api.Middleware;

public class ApiKeyAuthorization(RequestDelegate next)
{
    public const string ApiKeyHeader = "x-api-Key";

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger", StringComparison.OrdinalIgnoreCase))
        {
            await next(context);
            return;
        }

        var apiKeyHeader = context.Request.Headers[ApiKeyHeader];

        if (apiKeyHeader == StringValues.Empty)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("API Key is missing.");
            return;
        }

        string apiKey = apiKeyHeader.ToString();

        if (!apiKey.Contains("santander", StringComparison.InvariantCultureIgnoreCase))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("API Key is invalid.");
            return;
        }

        await next(context);
    }
}

public static class ApiKeyAuthorizationExtensions
{
    public static IApplicationBuilder UseApiKeyAuthorization(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ApiKeyAuthorization>();
    }
}