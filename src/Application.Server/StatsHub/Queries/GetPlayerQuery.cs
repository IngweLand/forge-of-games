using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.StatsHub.Factories;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Hoh.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetPlayerQuery : IRequest<PlayerWithRankings?>
{
    public required int PlayerId { get; init; }
}

public class GetPlayerQueryHandler(
    IFogDbContext context,
    IPlayerWithRankingsFactory playerWithRankingsFactory,
    IBattleQueryService battleQueryService)
    : IRequestHandler<GetPlayerQuery, PlayerWithRankings?>
{
    public async Task<PlayerWithRankings?> Handle(GetPlayerQuery request, CancellationToken cancellationToken)
    {
        var periodStartDate = DateTime.UtcNow.AddDays(FogConstants.DisplayedStatsDays * -1);
        var periodStartDateOnly = DateOnly.FromDateTime(periodStartDate);
        var player = await context.Players.AsNoTracking()
            .Include(p =>
                p.Rankings.Where(pr =>
                    pr.Type == PlayerRankingType.PowerPoints && pr.CollectedAt > periodStartDateOnly))
            .Include(p => p.PvpRankings.Where(pr => pr.CollectedAt > periodStartDate))
            .Include(p => p.NameHistory)
            .Include(p => p.AgeHistory)
            .Include(p => p.AllianceHistory)
            .Include(p => p.AllianceNameHistory)
            .Include(p => p.PvpWins.OrderByDescending(b => b.PerformedAt).Take(FogConstants.MaxDisplayedPvpBattles))
            .ThenInclude(b => b.Loser)
            .Include(p => p.PvpLosses.OrderByDescending(b => b.PerformedAt).Take(FogConstants.MaxDisplayedPvpBattles))
            .ThenInclude(b => b.Winner)
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == request.PlayerId, cancellationToken);

        if (player == null)
        {
            return null;
        }

        var pvpBattles = player.PvpWins.Concat(player.PvpLosses)
            .OrderByDescending(b => b.PerformedAt)
            .Take(FogConstants.MaxDisplayedPvpBattles)
            .ToList();
        var battleIds = pvpBattles.Select(src => src.InGameBattleId);
        var existingStatsIds = await battleQueryService.GetExistingBattleStatsIdsAsync(battleIds, cancellationToken);

        return await playerWithRankingsFactory.CreateAsync(player, pvpBattles, existingStatsIds);
    }
}
