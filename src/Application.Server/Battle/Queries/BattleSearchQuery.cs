using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.Battle.Queries;

public record BattleSearchQuery : IRequest<BattleSearchResult>
{
    public required string BattleDefinitionId { get; init; }
    public required BattleType BattleType { get; init; }
    public IReadOnlyCollection<string> UnitIds { get; init; } = new List<string>();
}

public class BattleSearchQueryHandler(
    IBattleSearchResultFactory battleSearchResultFactory,
    IFogDbContext context,
    IBattleQueryService battleQueryService)
    : IRequestHandler<BattleSearchQuery, BattleSearchResult>
{
    public async Task<BattleSearchResult> Handle(BattleSearchQuery request,
        CancellationToken cancellationToken)
    {
        IQueryable<BattleSummaryEntity> playerBattlesQuery;
        IQueryable<BattleSummaryEntity>? enemyBattlesQuery = null;
        if (request.UnitIds.Count > 0)
        {
            var unitIds = request.UnitIds.ToHashSet();
            playerBattlesQuery = BuildBattleQuery(unitIds, request.BattleDefinitionId, BattleSquadSide.Player);
            if (request.BattleType == BattleType.Pvp)
            {
                enemyBattlesQuery = BuildBattleQuery(unitIds, request.BattleDefinitionId, BattleSquadSide.Enemy);
            }
        }
        else
        {
            playerBattlesQuery = context.Battles.AsNoTracking()
                .Where(src => src.BattleDefinitionId == request.BattleDefinitionId);
        }

        var playerBattles = await playerBattlesQuery
            .OrderByDescending(src => src.Id)
            .Take(FogConstants.MaxDisplayedBattles)
            .ToListAsync(cancellationToken);
        var battleIds = playerBattles.Select(src => src.InGameBattleId);
        List<BattleSummaryEntity>? enemyBattles = null;
        if (enemyBattlesQuery != null)
        {
            enemyBattles = await enemyBattlesQuery
                .OrderByDescending(src => src.Id)
                .Take(FogConstants.MaxDisplayedBattles)
                .ToListAsync(cancellationToken);
            
            battleIds = battleIds.Concat(enemyBattles.Select(b => b.InGameBattleId));
        }

        var existingStatsIds = await battleQueryService.GetExistingBattleStatsIdsAsync(battleIds, cancellationToken);
        var playerResult = await battleSearchResultFactory.Create(playerBattles, existingStatsIds);
        if (enemyBattles == null)
        {
            return playerResult;
        }

        var enemyResult = await battleSearchResultFactory.Create(enemyBattles, existingStatsIds, BattleSquadSide.Enemy);
        return new BattleSearchResult
        {
            Battles = playerResult.Battles.Concat(enemyResult.Battles).ToList(),
            Heroes = playerResult.Heroes.Concat(enemyResult.Heroes).DistinctBy(h => h.Id).ToList(),
        };

    }

    private IQueryable<BattleSummaryEntity> BuildBattleQuery(HashSet<string> unitIds, string battleDefinitionId,
        BattleSquadSide side)
    {
        return context.Battles.AsNoTracking()
            .Where(b => b.BattleDefinitionId == battleDefinitionId &&
                unitIds.All(requiredId => b.Units.Any(u => u.UnitId == requiredId && u.Side == side)));
    }
}
