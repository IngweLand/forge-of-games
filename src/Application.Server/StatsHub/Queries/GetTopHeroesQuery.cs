using AutoMapper;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Shared.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetTopHeroesQuery : IRequest<IReadOnlyCollection<string>>, ICacheableRequest
{
    public string CacheKey => "TopHeroes";
    public TimeSpan? Duration { get; }
    public DateTimeOffset? Expiration => DateTimeUtils.GetNextMidnightUtc().AddHours(6);
}

public class GetTopHeroesQueryHandler(IFogDbContext context, IMapper mapper)
    : IRequestHandler<GetTopHeroesQuery, IReadOnlyCollection<string>>
{
    public async Task<IReadOnlyCollection<string>> Handle(GetTopHeroesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await context.ProfileSquads
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
