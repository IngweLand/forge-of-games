using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.Battle;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.Battle.Queries;

public record GetBattleQuery(int Id) : IRequest<BattleDto?>, ICacheableRequest
{
    public TimeSpan? Duration => TimeSpan.FromDays(30);
    public DateTimeOffset? Expiration { get; }
}

public class GetBattleQueryHandler(
    IBattleSearchResultFactory battleSearchResultFactory,
    IBattleQueryService battleQueryService,
    IFogDbContext context)
    : IRequestHandler<GetBattleQuery, BattleDto?>
{
    public async Task<BattleDto?> Handle(GetBattleQuery request,
        CancellationToken cancellationToken)
    {
        var battle = await context.Battles.AsNoTracking()
            .FirstOrDefaultAsync(src => src.Id == request.Id, cancellationToken);
        if (battle == null)
        {
            return null;
        }

        var timeline = await context.BattleTimelines.AsNoTracking()
            .FirstOrDefaultAsync(x => x.InGameBattleId == battle.InGameBattleId, cancellationToken);

        var existingStats =
            await battleQueryService.GetExistingBattleStatsAsync(battle.InGameBattleId, cancellationToken);
        return battleSearchResultFactory.Create(battle,
            timeline == null ? [] : timeline.Entries.OrderBy(x => x.TimeMillis).ToList(), existingStats?.Id);
    }
}
