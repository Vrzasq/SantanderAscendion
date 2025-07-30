using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SeedWork.MediatR.Caching;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SeedWork.MediatR.Behaviors;

public class CachingBehavior<TRequest, TResponse>(
    IDistributedCache distributedCache,
    ILogger<CachingBehavior<TRequest, TResponse>> logger
    )
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull, ICacheable
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        ReferenceHandler = ReferenceHandler.IgnoreCycles
    };

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        if (string.IsNullOrEmpty(request.Key))
            return await next(ct).ConfigureAwait(false);

        try
        {
            var cacheData = await distributedCache.GetAsync(request.Key, ct).ConfigureAwait(false);

            if (cacheData is not null)
            {
                var result = JsonSerializer.Deserialize<TResponse>(cacheData, _jsonSerializerOptions);

                if (result is not null)
                    return result;
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to retrieve cache for key {CacheKey}", request.Key);
        }

        var response = await next(ct).ConfigureAwait(false);

        try
        {
            if (response is not null)
            {
                var cacheData = JsonSerializer.SerializeToUtf8Bytes(response, _jsonSerializerOptions);
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = request.ExpirationTime
                };

                await distributedCache.SetAsync(request.Key, cacheData, cacheOptions, ct).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to cache response for key {CacheKey}", request.Key);
        }

        return response;
    }
}
