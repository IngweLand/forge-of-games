using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.Battle.Queries;

public record UserBattleSearchQuery : IRequest<PaginatedList<BattleSummaryDto>>
{
    public string? BattleDefinitionId { get; init; }
    public BattleType BattleType { get; init; }

    public int Count { get; init; }
    public int StartIndex { get; init; }
    public required Guid SubmissionId { get; init; }
    public IReadOnlyCollection<string> UnitIds { get; init; } = new List<string>();
}

public class PlayerBattleSearchQueryHandler(
    IBattleSearchResultFactory battleSearchResultFactory,
    IFogDbContext context,
    IBattleQueryService battleQueryService)
    : IRequestHandler<UserBattleSearchQuery, PaginatedList<BattleSummaryDto>>
{
    public async Task<PaginatedList<BattleSummaryDto>> Handle(UserBattleSearchQuery request,
        CancellationToken cancellationToken)
    {
        var battlesQuery = context.Battles.AsNoTracking()
            .Include(x => x.Squads)
            .Where(src => src.SubmissionId == request.SubmissionId && src.BattleType == request.BattleType);
        if (request.BattleDefinitionId != null)
        {
            if (request.UnitIds.Count > 0)
            {
                var unitIds = request.UnitIds.ToHashSet();

                if (request.BattleType == BattleType.Pvp)
                {
                    battlesQuery = battlesQuery
                        .Where(b => b.BattleDefinitionId == request.BattleDefinitionId &&
                            unitIds.All(requiredId => b.Units.Any(u => u.UnitId == requiredId)));
                }
                else
                {
                    battlesQuery = battlesQuery.Where(b => b.BattleDefinitionId == request.BattleDefinitionId &&
                        unitIds.All(requiredId =>
                            b.Units.Any(u => u.UnitId == requiredId && u.Side == BattleSquadSide.Player)));
                }
            }
            else
            {
                battlesQuery = battlesQuery.Where(src => src.BattleDefinitionId == request.BattleDefinitionId);
            }
        }

        battlesQuery = battlesQuery.OrderByDescending(src => src.PerformedAt).ThenByDescending(src => src.Id);
        var count = await battlesQuery.CountAsync(cancellationToken);

        if (count == 0)
        {
            return PaginatedList<BattleSummaryDto>.Empty;
        }

        var battles = await battlesQuery
            .Skip(request.StartIndex)
            .Take(request.Count)
            .ToListAsync(cancellationToken);
        var battleIds = battles.Select(src => src.InGameBattleId);

        var existingStatsIds = await battleQueryService.GetExistingBattleStatsIdsAsync(battleIds, cancellationToken);
        var searchResult = await battleSearchResultFactory.Create(battles, existingStatsIds);

        return new PaginatedList<BattleSummaryDto>(searchResult.Battles, request.StartIndex, count);
    }
}
