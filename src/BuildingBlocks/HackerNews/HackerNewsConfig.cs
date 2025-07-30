namespace HackerNews;

public record HackerNewsConfig
{
    public const string SectionName = "HackerNews";

    public string BaseUrl { get; init; } = "https://hacker-news.firebaseio.com/v0/";

    public string BestStoriesEndpoint { get; init; } = "beststories.json";

    public string ItemEndpoint { get; init; } = "item";

    public int TimeoutSeconds { get; init; } = 30;

    public int CacheExpirationSeconds { get; init; } = 60 * 5;
}
