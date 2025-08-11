using FluentResults;
using Ingweland.Fog.Application.Server.Errors;
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

    public async Task UpdateStatusAsync(IEnumerable<int> playerIds, InGameEntityStatus status,
        CancellationToken cancellationToken)
    {
        var uniqueIds = playerIds.ToHashSet();
        var players = await context.Players.Where(x => uniqueIds.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var player in players)
        {
            player.Status = status;
            if (status == InGameEntityStatus.Missing)
            {
                player.AllianceMembership = null;
            }
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateStatusAsync(int playerId, InGameEntityStatus status, CancellationToken cancellationToken)
    {
        var player = await context.Players.FindAsync(playerId, cancellationToken);
        if (player == null)
        {
            logger.LogWarning("Player with id {PlayerId} not found", playerId);
            return;
        }

        player.Status = status;
        if (status == InGameEntityStatus.Missing)
        {
            player.AllianceMembership = null;
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpsertPlayerAsync(PlayerProfile profile, string worldId)
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

    public async Task<Result<Player>> UpsertPlayerAsync(string worldId, HohPlayer player, int rankingPoints,
        DateTime lastOnline)
    {
        await _upsertSemaphore.WaitAsync();
        var now = DateTime.UtcNow;
        try
        {
            var modifiedPlayer = await AddOrUpdatePlayerAsync(worldId, player, null, rankingPoints, now);
            modifiedPlayer.LastSeenOnline = lastOnline;

            await context.SaveChangesAsync();

            return Result.Ok(modifiedPlayer);
        }
        catch (Exception ex)
        {
            return Result.Fail(new PlayerUpsertionError(worldId, player.Id, ex));
        }
        finally
        {
            _upsertSemaphore.Release();
        }
    }

    public async Task<Result<IReadOnlyCollection<Player>>> UpsertPlayersAsync(string worldId,
        IReadOnlyCollection<AllianceMember> allianceMembers)
    {
        var upsertedPlayers = new List<Player>();
        foreach (var member in allianceMembers)
        {
            var playerResult =
                await UpsertPlayerAsync(worldId, member.Player, member.RankingPoints, member.LastSeenOnline);
            if (playerResult.IsFailed)
            {
                return playerResult.ToResult();
            }

            upsertedPlayers.Add(playerResult.Value);
        }

        return Result.Ok<IReadOnlyCollection<Player>>(upsertedPlayers);
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

    private async Task<Player> AddOrUpdatePlayerAsync(string worldId, HohPlayer player, int? rank, int rankingPoints,
        DateTime now)
    {
        var today = now.ToDateOnly();

        logger.LogDebug("Querying existing player {PlayerId} from world {WorldId}",
            player.Id, worldId);
        var existingPlayer = await context.Players
            .Include(p =>
                p.Rankings.Where(pr => pr.Type == PlayerRankingType.PowerPoints && pr.CollectedAt == today))
            .Include(p => p.NameHistory)
            .Include(p => p.AgeHistory)
            .Include(p => p.AllianceHistory)
            .Include(p => p.Squads)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.InGamePlayerId == player.Id && x.WorldId == worldId);

        Player modifiedPlayer;
        if (existingPlayer != null)
        {
            logger.LogDebug("Updating existing player {PlayerId}", existingPlayer.Id);
            existingPlayer.Name = player.Name;
            existingPlayer.Age = player.Age;
            existingPlayer.Status = InGameEntityStatus.Active;
            modifiedPlayer = existingPlayer;
        }
        else
        {
            logger.LogInformation("Creating new player {PlayerId} from world {WorldId}", player.Id, worldId);
            var newPlayer = new Player
            {
                WorldId = worldId,
                InGamePlayerId = player.Id,
                Name = player.Name,
                Age = player.Age,
                Status = InGameEntityStatus.Active,
            };

            modifiedPlayer = newPlayer;
            context.Players.Add(newPlayer);
        }

        modifiedPlayer.UpdatedAt = today;
        modifiedPlayer.AvatarId = player.AvatarId;
        if (rank.HasValue)
        {
            modifiedPlayer.Rank = rank;
        }

        modifiedPlayer.RankingPoints = rankingPoints;

        if (modifiedPlayer.NameHistory.All(x => x.Name != player.Name))
        {
            logger.LogDebug("Adding name {PlayerName} to history for player {PlayerId}",
                player.Name, player.Id);
            modifiedPlayer.NameHistory.Add(new PlayerNameHistoryEntry {Name = player.Name});
        }

        if (modifiedPlayer.AgeHistory.OrderByDescending(x => x.ChangedAt).FirstOrDefault()?.Age != player.Age)
        {
            logger.LogDebug("Adding age {PlayerAge} to history for player {PlayerId}",
                player.Age, player.Id);
            modifiedPlayer.AgeHistory.Add(new PlayerAgeHistoryEntry {Age = player.Age, ChangedAt = now});
        }

        var existingRanking =
            modifiedPlayer.Rankings.FirstOrDefault(x =>
                x.Type == PlayerRankingType.PowerPoints && x.CollectedAt == today);
        if (existingRanking != null)
        {
            logger.LogDebug("Updating existing ranking for player {PlayerId}: Rank {Rank}, Points {Points}",
                player.Id, rank, rankingPoints);
            existingRanking.Points = rankingPoints;
            existingRanking.Rank = rank ?? 0;
            existingRanking.CollectedAt = today;
        }
        else
        {
            logger.LogDebug("Adding new ranking for player {PlayerId}: Rank {Rank}, Points {Points}",
                player.Id, rank, rankingPoints);
            modifiedPlayer.Rankings.Add(new PlayerRanking
            {
                Points = rankingPoints,
                Rank = rank ?? 0,
                CollectedAt = today,
                Type = PlayerRankingType.PowerPoints,
            });
        }

        return modifiedPlayer;
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

        var modifiedPlayer =
            await AddOrUpdatePlayerAsync(worldId, profile.Player, profile.Rank, profile.RankingPoints, now);
        modifiedPlayer.ProfileUpdatedAt = today;

        if (alliance != null)
        {
            logger.LogDebug("Setting alliance {AllianceId} for player {PlayerId}", alliance.Id, profile.Player.Id);
            modifiedPlayer.AllianceMembership = new AllianceMemberEntity
            {
                Alliance = alliance,
            };
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
            modifiedPlayer.AllianceMembership = null;
        }

        modifiedPlayer.TreasureHuntDifficulty = profile.TreasureHuntDifficulty;
        modifiedPlayer.PvpTier = profile.PvpTier;

        UpsertSquads(modifiedPlayer, profile.Squads, today);

        logger.LogDebug("Saving changes for player {PlayerId} from world {WorldId}",
            profile.Player.Id, worldId);
    }

    private async Task<Alliance> UpsertAlliance(HohAlliance hohAlliance, string worldId, DateTime now)
    {
        logger.LogDebug("Upserting alliance {AllianceId} from world {WorldId}",
            hohAlliance.Id, worldId);
        var existingAlliance = await context.Alliances
            .Include(p => p.NameHistory)
            .FirstOrDefaultAsync(x => x.InGameAllianceId == hohAlliance.Id && x.WorldId == worldId);

        Alliance alliance;
        if (existingAlliance != null)
        {
            logger.LogDebug("Updating existing alliance {AllianceId} with name {AllianceName} from world {WorldId}",
                hohAlliance.Id, hohAlliance.Name, worldId);
            existingAlliance.Name = hohAlliance.Name;

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
