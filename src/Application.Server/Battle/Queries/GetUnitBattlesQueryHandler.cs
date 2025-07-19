using System.Globalization;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Battle.Queries;

public record GetUnitBattlesQuery(string UnitId, BattleType BattleType)
    : IRequest<IReadOnlyCollection<UnitBattleDto>>, ICacheableRequest
{
    public string CacheKey => $"UnitBattles_{UnitId}_{BattleType}_{CultureInfo.CurrentCulture.Name}";
    public TimeSpan? Duration { get; }
    public DateTimeOffset? Expiration => DateTimeUtils.GetNextMidnightUtc();
}

public class GetUnitBattlesQueryHandler(
    IUnitBattleDtoFactory unitBattleDtoFactory,
    IFogDbContext context,
    IBattleQueryService battleQueryService,
    ILogger<GetUnitBattlesQueryHandler> logger)
    : IRequestHandler<GetUnitBattlesQuery, IReadOnlyCollection<UnitBattleDto>>
{
    public async Task<IReadOnlyCollection<UnitBattleDto>> Handle(GetUnitBattlesQuery request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UnitId))
        {
            return [];
        }

        var allBattles = await GetBattles(request.UnitId, request.BattleType, cancellationToken);
        if (allBattles.Count == 0)
        {
            return [];
        }

        var battleIds = allBattles.Select(src => src.InGameBattleId);
        var existingStats = await battleQueryService.GetExistingBattleStatsAsync(battleIds, cancellationToken);

        var dtos = await unitBattleDtoFactory.CreateUnitBattles(allBattles, request.UnitId, existingStats);
        var uniqueDtos = dtos.Where(src => src.BattleStats != null).GroupBy(src => src);
        var averaged = uniqueDtos.Select(src => src.Count() == 1 ? src.First() : CreateAverage(src.ToList())).ToList();
        
        logger.LogInformation("Final averaged battle result has {Battles} items.", averaged.Count);
        
        return averaged.Take(FogConstants.MaxDisplayedUnitBattles).ToList();
    }

    private async Task<List<BattleSummaryEntity>> GetBattles(string unitId, BattleType battleType,
        CancellationToken cancellationToken)
    {
        var limit = FogConstants.MaxDisplayedUnitBattles * 2;
        
        List<BattleSummaryEntity> allBattles = [];
        var initQuery = context.BattleUnits.AsNoTracking()
            .Where(bue => bue.UnitId == unitId)
            .SelectMany(bue => bue.Battles)
            .OrderByDescending(bse => bse.Id);

        var battleTypePrefix = battleType.GetPrefixForBattleType();
        if (!string.IsNullOrWhiteSpace(battleTypePrefix))
        {
            allBattles = await initQuery
                .Where(x => x.BattleDefinitionId.StartsWith(battleTypePrefix))
                .Take(limit)
                .ToListAsync(cancellationToken);

            logger.LogInformation("Final battle selection complete. Selected {Battles} battles with prefix search.",
                allBattles.Count);

            return allBattles;
        }

        var currentRun = 0;

        while (allBattles.Count < FogConstants.MaxDisplayedUnitBattles)
        {
            var batch = await initQuery
                .Skip(currentRun * limit)
                .Take(limit)
                .ToListAsync(cancellationToken);

            if (batch.Count == 0)
            {
                break;
            }

            allBattles.AddRange(batch.Where(b => b.BattleDefinitionId.ToBattleType() == battleType));

            logger.LogDebug("Fetch attempt {RunNumber}: Retrieved {Battles} battles.",
                currentRun + 1, allBattles.Count);
            
            currentRun++;
        }

        logger.LogInformation("Final battle selection complete. Selected {Battles} battles after {Runs} runs.",
            allBattles.Count, currentRun);

        return allBattles.ToList();
    }

    private static UnitBattleDto CreateAverage(List<UnitBattleDto> dtos)
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
