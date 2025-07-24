using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class PlayerProfileService(
    IGameWorldsProvider gameWorldsProvider,
    IInnSdkClient innSdkClient,
    IFogDbContext context,
    ILogger<PlayerProfileService> logger) : IPlayerProfileService
{
    private readonly SemaphoreSlim _upsertSemaphore = new(1, 1);

    public async Task<PlayerProfile?> FetchProfile(PlayerKey playerKey)
    {
        logger.LogInformation("Fetching profile for player {@PlayerKey}", playerKey);
        try
        {
            var gw = gameWorldsProvider.GetGameWorlds().FirstOrDefault(x => x.Id == playerKey.WorldId);
            if (gw == null)
            {
                logger.LogWarning("Could not find game world with id {WorldId}", playerKey.WorldId);
                return null;
            }

            logger.LogDebug("Calling PlayerService.GetPlayerProfileAsync for player {@PlayerKey}", playerKey);
            return await innSdkClient.PlayerService.GetPlayerProfileAsync(gw, playerKey.InGamePlayerId);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting player's profile {PlayerId} from world {WorldId}: {ErrorMessage}",
                playerKey.InGamePlayerId, playerKey.WorldId, e.Message);
            return null;
        }
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

    private Task UpsertSquads(Player player, IReadOnlyCollection<ProfileSquad> squads, DateOnly collectedAt)
    {
        logger.LogDebug("Upserting {SquadCount} squads for player {PlayerId}",
            squads.Count, player.InGamePlayerId);

        var addedCount = 0;
        var updatedCount = 0;

        foreach (var squad in squads)
        {
            var key = new ProfileSquadKey(player.Id, squad.Hero.UnitId, collectedAt);
            var existingSquad = player.Squads.FirstOrDefault(x => x.Key == key);
            if (existingSquad == null)
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
                });
                addedCount++;
            }
            else
            {
                existingSquad.Level = squad.Hero.Level;
                existingSquad.AscensionLevel = squad.Hero.AscensionLevel;
                existingSquad.AbilityLevel = squad.Hero.AbilityLevel;
                existingSquad.Hero = squad.Hero;
                existingSquad.SupportUnit = squad.SupportUnit;
                updatedCount++;
            }
        }

        logger.LogInformation(
            "Squad upsert complete: {AddedCount} squads added, {UpdatedCount} squads updated for player {PlayerId}",
            addedCount, updatedCount, player.InGamePlayerId);

        return context.SaveChangesAsync();
    }

    private async Task DoUpsertPlayer(PlayerProfile profile, string worldId)
    {
        var now = DateTime.UtcNow;
        var today = now.ToDateOnly();
        logger.LogDebug("Starting DoUpsertPlayer for {PlayerId} from world {WorldId}, date: {Today}",
            profile.Player.Id, worldId, today);

        Alliance existingAlliance = null!;
        if (profile.Alliance != null)
        {
            logger.LogDebug("Upserting alliance {AllianceId} for player {PlayerId}",
                profile.Alliance.Id, profile.Player.Id);
            existingAlliance = await UpsertAlliance(profile.Alliance, worldId, now);
        }

        logger.LogDebug("Querying existing player {PlayerId} from world {WorldId}",
            profile.Player.Id, worldId);
        var existingPlayer = await context.Players
            .Include(p =>
                p.Rankings.Where(pr => pr.Type == PlayerRankingType.PowerPoints && pr.CollectedAt == today))
            .Include(p => p.NameHistory)
            .Include(p => p.AgeHistory)
            .Include(p => p.AllianceHistory)
            .Include(p => p.AllianceNameHistory)
            .Include(p => p.Squads.Where(x => x.CollectedAt == today))
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
            };

            modifiedPlayer = newPlayer;
            context.Players.Add(newPlayer);
        }

        if (profile.Alliance != null)
        {
            logger.LogDebug("Setting alliance {AllianceId} for player {PlayerId}",
                profile.Alliance.Id, profile.Player.Id);
            modifiedPlayer.CurrentAlliance = existingAlliance;
            if (modifiedPlayer.AllianceHistory.All(a => a.Id != existingAlliance.Id))
            {
                modifiedPlayer.AllianceHistory.Add(existingAlliance);
                logger.LogDebug("Added alliance {AllianceId} to history for player {PlayerId}",
                    existingAlliance.Id, profile.Player.Id);
            }

            modifiedPlayer.AllianceName = existingAlliance.Name;
        }
        else
        {
            logger.LogDebug("Clearing alliance for player {PlayerId}", profile.Player.Id);
            modifiedPlayer.CurrentAlliance = null;
            modifiedPlayer.LedAlliance = null;
            modifiedPlayer.AllianceName = null;
        }

        modifiedPlayer.AvatarId = profile.Player.AvatarId;
        modifiedPlayer.TreasureHuntDifficulty = profile.TreasureHuntDifficulty;
        modifiedPlayer.PvpTier = profile.PvpTier;
        modifiedPlayer.Rank = profile.Rank;
        modifiedPlayer.RankingPoints = profile.RankingPoints;
        modifiedPlayer.IsPresentInGame = true;

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

        await UpsertSquads(modifiedPlayer, profile.Squads, today);

        logger.LogDebug("Saving changes for player {PlayerId} from world {WorldId}",
            profile.Player.Id, worldId);
        await context.SaveChangesAsync();
        logger.LogInformation("Successfully upserted player {PlayerId} from world {WorldId}",
            profile.Player.Id, worldId);
    }

    private async Task<Alliance> UpsertAlliance(HohAlliance alliance, string worldId, DateTime now)
    {
        logger.LogDebug("Upserting alliance {AllianceId} from world {WorldId}",
            alliance.Id, worldId);
        var today = now.ToDateOnly();
        var existingAlliance = await context.Alliances
            .Include(p => p.NameHistory)
            .FirstOrDefaultAsync(x => x.InGameAllianceId == alliance.Id && x.WorldId == worldId);

        Alliance modifiedAlliance;
        if (existingAlliance != null)
        {
            logger.LogDebug("Updating existing alliance {AllianceId} with name {AllianceName} from world {WorldId}",
                alliance.Id, alliance.Name, worldId);
            existingAlliance.Name = alliance.Name;
            existingAlliance.UpdatedAt = today;

            modifiedAlliance = existingAlliance;
        }
        else
        {
            logger.LogInformation("Creating new alliance {AllianceId} from world {WorldId}", alliance.Id, worldId);
            var newAlliance = new Alliance
            {
                WorldId = worldId,
                InGameAllianceId = alliance.Id,
                Name = alliance.Name,
                UpdatedAt = today,
            };

            modifiedAlliance = newAlliance;
            context.Alliances.Add(newAlliance);
        }

        modifiedAlliance.AvatarIconId = alliance.AvatarIconId;
        modifiedAlliance.AvatarBackgroundId = alliance.AvatarBackgroundId;

        if (modifiedAlliance.NameHistory.OrderByDescending(x => x.ChangedAt).FirstOrDefault()?.Name != alliance.Name)
        {
            logger.LogDebug("Adding name {AllianceName} to history for alliance {AllianceId}", alliance.Name,
                alliance.Id);
            modifiedAlliance.NameHistory.Add(new AllianceNameHistoryEntry {Name = alliance.Name, ChangedAt = now});
        }

        logger.LogDebug("Saving alliance {AllianceId} from world {WorldId}", alliance.Id, worldId);
        await context.SaveChangesAsync();
        logger.LogInformation("Successfully upserted alliance {AllianceId} from world {WorldId}", alliance.Id, worldId);

        return modifiedAlliance;
    }
}
