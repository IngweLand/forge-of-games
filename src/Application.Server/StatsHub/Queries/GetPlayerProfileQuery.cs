using System.Globalization;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.Application.Server.StatsHub.Factories;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.InnSdk.Hoh.Errors;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Extensions;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetPlayerProfileQuery : IRequest<PlayerProfileDto?>, ICacheableRequest
{
    public required int PlayerId { get; init; }
    public TimeSpan? Duration => TimeSpan.FromHours(3);
    public DateTimeOffset? Expiration { get; }
}

public class GetPlayerProfileQueryHandler(
    IFogDbContext context,
    IPlayerProfileDtoFactory playerProfileDtoFactory,
    IBattleQueryService battleQueryService,
    IInGamePlayerService inGamePlayerService,
    IFogPlayerService playerService,
    IFogAllianceService allianceService,
    IAppCache appCache,
    ICacheKeyFactory cacheKeyFactory,
    ILogger<GetPlayerProfileQueryHandler> logger)
    : IRequestHandler<GetPlayerProfileQuery, PlayerProfileDto?>
{
    public async Task<PlayerProfileDto?> Handle(GetPlayerProfileQuery request, CancellationToken cancellationToken)
    {
        logger.LogDebug("Processing GetPlayerProfileQuery for PlayerId: {PlayerId}", request.PlayerId);

        var existingPlayer = await context.Players.FindAsync(request.PlayerId, cancellationToken);
        if (existingPlayer == null)
        {
            logger.LogInformation("Player with ID {PlayerId} not found", request.PlayerId);
            return null;
        }

        var now = DateTime.UtcNow;
        var today = now.ToDateOnly();
        if (existingPlayer.ProfileUpdatedAt < today && existingPlayer.Status == InGameEntityStatus.Active)
        {
            logger.LogDebug("Player profile for ID {PlayerId} needs update (UpdatedAt: {UpdatedAt}, Today: {Today})",
                request.PlayerId, existingPlayer.ProfileUpdatedAt, today);
            var newProfileResult = await inGamePlayerService.FetchProfile(existingPlayer.Key);
            if (newProfileResult.IsSuccess)
            {
                logger.LogDebug("New profile fetched for player {PlayerId}, updating data", request.PlayerId);
                if (newProfileResult.Value.Alliance != null)
                {
                    await allianceService.UpsertAlliance(newProfileResult.Value.Alliance, existingPlayer.WorldId, now);
                }
                await playerService.UpsertPlayerAsync(newProfileResult.Value, existingPlayer.WorldId);
            }
            else if (newProfileResult.HasError<PlayerNotFoundError>())
            {
                await playerService.UpdateStatusAsync(existingPlayer.Id, InGameEntityStatus.Missing, cancellationToken);
            }
            else
            {
                logger.LogError(
                    "Error while fetching or updating player profile for player {@PlayerKey}. Errors: {Errors}",
                    existingPlayer.Key, newProfileResult.Errors);
            }
        }

        logger.LogDebug("Retrieving detailed player data for {PlayerId}", request.PlayerId);
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
            .Include(p => p.AllianceMembership).ThenInclude(x => x!.Alliance)
            .Include(p =>
                p.PvpWins.OrderByDescending(b => b.PerformedAt)
                    .Take(FogConstants.DefaultPlayerProfileDisplayedBattleCount))
            .ThenInclude(b => b.Loser)
            .Include(p =>
                p.PvpLosses.OrderByDescending(b => b.PerformedAt)
                    .Take(FogConstants.DefaultPlayerProfileDisplayedBattleCount))
            .ThenInclude(b => b.Winner)
            .Include(p => p.Squads)
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == request.PlayerId, cancellationToken);

        if (player == null)
        {
            logger.LogWarning("Detailed player data not found for {PlayerId} after attempting to retrieve it",
                request.PlayerId);
            return null;
        }
        
        if (player.AllianceMembership != null)
        {
            appCache.Remove(cacheKeyFactory.Alliance(player.AllianceMembership.AllianceId));
        }

        logger.LogDebug("Processing PVP battles for player {PlayerId}", request.PlayerId);
        var pvpBattles = player.PvpWins.Concat(player.PvpLosses)
            .OrderByDescending(b => b.PerformedAt)
            .Take(FogConstants.DefaultPlayerProfileDisplayedBattleCount)
            .ToList();

        logger.LogDebug("Found {BattleCount} PVP battles for player {PlayerId}", pvpBattles.Count, request.PlayerId);
        var battleIds = pvpBattles.Select(src => src.InGameBattleId);
        var existingStatsIds = await battleQueryService.GetExistingBattleStatsIdsAsync(battleIds, cancellationToken);
        logger.LogDebug("Retrieved {StatsCount} battle stats IDs for player {PlayerId}", existingStatsIds.Count,
            request.PlayerId);

        logger.LogDebug("Creating player profile for {PlayerId}", request.PlayerId);
        var result = playerProfileDtoFactory.Create(player, pvpBattles, existingStatsIds);
        logger.LogInformation("Successfully created profile for player {PlayerId}", request.PlayerId);
        return result;
    }
}
