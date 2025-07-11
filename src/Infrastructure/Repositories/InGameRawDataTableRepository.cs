using AutoMapper;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Infrastructure.Entities;
using Ingweland.Fog.Infrastructure.Repositories.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Infrastructure.Repositories;

public class InGameRawDataTableRepository(
    ITableStorageRepository<InGameRawDataTableEntity> tableStorageRepository,
    IMapper mapper,
    ILogger<InGameRawDataTableRepository> logger) : IInGameRawDataTableRepository
{
    public async Task<IReadOnlyCollection<InGameRawData>> GetAllAsync(string partitionKey)
    {
        var entities = await tableStorageRepository.GetAllAsync(partitionKey);
        return mapper.Map<IReadOnlyCollection<InGameRawData>>(entities);
    }

    public async Task<InGameRawData?> GetAsync(string partitionKey, string rowKey)
    {
        var data = await tableStorageRepository.GetAsync(partitionKey, rowKey);
        return data != null ? mapper.Map<InGameRawData>(data) : null;
    }

    public Task SaveAsync(InGameRawData data, string partitionKey)
    {
        return SaveAsync(data, partitionKey, CreateRowKey());
    }

    public async Task SaveAsync(InGameRawData data, string partitionKey, string rowKey)
    {
        logger.LogInformation("Starting to save in-game raw data: {pk}, {rk}", partitionKey, rowKey);

        var entity = mapper.Map<InGameRawDataTableEntity>(data);
        entity.PartitionKey = partitionKey;
        entity.RowKey = rowKey;

        try
        {
            await tableStorageRepository.UpsertEntityAsync(entity);
            logger.LogInformation("Successfully saved in-game raw data.  {pk}, {rk}", partitionKey, rowKey);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save in-game raw data.  {pk}, {rk}", partitionKey, rowKey);
            throw;
        }
    }

    private static string CreateRowKey()
    {
        return Guid.NewGuid().ToString();
    }
}
