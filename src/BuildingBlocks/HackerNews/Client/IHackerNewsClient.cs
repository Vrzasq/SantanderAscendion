namespace HackerNews.Client;

public interface IHackerNewsClient
{
    /// <summary>
    /// Gets the top stories from Hacker News.
    /// </summary>
    /// <returns>A list of item ids representing the top stories.</returns>
    Task<IEnumerable<int>> GetTopStoriesAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets a specific item by its ID.
    /// </summary>
    /// <param name="id">The ID of the item to retrieve.</param>
    /// <returns>A HackerNewsItem representing the requested item.</returns>
    Task<HackerNewsItem> GetItemAsync(long id, CancellationToken ct = default);
}
