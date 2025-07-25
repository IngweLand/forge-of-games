using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Shared.Extensions;
using Ingweland.Fog.Shared.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetTopHeroesQuery : IRequest<IReadOnlyCollection<string>>, ICacheableRequest
{
    public string? AgeId { get; init; }
    public int? FromLevel { get; init; } = 0;
    public int? ToLevel { get; init; } = int.MaxValue;
    public string CacheKey => $"TopHeroes-{AgeId}-{FromLevel}-{ToLevel}";
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
        
        if (request.FromLevel.HasValue || request.ToLevel.HasValue)
        {
            var fromLevel = request.FromLevel ?? 0;
            var toLevel = request.ToLevel ?? int.MaxValue;
            initQuery = initQuery.Where(x => x.Level >= fromLevel && x.Level <= toLevel);
        }

        if (request.AgeId != null)
        {
            initQuery = initQuery.Where(x => x.Age == request.AgeId);
        }

        var result = await initQuery
            .GroupBy(ps => ps.PlayerId)
            .Select(g => new
            {
                PlayerId = g.Key,
                LatestCollectedAt = g.Max(ps => ps.CollectedAt),
            })
            .Join(context.ProfileSquads,
                lt => new {lt.PlayerId, CollectedAt = lt.LatestCollectedAt},
                ps => new {ps.PlayerId, ps.CollectedAt},
                (lt, ps) => ps.UnitId)
            .GroupBy(x => x)
            .Select(g => new
            {
                UnitId = g.Key,
                Count = g.Count(),
            })
            .OrderByDescending(g => g.Count)
            .Take(FogConstants.TOP_HEROES_COUNT)
            .Select(x => x.UnitId)
            .ToListAsync(cancellationToken);

        return result;
    }
}
