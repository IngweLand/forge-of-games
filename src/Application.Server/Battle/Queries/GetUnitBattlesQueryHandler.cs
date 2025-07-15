using System.Globalization;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Shared.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.Battle.Queries;

public record GetUnitBattlesQuery(string UnitId) : IRequest<IReadOnlyCollection<UnitBattleDto>>, ICacheableRequest
{
    public string CacheKey => $"UnitBattles_{UnitId}_{CultureInfo.CurrentCulture.Name}";
    public TimeSpan? Duration { get; }
    public DateTimeOffset? Expiration => DateTimeUtils.GetNextMidnightUtc();
}

public class GetUnitBattlesQueryHandler(
    IUnitBattleDtoFactory unitBattleDtoFactory,
    IFogDbContext context,
    IBattleQueryService battleQueryService)
    : IRequestHandler<GetUnitBattlesQuery, IReadOnlyCollection<UnitBattleDto>>
{
    public async Task<IReadOnlyCollection<UnitBattleDto>> Handle(GetUnitBattlesQuery request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UnitId))
        {
            return [];
        }

        var allBattles = await context.BattleUnits.AsNoTracking()
            .Where(bue => bue.UnitId == request.UnitId)
            .SelectMany(bue => bue.Battles)
            .OrderByDescending(bse => bse.Id)
            .Take(FogConstants.MaxDisplayedUnitBattles * 4)
            .ToListAsync(cancellationToken);

        var battleIds = allBattles.Select(src => src.InGameBattleId);
        var existingStats = await battleQueryService.GetExistingBattleStatsAsync(battleIds, cancellationToken);

        var dtos = await unitBattleDtoFactory.CreateUnitBattles(allBattles, request.UnitId, existingStats);
        var uniqueDtos = dtos.Where(src => src.BattleStats != null).GroupBy(src => src);
        var averaged = uniqueDtos.Select(src =>
        {
            if (src.Count() == 1)
            {
                return src.First();
            }

            return CreateAverage(src.ToList());
        }).ToList();
        return averaged.Take(FogConstants.MaxDisplayedUnitBattles).ToList();
    }

    private UnitBattleDto CreateAverage(IList<UnitBattleDto> dtos)
    {
        var first = dtos.First();
        var averageStats = new UnitBattleStatsDto
        {
            Name = first.BattleStats!.Name,
            AssetId = first.BattleStats.AssetId,
            UnitId = first.BattleStats.UnitId,
            Attack = dtos.Average(src => src.BattleStats!.Attack),
            Defense = dtos.Average(src => src.BattleStats!.Defense),
            Heal = dtos.Average(src => src.BattleStats!.Heal),
        };
        return first with {BattleStats = averageStats};
    }
}
