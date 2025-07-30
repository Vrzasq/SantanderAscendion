namespace SantanderAscendion.Application.Queries.GetBestStories;

public record GetBestStoriesQueryResult(
    IEnumerable<StoryItem> TopStories
    );