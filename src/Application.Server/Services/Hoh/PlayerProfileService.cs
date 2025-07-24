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
        try
        {
            var gw = gameWorldsProvider.GetGameWorlds().FirstOrDefault(x => x.Id == playerKey.WorldId);
            if (gw == null)
            {
                logger.LogWarning("Could not find game world with id {WorldId}", playerKey.WorldId);
                return null;
            }

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
        await _upsertSemaphore.WaitAsync();
        try
        {
            await DoUpsertPlayer(profile, worldId);
        }
        finally
        {
            _upsertSemaphore.Release();
        }
    }

    private async Task DoUpsertPlayer(PlayerProfile profile, string worldId)
    {
        Alliance existingAlliance = null!;
        if (profile.Alliance != null)
        {
            existingAlliance = await UpsertAlliance(profile.Alliance, worldId);
        }

        var now = DateTime.UtcNow;
        var today = now.ToDateOnly();
        var existingPlayer = await context.Players
            .Include(p =>
                p.Rankings.Where(pr => pr.Type == PlayerRankingType.PowerPoints && pr.CollectedAt == today))
            .Include(p => p.NameHistory)
            .Include(p => p.AgeHistory)
            .Include(p => p.AllianceHistory)
            .Include(p => p.AllianceNameHistory)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.InGamePlayerId == profile.Player.Id && x.WorldId == worldId);
        Player modifiedPlayer;
        if (existingPlayer != null)
        {
            existingPlayer.Name = profile.Player.Name;
            existingPlayer.Age = profile.Player.Age;
            existingPlayer.UpdatedAt = today;

            modifiedPlayer = existingPlayer;
        }
        else
        {
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
            modifiedPlayer.CurrentAlliance = existingAlliance;
            if (modifiedPlayer.AllianceHistory.All(a => a.Id != existingAlliance.Id))
            {
                modifiedPlayer.AllianceHistory.Add(existingAlliance);
            }

            modifiedPlayer.AllianceName = existingAlliance.Name;
        }
        else
        {
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
            modifiedPlayer.NameHistory.Add(new PlayerNameHistoryEntry {Name = profile.Player.Name});
        }

        if (modifiedPlayer.AgeHistory.OrderByDescending(x => x.ChangedAt).FirstOrDefault()?.Age != profile.Player.Age)
        {
            modifiedPlayer.AgeHistory.Add(new PlayerAgeHistoryEntry {Age = profile.Player.Age, ChangedAt = now});
        }

        var existingRanking =
            modifiedPlayer.Rankings.FirstOrDefault(x =>
                x.Type == PlayerRankingType.PowerPoints && x.CollectedAt == today);
        if (existingRanking != null)
        {
            existingRanking.Points = profile.RankingPoints;
            existingRanking.Rank = profile.Rank;
            existingRanking.CollectedAt = today;
        }
        else
        {
            modifiedPlayer.Rankings.Add(new PlayerRanking
            {
                Points = profile.RankingPoints,
                Rank = profile.Rank,
                CollectedAt = today,
                Type = PlayerRankingType.PowerPoints,
            });
        }

        await context.SaveChangesAsync();
    }

    private async Task<Alliance> UpsertAlliance(HohAlliance alliance, string worldId)
    {
        var now = DateTime.UtcNow;
        var today = now.ToDateOnly();
        var existingAlliance = await context.Alliances
            .Include(p => p.NameHistory)
            .FirstOrDefaultAsync(x => x.InGameAllianceId == alliance.Id && x.WorldId == worldId);
        Alliance modifiedAlliance;
        if (existingAlliance != null)
        {
            existingAlliance.Name = alliance.Name;
            existingAlliance.UpdatedAt = today;

            modifiedAlliance = existingAlliance;
        }
        else
        {
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
            modifiedAlliance.NameHistory.Add(new AllianceNameHistoryEntry {Name = alliance.Name, ChangedAt = now});
        }

        await context.SaveChangesAsync();

        return modifiedAlliance;
    }
}
