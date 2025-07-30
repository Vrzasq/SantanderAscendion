using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace HackerNews.Client;

internal class HackerNewsClient(
    HttpClient httpClient,
    IOptions<HackerNewsConfig> options,
    [FromKeyedServices(HackerNewsSerializationOptions.Name)] JsonSerializerOptions jsonSerializerOptions
    )
    : IHackerNewsClient
{
    private const string DeserializeErrorMessage = "Failed to deserialize response {0} to type {1}";
    private readonly HackerNewsConfig _config = options.Value;

    public Task<IEnumerable<int>> GetTopStoriesAsync(CancellationToken ct = default) =>
        GetRequest<IEnumerable<int>>(_config.BestStoriesEndpoint, ct);

    public Task<HackerNewsItem> GetItemAsync(long id, CancellationToken ct = default) =>
        GetRequest<HackerNewsItem>($"{_config.ItemEndpoint}/{id}.json", ct);

    private async Task<T> GetRequest<T>(string endpoint, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(endpoint);

        var responseMessage = (await httpClient.GetAsync(endpoint, ct))
            .EnsureSuccessStatusCode();

        string json = await responseMessage.Content.ReadAsStringAsync(ct);

        return JsonSerializer.Deserialize<T>(json, jsonSerializerOptions)
            ?? throw new InvalidOperationException(string.Format(DeserializeErrorMessage, json, typeof(T).Name));
    }
}
