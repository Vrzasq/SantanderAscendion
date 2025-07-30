using HackerNews.Client;

namespace SantanderAscendion.Application.Queries.GetBestStories;

public record class StoryItem(
    long Id,
    bool? Deleted,
    string? Type,
    string? By,
    long? Time,
    string? Text,
    bool? Dead,
    int? Parent,
    int? Poll,
    IEnumerable<int>? Kids,
    string? Url,
    int? Score,
    string? Title,
    IEnumerable<int>? Parts,
    int? Descendants
    );


internal static class HackerNewsItemExtensions
{
    public static StoryItem ToStoryItem(this HackerNewsItem item)
    {
        return new StoryItem(
            Id: item.Id,
            Deleted: item.Deleted,
            Type: item.Type,
            By: item.By,
            Time: item.Time,
            Text: item.Text,
            Dead: item.Dead,
            Parent: item.Parent,
            Poll: item.Poll,
            Kids: item.Kids,
            Url: item.Url,
            Score: item.Score,
            Title: item.Title,
            Parts: item.Parts,
            Descendants: item.Descendants
        );
    }
}
