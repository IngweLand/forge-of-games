using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Shared.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetTopHeroesQuery : IRequest<IReadOnlyCollection<string>>, ICacheableRequest
{
    public string? AgeId { get; init; }
    public int? FromLevel { get; init; } = 0;
    public required HeroInsightsMode Mode { get; init; }
    public int? ToLevel { get; init; } = int.MaxValue;
    public string CacheKey => $"TopHeroes-{Mode}-{AgeId}-{FromLevel}-{ToLevel}";
    public TimeSpan? Duration => TimeSpan.FromHours(3);
    public DateTimeOffset? Expiration { get; }
}

public class GetTopHeroesQueryHandler(IFogDbContext context)
    : IRequestHandler<GetTopHeroesQuery, IReadOnlyCollection<string>>
{
    public async Task<IReadOnlyCollection<string>> Handle(GetTopHeroesQuery request,
        CancellationToken cancellationToken)
    {
        var minCollectionDate = DateTime.UtcNow.ToDateOnly().AddDays(-FogConstants.TOP_HEROES_LOOKBACK_DAYS);
        var initQuery = context.ProfileSquads.AsNoTracking()
            .Where(x => x.CollectedAt > minCollectionDate);

        if (request.Mode == HeroInsightsMode.MostPopular && (request.FromLevel.HasValue || request.ToLevel.HasValue))
        {
            var fromLevel = request.FromLevel ?? 0;
            var toLevel = request.ToLevel ?? int.MaxValue;
            initQuery = initQuery.Where(x => x.Level >= fromLevel && x.Level <= toLevel);
        }

        if (request.AgeId != null)
        {
            initQuery = initQuery.Where(x => x.Age == request.AgeId);
        }

        List<string> result;
        switch (request.Mode)
        {
            case HeroInsightsMode.Top:
            {
                var topLevels = await initQuery
                    .GroupBy(x => x.Level)
                    .OrderByDescending(g => g.Key)
                    .Select(g => g.Key)
                    .Take(FogConstants.MAX_TOP_HERO_LEVELS_TO_RETURN)
                    .ToHashSetAsync(cancellationToken);
                result = await initQuery
                    .Where(x => topLevels.Contains(x.Level))
                    .Select(x => x.UnitId)
                    .ToListAsync(cancellationToken);
                break;
            }
            default:
            {
                result = await initQuery
                    .GroupBy(x => x.UnitId)
                    .Select(g => new {UnitId = g.Key, Count = g.Count()})
                    .OrderByDescending(g => g.Count)
                    .Take(FogConstants.MAX_MOST_POPULAR_HEROES_TO_RETURN)
                    .Select(x => x.UnitId)
                    .ToListAsync(cancellationToken);
                break;
            }
        }

        return result.ToHashSet();
    }
}
