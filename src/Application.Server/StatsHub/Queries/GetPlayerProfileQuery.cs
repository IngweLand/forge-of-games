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
        var player = await context.Players.AsNoTracking()
            .Include(p => p.NameHistory)
            .Include(p => p.AgeHistory)
            .Include(p => p.AllianceHistory)
            .Include(p => p.AllianceMembership).ThenInclude(x => x!.Alliance)
            .Include(p => p.PvpWins.Take(1))
            .ThenInclude(b => b.Loser)
            .Include(p => p.PvpLosses.Take(1))
            .ThenInclude(b => b.Winner)
            .Include(p => p.Squads).ThenInclude(x => x.Data)
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
        var hasPvpBattles = player.PvpWins.Count != 0 || player.PvpLosses.Count != 0;

        logger.LogDebug("Retrieving city snapshot days for player {PlayerId}", player.Id);
        var citySnapshotDays = await context.PlayerCitySnapshots.Where(x => x.PlayerId == player.Id)
            .Select(x => x.CollectedAt).ToListAsync(cancellationToken);
        logger.LogDebug("Retrieved {Count} city snapshot days for player {PlayerId}", citySnapshotDays.Count,
            player.Id);

        logger.LogDebug("Creating player profile for {PlayerId}", request.PlayerId);
        var result = playerProfileDtoFactory.Create(player, hasPvpBattles, citySnapshotDays);
        logger.LogInformation("Successfully created profile for player {PlayerId}", request.PlayerId);
        return result;
    }
}
