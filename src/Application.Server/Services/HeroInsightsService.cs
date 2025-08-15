using FluentResults;
using FluentResults.Extensions;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.Services;

public class HeroInsightsService(IFogDbContext context) : IHeroInsightsService
{
    public async Task<Result<IReadOnlySet<string>>> GetAsync(HeroInsightsMode mode, string? ageId, int? fromLevel,
        int? toLevel, CancellationToken cancellationToken)
    {
        var minCollectionDate = DateTime.UtcNow.ToDateOnly().AddDays(-FogConstants.TOP_HEROES_LOOKBACK_DAYS);
        var initQuery = context.ProfileSquads.AsNoTracking().Where(x => x.CollectedAt > minCollectionDate);

        if (mode == HeroInsightsMode.MostPopular && (fromLevel.HasValue || toLevel.HasValue))
        {
            var fLevel = fromLevel ?? 0;
            var tLevel = toLevel ?? int.MaxValue;
            initQuery = initQuery.Where(x => x.Level >= fLevel && x.Level <= tLevel);
        }

        if (ageId != null)
        {
            initQuery = initQuery.Where(x => x.Age == ageId);
        }

        Result<List<string>> result;
        switch (mode)
        {
            case HeroInsightsMode.Top:
            {
                result = await Result.Try(() => initQuery
                        .Select(x => new {x.UnitId, x.Level})
                        .ToListAsync(cancellationToken))
                    .Bind(topLevels => Result.Try(() => topLevels
                        .GroupBy(x => x.Level)
                        .OrderByDescending(g => g.Key)
                        .Take(FogConstants.MAX_TOP_HERO_LEVELS_TO_RETURN)
                        .SelectMany(x => x.Select(y => y.UnitId))
                        .Distinct()
                        .ToList()));
                break;
            }
            default:
            {
                result = await Result.Try(() => initQuery
                    .GroupBy(x => x.UnitId)
                    .Select(g => new {UnitId = g.Key, Count = g.Count()})
                    .OrderByDescending(g => g.Count)
                    .Take(FogConstants.MAX_MOST_POPULAR_HEROES_TO_RETURN)
                    .Select(x => x.UnitId)
                    .ToListAsync(cancellationToken));
                break;
            }
        }

        return result.IsSuccess
            ? Result.Ok<IReadOnlySet<string>>(result.Value.ToHashSet())
            : result.ToResult<IReadOnlySet<string>>();
    }
}
