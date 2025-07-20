using System.Globalization;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.StatsHub.Factories;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Hoh.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetPlayerProfileQuery : IRequest<PlayerProfile?>, ICacheableRequest
{
    public required int PlayerId { get; init; }
    public string CacheKey => $"PlayerProfile_{PlayerId}_{CultureInfo.CurrentCulture.Name}";
    public TimeSpan? Duration => TimeSpan.FromHours(6);
    public DateTimeOffset? Expiration { get; }
}

public class GetPlayerProfileQueryHandler(
    IFogDbContext context,
    IPlayerProfileFactory playerProfileFactory,
    IBattleQueryService battleQueryService)
    : IRequestHandler<GetPlayerProfileQuery, PlayerProfile?>
{
    public async Task<PlayerProfile?> Handle(GetPlayerProfileQuery request, CancellationToken cancellationToken)
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
            .Include(p => p.PvpWins.OrderByDescending(b => b.PerformedAt).Take(FogConstants.DefaultPlayerProfileDisplayedBattleCount))
            .ThenInclude(b => b.Loser)
            .Include(p => p.PvpLosses.OrderByDescending(b => b.PerformedAt).Take(FogConstants.DefaultPlayerProfileDisplayedBattleCount))
            .ThenInclude(b => b.Winner)
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == request.PlayerId, cancellationToken);

        if (player == null)
        {
            return null;
        }

        var pvpBattles = player.PvpWins.Concat(player.PvpLosses)
            .OrderByDescending(b => b.PerformedAt)
            .Take(FogConstants.DefaultPlayerProfileDisplayedBattleCount)
            .ToList();
        var battleIds = pvpBattles.Select(src => src.InGameBattleId);
        var existingStatsIds = await battleQueryService.GetExistingBattleStatsIdsAsync(battleIds, cancellationToken);

        return playerProfileFactory.Create(player, pvpBattles, existingStatsIds);
    }
}
