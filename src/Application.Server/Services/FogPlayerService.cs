using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Alliance;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Services;

public class FogPlayerService(IFogDbContext context, ILogger<FogPlayerService> logger) : IFogPlayerService
{
    private readonly SemaphoreSlim _upsertSemaphore = new(1, 1);

    public async Task UpdateStatusAsync(IEnumerable<int> playerIds, PlayerStatus status,
        CancellationToken cancellationToken)
    {
        var uniqueIds = playerIds.ToHashSet();
        var players = await context.Players.Where(x => uniqueIds.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var player in players)
        {
            player.Status = status;
            if (status == PlayerStatus.Missing)
            {
                player.CurrentAlliance = null;
            }
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateStatusAsync(int playerId, PlayerStatus status, CancellationToken cancellationToken)
    {
        var player = await context.Players.FindAsync(playerId, cancellationToken);
        if (player == null)
        {
            logger.LogWarning("Player with id {PlayerId} not found", playerId);
            return;
        }

        player.Status = status;
        if (status == PlayerStatus.Missing)
        {
            player.CurrentAlliance = null;
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpsertPlayer(PlayerProfile profile, string worldId)
    {
        logger.LogInformation("Upserting player {PlayerId} from world {WorldId}", profile.Player.Id, worldId);
        await _upsertSemaphore.WaitAsync();
        try
        {
            logger.LogDebug("Acquired semaphore for player {PlayerId} from world {WorldId}",
                profile.Player.Id, worldId);
            await DoUpsertPlayer(profile, worldId);
            await context.SaveChangesAsync();
            logger.LogInformation("Successfully upserted player {PlayerId} from world {WorldId}",
                profile.Player.Id, worldId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error upserting player {PlayerId} from world {WorldId}: {ErrorMessage}",
                profile.Player.Id, worldId, ex.Message);
            throw;
        }
        finally
        {
            _upsertSemaphore.Release();
            logger.LogDebug("Released semaphore for player {PlayerId} from world {WorldId}",
                profile.Player.Id, worldId);
        }
    }

    private void UpsertSquads(Player player, IReadOnlyCollection<ProfileSquad> squads, DateOnly collectedAt)
    {
        logger.LogDebug("Upserting {SquadCount} squads for player {PlayerId}", squads.Count, player.InGamePlayerId);

        player.Squads.Clear();
        foreach (var squad in squads)
        {
            player.Squads.Add(new ProfileSquadEntity
            {
                UnitId = squad.Hero.UnitId,
                Level = squad.Hero.Level,
                AscensionLevel = squad.Hero.AscensionLevel,
                AbilityLevel = squad.Hero.AbilityLevel,
                CollectedAt = collectedAt,
                Hero = squad.Hero,
                SupportUnit = squad.SupportUnit,
                Age = player.Age,
            });
        }

        logger.LogDebug("Upserting squds completed.");
    }

    private async Task DoUpsertPlayer(PlayerProfile profile, string worldId)
    {
        var now = DateTime.UtcNow;
        var today = now.ToDateOnly();
        logger.LogDebug("Starting DoUpsertPlayer for {PlayerId} from world {WorldId}, date: {Today}",
            profile.Player.Id, worldId, today);

        Alliance? alliance = null;
        if (profile.Alliance != null)
        {
            logger.LogDebug("Upserting alliance {AllianceId} for player {PlayerId}",
                profile.Alliance.Id, profile.Player.Id);
            alliance = await UpsertAlliance(profile.Alliance, worldId, now);
        }

        logger.LogDebug("Querying existing player {PlayerId} from world {WorldId}",
            profile.Player.Id, worldId);
        var existingPlayer = await context.Players
            .Include(p =>
                p.Rankings.Where(pr => pr.Type == PlayerRankingType.PowerPoints && pr.CollectedAt == today))
            .Include(p => p.NameHistory)
            .Include(p => p.AgeHistory)
            .Include(p => p.AllianceHistory)
            .Include(p => p.Squads)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.InGamePlayerId == profile.Player.Id && x.WorldId == worldId);

        Player modifiedPlayer;
        if (existingPlayer != null)
        {
            logger.LogDebug("Updating existing player {PlayerId}", existingPlayer.Id);
            existingPlayer.Name = profile.Player.Name;
            existingPlayer.Age = profile.Player.Age;
            existingPlayer.UpdatedAt = today;

            modifiedPlayer = existingPlayer;
        }
        else
        {
            logger.LogInformation("Creating new player {PlayerId} from world {WorldId}", profile.Player.Id, worldId);
            var newPlayer = new Player
            {
                WorldId = worldId,
                InGamePlayerId = profile.Player.Id,
                Name = profile.Player.Name,
                Age = profile.Player.Age,
                UpdatedAt = today,
                Status = PlayerStatus.Active,
            };

            modifiedPlayer = newPlayer;
            context.Players.Add(newPlayer);
        }

        if (alliance != null)
        {
            logger.LogDebug("Setting alliance {AllianceId} for player {PlayerId}", alliance.Id, profile.Player.Id);
            modifiedPlayer.CurrentAlliance = alliance;
            if (modifiedPlayer.AllianceHistory.All(a => a.Id != alliance.Id))
            {
                modifiedPlayer.AllianceHistory.Add(alliance);
                logger.LogDebug("Added alliance {AllianceId} to history for player {PlayerId}",
                    alliance.Id, profile.Player.Id);
            }
        }
        else
        {
            logger.LogDebug("Clearing alliance for player {PlayerId}", profile.Player.Id);
            modifiedPlayer.CurrentAlliance = null;
            modifiedPlayer.LedAlliance = null;
        }

        modifiedPlayer.AvatarId = profile.Player.AvatarId;
        modifiedPlayer.TreasureHuntDifficulty = profile.TreasureHuntDifficulty;
        modifiedPlayer.PvpTier = profile.PvpTier;
        modifiedPlayer.Rank = profile.Rank;
        modifiedPlayer.RankingPoints = profile.RankingPoints;
        modifiedPlayer.Status = PlayerStatus.Active;

        if (modifiedPlayer.NameHistory.All(x => x.Name != profile.Player.Name))
        {
            logger.LogDebug("Adding name {PlayerName} to history for player {PlayerId}",
                profile.Player.Name, profile.Player.Id);
            modifiedPlayer.NameHistory.Add(new PlayerNameHistoryEntry {Name = profile.Player.Name});
        }

        if (modifiedPlayer.AgeHistory.OrderByDescending(x => x.ChangedAt).FirstOrDefault()?.Age != profile.Player.Age)
        {
            logger.LogDebug("Adding age {PlayerAge} to history for player {PlayerId}",
                profile.Player.Age, profile.Player.Id);
            modifiedPlayer.AgeHistory.Add(new PlayerAgeHistoryEntry {Age = profile.Player.Age, ChangedAt = now});
        }

        var existingRanking =
            modifiedPlayer.Rankings.FirstOrDefault(x =>
                x.Type == PlayerRankingType.PowerPoints && x.CollectedAt == today);
        if (existingRanking != null)
        {
            logger.LogDebug("Updating existing ranking for player {PlayerId}: Rank {Rank}, Points {Points}",
                profile.Player.Id, profile.Rank, profile.RankingPoints);
            existingRanking.Points = profile.RankingPoints;
            existingRanking.Rank = profile.Rank;
            existingRanking.CollectedAt = today;
        }
        else
        {
            logger.LogDebug("Adding new ranking for player {PlayerId}: Rank {Rank}, Points {Points}",
                profile.Player.Id, profile.Rank, profile.RankingPoints);
            modifiedPlayer.Rankings.Add(new PlayerRanking
            {
                Points = profile.RankingPoints,
                Rank = profile.Rank,
                CollectedAt = today,
                Type = PlayerRankingType.PowerPoints,
            });
        }

        UpsertSquads(modifiedPlayer, profile.Squads, today);

        logger.LogDebug("Saving changes for player {PlayerId} from world {WorldId}",
            profile.Player.Id, worldId);
    }

    private async Task<Alliance> UpsertAlliance(HohAlliance hohAlliance, string worldId, DateTime now)
    {
        logger.LogDebug("Upserting alliance {AllianceId} from world {WorldId}",
            hohAlliance.Id, worldId);
        var today = now.ToDateOnly();
        var existingAlliance = await context.Alliances
            .Include(p => p.NameHistory)
            .FirstOrDefaultAsync(x => x.InGameAllianceId == hohAlliance.Id && x.WorldId == worldId);

        Alliance alliance;
        if (existingAlliance != null)
        {
            logger.LogDebug("Updating existing alliance {AllianceId} with name {AllianceName} from world {WorldId}",
                hohAlliance.Id, hohAlliance.Name, worldId);
            existingAlliance.Name = hohAlliance.Name;
            existingAlliance.UpdatedAt = today;

            alliance = existingAlliance;
        }
        else
        {
            logger.LogInformation("Creating new alliance {AllianceId} from world {WorldId}", hohAlliance.Id, worldId);
            var newAlliance = new Alliance
            {
                WorldId = worldId,
                InGameAllianceId = hohAlliance.Id,
                Name = hohAlliance.Name,
                UpdatedAt = today,
            };

            alliance = newAlliance;
            context.Alliances.Add(newAlliance);
        }

        alliance.AvatarIconId = hohAlliance.AvatarIconId;
        alliance.AvatarBackgroundId = hohAlliance.AvatarBackgroundId;

        if (alliance.NameHistory.OrderByDescending(x => x.ChangedAt).FirstOrDefault()?.Name != hohAlliance.Name)
        {
            logger.LogDebug("Adding name {AllianceName} to history for alliance {AllianceId}", hohAlliance.Name,
                hohAlliance.Id);
            alliance.NameHistory.Add(new AllianceNameHistoryEntry {Name = hohAlliance.Name, ChangedAt = now});
        }

        logger.LogInformation("Successfully upserted alliance {AllianceId} from world {WorldId}", hohAlliance.Id,
            worldId);

        return alliance;
    }
}
