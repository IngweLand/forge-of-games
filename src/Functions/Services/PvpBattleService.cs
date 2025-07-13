using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PvpBattle = Ingweland.Fog.Models.Hoh.Entities.Battle.PvpBattle;

namespace Ingweland.Fog.Functions.Services;

public interface IPvpBattleService
{
    Task AddAsync(IEnumerable<(string WorldId, PvpBattle PvpBattle)> battles);
}

public class PvpBattleService(IFogDbContext context, IMapper mapper, ILogger<PvpBattleService> logger)
    : IPvpBattleService
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters = {new JsonStringEnumConverter()},
    };

    public async Task AddAsync(IEnumerable<(string WorldId, PvpBattle PvpBattle)> battles)
    {
        var unique = battles
            .DistinctBy(t => new BattleKey(t.WorldId, t.PvpBattle.Id))
            .ToDictionary(t => new BattleKey(t.WorldId, t.PvpBattle.Id), t => t.PvpBattle);
        logger.LogInformation("{ValidCount} unique pvp battles after filtering", unique.Count);
        var existingBattleKeys =
            await GetExistingBattlesAsync(unique.Keys.Select(bk => bk.InGameBattleId).ToHashSet());
        logger.LogInformation("Retrieved {ExistingBattlesCount} existing pvp battles", existingBattleKeys.Count);
        var newBattleKeys = unique.Keys.ToHashSet().Except(existingBattleKeys).ToList();
        logger.LogInformation("{NewBattlesCount} new pvp battles identified", newBattleKeys.Count);
        var newBattlePlayerKeys = newBattleKeys.SelectMany(k =>
        {
            var battle = unique[k];
            return new List<PlayerKey>
            {
                new(k.WorldId, battle.Winner.Id),
                new(k.WorldId, battle.Loser.Id),
            };
        });
        var existingPlayers =
            await GetExistingPlayersAsync(newBattlePlayerKeys.Select(pk => pk.InGamePlayerId).ToHashSet());
        logger.LogInformation("Found {ExistingCount} existing players.", existingPlayers.Count);
        var newBattles = newBattleKeys.Select(k =>
            {
                var battle = unique[k];
                if (!existingPlayers.TryGetValue(new PlayerKey(k.WorldId, battle.Winner.Id), out var winnerId))
                {
                    return null;
                }

                if (!existingPlayers.TryGetValue(new PlayerKey(k.WorldId, battle.Loser.Id), out var loserId))
                {
                    return null;
                }

                return new Models.Fog.Entities.PvpBattle
                {
                    WorldId = k.WorldId,
                    InGameBattleId = battle.Id,
                    PerformedAt = battle.PerformedAt,
                    WinnerUnits = JsonSerializer.Serialize(battle.WinnerUnits, JsonSerializerOptions),
                    LoserUnits = JsonSerializer.Serialize(battle.LoserUnits, JsonSerializerOptions),
                    WinnerId = winnerId,
                    LoserId = loserId,
                };
            })
            .Where(b => b != null)
            .ToList();
        context.PvpBattles.AddRange(newBattles!);
        await context.SaveChangesAsync();
        logger.LogInformation("SaveChangesAsync completed, added {AddedBattlesCount} pvp battles", newBattles.Count);
    }

    private async Task<HashSet<BattleKey>> GetExistingBattlesAsync(HashSet<byte[]> inGameBattleIds)
    {
        var existing = await context.PvpBattles
            .Where(p => inGameBattleIds.Contains(p.InGameBattleId))
            .ProjectTo<BattleKey>(mapper.ConfigurationProvider)
            .ToHashSetAsync();
        logger.LogInformation("GetExistingBattlesAsync found {Count} battles", existing.Count);
        return existing;
    }

    private async Task<Dictionary<PlayerKey, int>> GetExistingPlayersAsync(HashSet<int> inGamePlayerIds)
    {
        logger.LogInformation("Querying existing players for {IdCount} in-game player IDs.", inGamePlayerIds.Count);
        var existing = await context.Players
            .Where(p => inGamePlayerIds.Contains(p.InGamePlayerId))
            .Select(p => new {p.Id, p.WorldId, p.InGamePlayerId})
            .ToDictionaryAsync(src => new PlayerKey(src.WorldId, src.InGamePlayerId), src => src.Id);
        logger.LogInformation("Query returned {ExistingCount} existing players.", existing.Count);
        return existing;
    }
}
