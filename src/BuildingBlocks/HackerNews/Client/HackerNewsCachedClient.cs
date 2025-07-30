using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace HackerNews.Client;

internal class HackerNewsCachedClient(
    HackerNewsClient hackerNewsClient,
    IOptions<HackerNewsConfig> options,
    IDistributedCache cache,
    [FromKeyedServices(HackerNewsSerializationOptions.Name)] JsonSerializerOptions jsonSerializerOptions,
    ILogger<HackerNewsCachedClient> logger
    )
    : IHackerNewsClient
{
    private const string TopStoriesCacheKey = $"{nameof(HackerNewsClient)}:{nameof(GetTopStoriesAsync)}";
    private const string ItemCacheKey = $"{nameof(HackerNewsClient)}:{nameof(GetItemAsync)}";

    private readonly DistributedCacheEntryOptions _cacheOptions = new()
    {
        SlidingExpiration = TimeSpan.FromSeconds(options.Value.CacheExpirationSeconds)
    };

    public async Task<IEnumerable<int>> GetTopStoriesAsync(CancellationToken ct = default)
    {
        var cacheData = await cache.GetAsync(TopStoriesCacheKey, ct);

        if (cacheData is not null)
        {
            var result = JsonSerializer.Deserialize<IEnumerable<int>>(cacheData, jsonSerializerOptions);

            if (result is not null)
                return result;
        }

        var response = await hackerNewsClient.GetTopStoriesAsync(ct);
        await AddToCache(TopStoriesCacheKey, response, ct);

        return response;
    }

    public async Task<HackerNewsItem> GetItemAsync(long id, CancellationToken ct = default)
    {
        var cacheData = await cache.GetAsync($"{ItemCacheKey}:{id}", ct);

        if (cacheData is not null)
        {
            var result = JsonSerializer.Deserialize<HackerNewsItem>(cacheData, jsonSerializerOptions);

            if (result is not null)
                return result;
        }

        var response = await hackerNewsClient.GetItemAsync(id, ct);
        await AddToCache($"{ItemCacheKey}:{id}", response, ct);

        return response;
    }

    private Task AddToCache<T>(string key, T value, CancellationToken ct)
    {
        try
        {
            var data = JsonSerializer.SerializeToUtf8Bytes(value);
            return cache.SetAsync(key, data, _cacheOptions,  ct);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to cache data for key {Key}", key);
        }

        return Task.CompletedTask;
    }
}
