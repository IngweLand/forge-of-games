using AutoMapper;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Functions.Data;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Shared.Constants;
using Ingweland.Fog.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services;

public interface IPlayerSquadsUpdater
{
    Task AddOrUpdateAsync(IEnumerable<PlayerAggregate> playerAggregates);
}

public class PlayerSquadsUpdater(IFogDbContext context, IMapper mapper, ILogger<PlayerRankingService> logger) : IPlayerSquadsUpdater
{
    public async Task AddOrUpdateAsync(IEnumerable<PlayerAggregate> playerAggregates)
    {
        var unique = playerAggregates
            .Where(p => p.CanUpdateHeroes())
            .OrderByDescending(p => p.CollectedAt) // we need this to pick the latest one
            .DistinctBy(p => p.Key)
            .ToList();
        if (unique.Count == 0)
        {
            logger.LogInformation("No valid player aggregates to process.");
            return;
        }
        logger.LogDebug("Filtered aggregates to {UniqueCount} unique items.", unique.Count);
        var existingPlayerKeys = await GetExistingPlayersAsync(unique.Select(pk => pk.InGamePlayerId).ToHashSet());
        logger.LogDebug("Found {ExistingCount} existing players.", existingPlayerKeys.Count);
        
        int updatedPlayersCount = 0;
        int addedSquadsCount = 0;
        int updatedSquadsCount = 0;
        
        foreach (var playerAggregate in unique)
        {
            if (!existingPlayerKeys.TryGetValue(playerAggregate.Key, out var existingPlayer))
            {
                logger.LogWarning("Player with key {@PlayerKey} not found in existing players", playerAggregate.Key);
                continue;
            }

            var newSquads = mapper.Map<List<ProfileSquadEntity>>(playerAggregate.ProfileSquads, opt =>
            {
                opt.Items[ResolutionContextKeys.DATE] = playerAggregate.CollectedAt.ToDateOnly();
                opt.Items[ResolutionContextKeys.AGE] = playerAggregate.Age!;
            });
            
            logger.LogDebug("Processing squads for player {@PlayerKey}", playerAggregate.Key);
            
            foreach (var squad in newSquads)
            {
                var existingSquad = existingPlayer.Squads.FirstOrDefault(x => x.Key == squad.Key);
                if (existingSquad == null)
                {
                    existingPlayer.Squads.Add(squad);
                    addedSquadsCount++;
                }
                else
                {
                    squad.Level = squad.Level;
                    squad.AscensionLevel = squad.AscensionLevel;
                    squad.AbilityLevel = squad.AbilityLevel;
                    squad.Hero = squad.Hero;
                    squad.SupportUnit = squad.SupportUnit;
                    squad.Age = playerAggregate.Age!;
                    updatedSquadsCount++;
                }
            }
            
            updatedPlayersCount++;
        }
        
        logger.LogInformation(
            "Completed processing heroes: {UpdatedPlayers} players updated, {AddedSquads} squads added, {UpdatedSquads} squads updated",
            updatedPlayersCount, addedSquadsCount, updatedSquadsCount);
        
        await context.SaveChangesAsync();
        logger.LogInformation("Changes saved to database");
    }
    
    private async Task<Dictionary<PlayerKey, Player>> GetExistingPlayersAsync(HashSet<int> inGamePlayerIds)
    {
        logger.LogDebug("Querying existing players for {IdCount} in-game player IDs.", inGamePlayerIds.Count);
        var existing = await context.Players
            .Include(x => x.Squads)
            .Where(p => inGamePlayerIds.Contains(p.InGamePlayerId))
            .ToListAsync();
        logger.LogDebug("Query returned {ExistingCount} existing players.", existing.Count);
        return existing.ToDictionary(x => x.Key);
    }
}