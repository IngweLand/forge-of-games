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

public record BattleSearchQuery : IRequest<BattleSearchResult>, ICacheableRequest
{
    public required string BattleDefinitionId { get; init; }
    public required BattleType BattleType { get; init; }
    public IReadOnlyCollection<string> UnitIds { get; init; } = new List<string>();
    public TimeSpan? Duration => TimeSpan.FromHours(3);
    public DateTimeOffset? Expiration { get; }
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
        IQueryable<BattleSummaryEntity> battlesQuery;
        if (request.UnitIds.Count > 0)
        {
            var unitIds = request.UnitIds.ToHashSet();
            battlesQuery = BuildBattleQuery(unitIds, request.BattleDefinitionId, request.BattleType);
        }
        else
        {
            battlesQuery = context.Battles.AsNoTracking()
                .Where(src => src.BattleDefinitionId == request.BattleDefinitionId);
        }

        var battles = await battlesQuery
            .OrderByDescending(src => src.PerformedAt)
            .ThenByDescending(src => src.Id)
            .Take(FogConstants.MaxDisplayedBattles)
            .ToListAsync(cancellationToken);
        var battleIds = battles.Select(src => src.InGameBattleId);

        var existingStatsIds = await battleQueryService.GetExistingBattleStatsIdsAsync(battleIds, cancellationToken);
        return await battleSearchResultFactory.Create(battles, existingStatsIds);
    }

    private IQueryable<BattleSummaryEntity> BuildBattleQuery(HashSet<string> unitIds, string battleDefinitionId,
        BattleType battleType)
    {
        if (battleType == BattleType.Pvp)
        {
            return context.Battles.AsNoTracking()
                .Where(b => b.BattleDefinitionId == battleDefinitionId &&
                    unitIds.All(requiredId => b.Units.Any(u => u.UnitId == requiredId)));
        }

        return context.Battles.AsNoTracking()
            .Where(b => b.BattleDefinitionId == battleDefinitionId &&
                unitIds.All(requiredId =>
                    b.Units.Any(u => u.UnitId == requiredId && u.Side == BattleSquadSide.Player)));
    }
}
