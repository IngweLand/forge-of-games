using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Shared.Extensions;
using Ingweland.Fog.Shared.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetTopHeroesQuery : IRequest<IReadOnlyCollection<string>>, ICacheableRequest
{
    public string? AgeId { get; init; }
    public int? FromLevel { get; init; } = 0;
    public required HeroInsightsMode Mode { get; init; }
    public int? ToLevel { get; init; } = int.MaxValue;
    public TimeSpan? Duration { get; }
    public DateTimeOffset? Expiration => DateTimeUtils.GetNextMidnightUtc();
}

public class GetTopHeroesQueryHandler(
    IFogDbContext context,
    IHeroInsightsService heroInsightsService,
    ILogger<GetTopHeroesQueryHandler> logger)
    : IRequestHandler<GetTopHeroesQuery, IReadOnlyCollection<string>>
{
    public async Task<IReadOnlyCollection<string>> Handle(GetTopHeroesQuery request,
        CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.ToDateOnly();
        var existingInsightsQuery = context.TopHeroInsights.AsNoTracking()
            .Where(x => x.Mode == request.Mode && x.AgeId == request.AgeId && x.CreatedAt == today);
        if (request.Mode == HeroInsightsMode.MostPopular && (request.FromLevel.HasValue || request.ToLevel.HasValue))
        {
            var fromLevel = request.FromLevel ?? 0;
            var toLevel = request.ToLevel ?? int.MaxValue;
            existingInsightsQuery = existingInsightsQuery.Where(x => x.FromLevel == fromLevel && x.ToLevel == toLevel);
        }
        else
        {
            existingInsightsQuery = existingInsightsQuery.Where(x => x.FromLevel == null && x.ToLevel == null);
        }

        var existingInsights = await existingInsightsQuery.FirstOrDefaultAsync(cancellationToken);

        if (existingInsights != null)
        {
            logger.LogDebug("Found existing insights");
            return existingInsights.Heroes.ToList();
        }

        logger.LogDebug("Existing insights not found, fetching new insights.");
        var insightsResult = await heroInsightsService.GetAsync(request.Mode, request.AgeId, request.FromLevel,
            request.ToLevel, cancellationToken);
        insightsResult.LogIfFailed<GetTopHeroesQueryHandler>();
        return insightsResult.IsSuccess ? insightsResult.Value : [];
    }
}
