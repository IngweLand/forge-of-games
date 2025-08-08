using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.Battle;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.Battle.Queries;

public record GetBattleQuery(int Id) : IRequest<BattleSummaryDto?>, ICacheableRequest
{
    public TimeSpan? Duration => TimeSpan.FromDays(30);
    public DateTimeOffset? Expiration { get; }
}

public class GetBattleQueryHandler(
    IBattleSearchResultFactory battleSearchResultFactory,
    IBattleQueryService battleQueryService,
    IFogDbContext context)
    : IRequestHandler<GetBattleQuery, BattleSummaryDto?>
{
    public async Task<BattleSummaryDto?> Handle(GetBattleQuery request,
        CancellationToken cancellationToken)
    {
        var battle = await context.Battles.AsNoTracking()
            .FirstOrDefaultAsync(src => src.Id == request.Id, cancellationToken);
        if (battle == null)
        {
            return null;
        }

        var existingStats =
            await battleQueryService.GetExistingBattleStatsAsync(battle.InGameBattleId, cancellationToken);
        return battleSearchResultFactory.Create(battle, existingStats?.Id);
    }
}
