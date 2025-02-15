namespace Ingweland.Fog.Infrastructure.Repositories.Abstractions;

public interface ITableStorageRepository<T>
{
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task DeleteAsync(string partitionKey, string rowKey);
    Task<IEnumerable<T>> GetAllAsync(string partitionKey);
    Task<T?> GetAsync(string partitionKey, string rowKey);
    Task UpdateAsync(T entity);
    Task UpsertRangeAsync(IEnumerable<T> entities);
}
