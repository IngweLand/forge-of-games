using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.StatsHub.Factories;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetPlayerQuery : IRequest<PlayerWithRankings?>
{
    public required PlayerKey PlayerKey { get; init; }
}

public class GetPlayerQueryHandler(IFogDbContext context, IPlayerWithRankingsFactory playerWithRankingsFactory)
    : IRequestHandler<GetPlayerQuery, PlayerWithRankings?>
{
    public async Task<PlayerWithRankings?> Handle(GetPlayerQuery request, CancellationToken cancellationToken)
    {
        var periodStartDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(FogConstants.DisplayedStatsDays * -1);
        var player = await context.Players.AsNoTracking()
            .Include(p => p.Rankings.Where(pr => pr.CollectedAt > periodStartDate))
            .FirstOrDefaultAsync(
                p => p.WorldId == request.PlayerKey.WorldId && p.InGamePlayerId == request.PlayerKey.InGamePlayerId,
                cancellationToken: cancellationToken);

        return player == null ? null : playerWithRankingsFactory.Create(player);
    }
}
