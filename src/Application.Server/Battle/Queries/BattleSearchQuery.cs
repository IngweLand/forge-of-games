using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Fog.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.Battle.Queries;

public record BattleSearchQuery : IRequest<BattleSearchResult>
{
    public required string BattleDefinitionId { get; init; }
    public IReadOnlyCollection<string> UnitIds { get; init; } = new List<string>();
}

public class BattleSearchQueryHandler(IBattleSearchResultFactory battleSearchResultFactory, IFogDbContext context)
    : IRequestHandler<BattleSearchQuery, BattleSearchResult>
{
    public async Task<BattleSearchResult> Handle(BattleSearchQuery request,
        CancellationToken cancellationToken)
    {
        IQueryable<BattleSummaryEntity> battlesQuery;
        if (request.UnitIds.Count > 0)
        {
            var unitIds = request.UnitIds.ToHashSet();
            battlesQuery = context.Battles.AsNoTracking()
                .Where(b => b.BattleDefinitionId == request.BattleDefinitionId &&
                            unitIds.All(requiredId => b.Units.Any(u => u.UnitId == requiredId)));
        }
        else
        {
            battlesQuery = context.Battles.AsNoTracking()
                .Where(src => src.BattleDefinitionId == request.BattleDefinitionId);
        }


        var battles = await battlesQuery
            .OrderByDescending(src => src.Id)
            .Take(FogConstants.MaxDisplayedBattles)
            .ToListAsync(cancellationToken);

        return await battleSearchResultFactory.Create(battles);
    }
}
