namespace Ingweland.Fog.Infrastructure.Repositories.Abstractions;

public interface ITableStorageRepository<T>
{
    Task<T?> GetAsync(string partitionKey, string rowKey);
    Task<IEnumerable<T>> GetAllAsync(string partitionKey);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(string partitionKey, string rowKey);
}
