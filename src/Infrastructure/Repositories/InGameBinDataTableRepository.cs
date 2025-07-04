using AutoMapper;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Infrastructure.Entities;
using Ingweland.Fog.Infrastructure.Repositories.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Infrastructure.Repositories;

public class InGameBinDataTableRepository(
    ITableStorageRepository<InGameBinDataTableEntity> tableStorageRepository,
    IMapper mapper,
    ILogger<InGameBinDataTableRepository> logger) : IInGameBinDataTableRepository
{
    private static readonly string MinDate = new DateOnly(2000, 1, 1).ToString("O");
    private static readonly string MaxDate = new DateOnly(3000, 1, 1).ToString("O");

    public async Task<IReadOnlyCollection<InGameBinData>> GetAllAsync(string dataType, string gameWorldId, int playerId)
    {
        var entities = await GetAllTableEntities(dataType, gameWorldId, playerId);
        return mapper.Map<IReadOnlyCollection<InGameBinData>>(entities);
    }

    public async Task<InGameBinData?> GetAsync(string dataType, string gameWorldId, int playerId, DateOnly collectedAt)
    {
        var data = await tableStorageRepository.GetAsync(dataType, $"{gameWorldId}_{playerId}_{collectedAt:O}");
        return data != null ? mapper.Map<InGameBinData>(data) : null;
    }

    public async Task<InGameBinData?> GetLatestAsync(string dataType, string gameWorldId, int playerId)
    {
        var entities = await GetAllTableEntities(dataType, gameWorldId, playerId);
        var last = entities.OrderByDescending(x => x.CollectedAt).FirstOrDefault();
        return last != null ? mapper.Map<InGameBinData>(last) : null;
    }

    public async Task SaveAsync(InGameBinData data)
    {
        logger.LogInformation("Starting to save in-game bin data: {@X}",
            new {DataType = data.DataKey, data.GameWorldId, data.PlayerId});

        var entity = mapper.Map<InGameBinDataTableEntity>(data);

        try
        {
            await tableStorageRepository.UpsertEntityAsync(entity);
            logger.LogInformation("Successfully saved in-game bin data: {@X}",
                new {DataType = data.DataKey, data.GameWorldId, data.PlayerId});
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save in-game bin data: {@X}",
                new {DataType = data.DataKey, data.GameWorldId, data.PlayerId});
            throw;
        }
    }

    private Task<IEnumerable<InGameBinDataTableEntity>> GetAllTableEntities(string dataType, string gameWorldId,
        int playerId)
    {
        var rowKeyPrefix = $"{gameWorldId}_{playerId}";

        return tableStorageRepository.GetAllAsync(x =>
            x.PartitionKey == dataType &&
            x.RowKey.CompareTo($"{rowKeyPrefix}_{MinDate}") >= 0 &&
            x.RowKey.CompareTo($"{rowKeyPrefix}_{MaxDate}") <= 0);
    }
}
