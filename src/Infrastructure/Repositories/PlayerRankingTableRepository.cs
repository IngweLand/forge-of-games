using AutoMapper;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Infrastructure.Entities;
using Ingweland.Fog.Infrastructure.Repositories.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Constants;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Infrastructure.Repositories;

public class PlayerRankingTableRepository(
    ITableStorageRepository<PlayerRankingTableEntity> tableStorageRepository,
    IMapper mapper,
    ILogger<PlayerRankingTableRepository> logger) : IPlayerRankingTableRepository
{
    public async Task SaveAsync(IEnumerable<PlayerRank> rankings, string worldId, PlayerRankingType rankingType,
        DateOnly date)
    {
        logger.LogInformation("Starting to save player rankings: {@Summary}", new {worldId, rankingType});

        var pk = CreatePartitionKey(worldId, date, rankingType);
        var rankingEntities = mapper.Map<List<PlayerRankingTableEntity>>(rankings,
            opt => { opt.Items[ResolutionContextKeys.PLAYER_RANKING_TYPE] = rankingType; });
        foreach (var entity in rankingEntities)
        {
            entity.PartitionKey = pk;
            entity.RowKey = CreateRowKey(worldId, entity.Id);
        }

        try
        {
            await tableStorageRepository.UpsertRangeAsync(rankingEntities);
            logger.LogInformation("Successfully saved player rankings. Total: {RankingsCount}", rankingEntities.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save player rankings");
            throw;
        }
    }

    public async Task<IReadOnlyCollection<PlayerRank>> GetAllAsync(string worldId, PlayerRankingType rankingType,
        DateOnly date)
    {
        var pk = CreatePartitionKey(worldId, date, rankingType);
        var entities = await tableStorageRepository.GetAllAsync(pk);
        return mapper.Map<IReadOnlyCollection<PlayerRank>>(entities);
    }

    private string CreatePartitionKey(string worldId, DateOnly date, PlayerRankingType rankingType)
    {
        return $"{worldId}_{date.ToString("O")}_{rankingType}";
    }

    private string CreateRowKey(string worldId, int inGamePlayerId)
    {
        return $"{worldId}_{inGamePlayerId}";
    }
}
