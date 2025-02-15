using Azure;
using Azure.Data.Tables;
using Ingweland.Fog.Infrastructure.Entities;
using Ingweland.Fog.Infrastructure.Repositories.Abstractions;

namespace Ingweland.Fog.Infrastructure.Repositories;

public class TableStorageRepository<T>(string connectionString, string tableName) : ITableStorageRepository<T>
    where T : TableEntityBase
{
    private readonly Lazy<TableClient> _tableClient = new(() => new TableClient(connectionString, tableName));

    public async Task<T?> GetAsync(string partitionKey, string rowKey)
    {
        try
        {
            return await _tableClient.Value.GetEntityAsync<T>(partitionKey, rowKey);
        }
        catch (RequestFailedException ex)
        {
            return null;
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync(string partitionKey)
    {
        var results = new List<T>();
        var queryResults = _tableClient.Value.QueryAsync<T>(x => x.PartitionKey == partitionKey);

        await foreach (var entity in queryResults)
        {
            results.Add(entity);
        }

        return results;
    }

    public async Task AddAsync(T entity)
    {
        await _tableClient.Value.AddEntityAsync(entity);
    }

    public Task AddRangeAsync(IEnumerable<T> entities)
    {
        var transactions =
            entities.Select(src => new TableTransactionAction(TableTransactionActionType.Add, src));
        return _tableClient.Value.SubmitTransactionAsync(transactions);
    }

    public Task UpsertRangeAsync(IEnumerable<T> entities)
    {
        var transactions =
            entities.Select(src => new TableTransactionAction(TableTransactionActionType.UpsertReplace, src));
        return _tableClient.Value.SubmitTransactionAsync(transactions);
    }

    public async Task UpdateAsync(T entity)
    {
        await _tableClient.Value.UpdateEntityAsync(entity, entity.ETag);
    }

    public async Task DeleteAsync(string partitionKey, string rowKey)
    {
        await _tableClient.Value.DeleteEntityAsync(partitionKey, rowKey);
    }
}
