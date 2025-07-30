using MediatR;
using SeedWork.MediatR.Caching;

namespace SantanderAscendion.Application.Queries.GetBestStories;

public record GetBestStoriesQuery(
    int? Count = null
    )
    : IRequest<GetBestStoriesQueryResult>, ICacheable
{
    string ICacheable.Key { get; } = $"{nameof(GetBestStoriesQuery)}:{Count}";
    TimeSpan ICacheable.ExpirationTime { get; } = TimeSpan.FromMinutes(2);
}
