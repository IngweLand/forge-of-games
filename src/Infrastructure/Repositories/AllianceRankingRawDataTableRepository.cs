using AutoMapper;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Infrastructure.Entities;
using Ingweland.Fog.Infrastructure.Repositories.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Infrastructure.Repositories;

public class AllianceRankingRawDataTableRepository(
    ITableStorageRepository<AllianceRankingRawDataTableEntity> tableStorageRepository,
    IMapper mapper,
    ILogger<AllianceRankingRawDataTableRepository> logger) : IAllianceRankingRawDataTableRepository
{
    public async Task SaveAsync(AllianceRankingRawData data, string worldId, AllianceRankingType rankingType)
    {
        logger.LogInformation("Starting to save alliance rankings raw data: {@Summary}", new {worldId, rankingType});

        var pk = CreatePartitionKey(worldId, DateOnly.FromDateTime(data.CollectedAt), rankingType);
        var entity = mapper.Map<AllianceRankingRawDataTableEntity>(data);
        entity.PartitionKey = pk;
        entity.RowKey = CreateRowKey();

        try
        {
            await tableStorageRepository.AddAsync(entity);
            logger.LogInformation("Successfully saved alliance rankings raw data.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save alliance rankings raw data.");
            throw;
        }
    }

    public async Task<IReadOnlyCollection<AllianceRankingRawData>> GetAllAsync(string worldId,
        AllianceRankingType rankingType, DateOnly date)
    {
        var pk = CreatePartitionKey(worldId, date, rankingType);
        var entities = await tableStorageRepository.GetAllAsync(pk);
        return mapper.Map<IReadOnlyCollection<AllianceRankingRawData>>(entities);
    }

    private string CreatePartitionKey(string worldId, DateOnly date, AllianceRankingType rankingType)
    {
        return $"{worldId}_{date.ToString("O")}_{rankingType}";
    }

    private string CreateRowKey()
    {
        return Guid.NewGuid().ToString();
    }
}
