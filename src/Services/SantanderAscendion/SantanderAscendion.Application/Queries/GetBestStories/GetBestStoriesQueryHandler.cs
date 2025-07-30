using HackerNews.Client;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace SantanderAscendion.Application.Queries.GetBestStories;

public class GetBestStoriesQueryHandler(
    IHackerNewsClient hackerNewsClient,
    ILogger<GetBestStoriesQueryHandler> logger
    )
    : IRequestHandler<GetBestStoriesQuery, GetBestStoriesQueryResult>
{
    public async Task<GetBestStoriesQueryResult> Handle(GetBestStoriesQuery request, CancellationToken ct)
    {
        if (request.Count <= 0)
            return new GetBestStoriesQueryResult([]);

        var topStories = (await hackerNewsClient.GetTopStoriesAsync(ct)).ToArray();
        var detailedStories = await GetDetailedStoriesAsync(topStories, ct);

        IEnumerable<StoryItem> bestStories = detailedStories
            .Select(x => x.ToStoryItem())
            .OrderByDescending(s => s.Score);

        if (request.Count.HasValue)
            bestStories = bestStories.Take(request.Count.Value);

        return new GetBestStoriesQueryResult(bestStories);
    }

    private async Task<IEnumerable<HackerNewsItem>> GetDetailedStoriesAsync(
        int[] topStories,
        CancellationToken ct)
    {
        var detailedStories = new ConcurrentBag<HackerNewsItem>();

        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = topStories.Length / 20,
            CancellationToken = ct
        };

        await Parallel.ForEachAsync(topStories, parallelOptions, async (storyId, ct) =>
        {
            try
            {
                var item = await hackerNewsClient.GetItemAsync(storyId, ct);
                detailedStories.Add(item);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to fetch story with ID {StoryId}", storyId);
            }
        });

        return detailedStories;
    }
}
