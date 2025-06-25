using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Battle;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.Battle.Queries;

public record GetBattleStatsQuery(int Id) : IRequest<BattleStatsDto?>;

public class GetBattleStatsQueryHandler(IBattleStatsDtoFactory battleStatsDtoFactory, IFogDbContext context)
    : IRequestHandler<GetBattleStatsQuery, BattleStatsDto?>
{
    public async Task<BattleStatsDto?> Handle(GetBattleStatsQuery request,
        CancellationToken cancellationToken)
    {
        var battleStats = await context.BattleStats.AsNoTracking()
            .Include(e => e.Squads).ThenInclude(src => src.Hero)
            .Include(e => e.Squads).ThenInclude(src => src.SupportUnit)
            .FirstOrDefaultAsync(src => src.Id == request.Id, cancellationToken);
        if (battleStats == null)
        {
            return null;
        }

        return await battleStatsDtoFactory.Create(battleStats);
    }
}
