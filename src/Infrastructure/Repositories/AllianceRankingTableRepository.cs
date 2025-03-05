using AutoMapper;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Infrastructure.Entities;
using Ingweland.Fog.Infrastructure.Repositories.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Constants;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Infrastructure.Repositories;

public class AllianceRankingTableRepository(
    ITableStorageRepository<AllianceRankingTableEntity> tableStorageRepository,
    IMapper mapper,
    ILogger<AllianceRankingTableRepository> logger) : IAllianceRankingTableRepository
{
    public async Task SaveAsync(IEnumerable<AllianceRank> rankings, string worldId, AllianceRankingType rankingType,
        DateOnly date)
    {
        var rankingList = rankings as IList<AllianceRank> ?? rankings.ToList();
        if (rankingList.Count == 0)
        {
            logger.LogInformation("No alliance rankings to add. World: {world}", worldId);
            return;
        }
        
        logger.LogInformation("Starting to save alliance rankings: {@Summary}", new {worldId, rankingType});

        var pk = CreatePartitionKey(worldId, date, rankingType);
        var rankingEntities = mapper.Map<List<AllianceRankingTableEntity>>(rankingList,
            opt => { opt.Items[ResolutionContextKeys.ALLIANCE_RANKING_TYPE] = rankingType; });
        foreach (var entity in rankingEntities)
        {
            entity.PartitionKey = pk;
            entity.RowKey = CreateRowKey(worldId, entity.Id);
        }

        try
        {
            await tableStorageRepository.UpsertRangeAsync(rankingEntities);
            logger.LogInformation("Successfully saved alliance rankings. Total: {RankingsCount}", rankingEntities.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save alliance rankings");
            throw;
        }
    }

    public async Task<IReadOnlyCollection<AllianceRank>> GetAllAsync(string worldId, AllianceRankingType rankingType,
        DateOnly date)
    {
        var pk = CreatePartitionKey(worldId, date, rankingType);
        var entities = await tableStorageRepository.GetAllAsync(pk);
        return mapper.Map<IReadOnlyCollection<AllianceRank>>(entities);
    }

    private string CreatePartitionKey(string worldId, DateOnly date, AllianceRankingType rankingType)
    {
        return $"{worldId}_{date.ToString("O")}_{rankingType}";
    }

    private string CreateRowKey(string worldId, int inGameAllianceId)
    {
        return $"{worldId}_{inGameAllianceId}";
    }
}