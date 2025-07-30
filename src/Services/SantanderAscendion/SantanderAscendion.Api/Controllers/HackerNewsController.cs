using MediatR;
using Microsoft.AspNetCore.Mvc;
using SantanderAscendion.Application.Queries.GetBestStories;

namespace SantanderAscendion.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class HackerNewsController(IMediator mediator) : ControllerBase
{
    [HttpGet("best-stories")]
    [ProducesResponseType(typeof(GetBestStoriesQueryResult), StatusCodes.Status200OK)]
    public Task<GetBestStoriesQueryResult> GetBestStories(
        [FromQuery] GetBestStoriesQuery query,
        CancellationToken ct = default)
    {
        return mediator.Send(query, ct);
    }
}
