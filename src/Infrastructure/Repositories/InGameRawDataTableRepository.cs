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
        logger.LogDebug("Retrieving saved in-game raw data for partition key: {PartitionKey}", partitionKey);
        var result = new List<InGameRawData>();
        await foreach (var entity in tableStorageRepository.GetAllAsync(partitionKey))
        {
            result.Add(mapper.Map<InGameRawData>(entity));
        }

        logger.LogDebug("Successfully mapped {Count} entities", result.Count);
        return result;
    }

    public async Task<InGameRawData?> GetAsync(string partitionKey, string rowKey)
    {
        var data = await tableStorageRepository.GetAsync(partitionKey, rowKey);
        return data != null ? mapper.Map<InGameRawData>(data) : null;
    }

    public async Task<string> SaveAsync(InGameRawData data, string partitionKey)
    {
        var rowKey = CreateRowKey();
        await SaveAsync(data, partitionKey, rowKey);
        return rowKey;
    }

    public async Task SaveAsync(InGameRawData data, string partitionKey, string rowKey)
    {
        logger.LogDebug("Starting to save in-game raw data: {pk}, {rk}", partitionKey, rowKey);

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

    public async Task DeleteAllAsync(DateTime cutOffDate)
    {
        var entities = tableStorageRepository.GetAllAsync(x => x.CollectedAt < cutOffDate, 1000,
            ["PartitionKey, RowKey"]);
        await foreach (var entity in entities)
        {
            await tableStorageRepository.DeleteAsync(entity.PartitionKey, entity.RowKey);
        }
    }

    /// <summary>
    ///     Not efficient, but works for now.
    /// </summary>
    /// <param name="partitionKey"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <returns></returns>
    public async Task<IReadOnlyCollection<InGameRawData>> GetAsync(string partitionKey, int skip, int take)
    {
        var result = await tableStorageRepository.GetAsync(partitionKey, skip, take);
        return mapper.Map<IReadOnlyCollection<InGameRawData>>(result);
    }

    private static string CreateRowKey()
    {
        return Guid.NewGuid().ToString();
    }
}
