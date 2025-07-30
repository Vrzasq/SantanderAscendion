using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace SeedWork.MediatR.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger
    )
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        var sw = Stopwatch.StartNew();

        logger.LogInformation("Handling {RequestName} {@Request}",
            typeof(TRequest).Name,
            request);

        try
        {
            var response = await next(ct).ConfigureAwait(false);
            sw.Stop();

            logger.LogInformation("Handled: {RequestName} - Response {ResponseName}: {@Response}, Elapsed: {Elapsed}ms",
                typeof(TRequest).Name,
                typeof(TResponse).Name,
                response,
                sw.ElapsedMilliseconds);

            return response;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while handling {RequestName}", typeof(TRequest).Name);
            throw;
        }
    }
}
