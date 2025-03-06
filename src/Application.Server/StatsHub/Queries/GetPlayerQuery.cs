using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.StatsHub.Factories;
using Ingweland.Fog.Dtos.Hoh.Stats;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetPlayerQuery : IRequest<PlayerWithRankings?>
{
    public required int PlayerId { get; init; }
}

public class GetPlayerQueryHandler(IFogDbContext context, IPlayerWithRankingsFactory playerWithRankingsFactory)
    : IRequestHandler<GetPlayerQuery, PlayerWithRankings?>
{
    public async Task<PlayerWithRankings?> Handle(GetPlayerQuery request, CancellationToken cancellationToken)
    {
        var periodStartDate = DateTime.UtcNow.AddDays(FogConstants.DisplayedStatsDays * -1);
        var periodStartDateOnly = DateOnly.FromDateTime(periodStartDate);
        var player = await context.Players.AsNoTracking()
            .Include(p => p.Rankings.Where(pr => pr.CollectedAt > periodStartDateOnly))
            .Include(p => p.PvpRankings.Where(pr => pr.CollectedAt > periodStartDate))
            .Include(p => p.NameHistory)
            .Include(p => p.AgeHistory)
            .Include(p => p.AllianceHistory)
            .Include(p => p.AllianceNameHistory)
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == request.PlayerId, cancellationToken: cancellationToken);

        return player == null ? null : playerWithRankingsFactory.Create(player);
    }
}