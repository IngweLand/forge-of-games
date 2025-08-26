using System.Linq.Expressions;
using Azure;

namespace Ingweland.Fog.Infrastructure.Repositories.Abstractions;

public interface ITableStorageRepository<T> where T : notnull
{
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task DeleteAsync(string partitionKey, string rowKey);
    Task<IEnumerable<T>> GetAllAsync(string partitionKey);
    Task<T?> GetAsync(string partitionKey, string rowKey);
    Task UpdateAsync(T entity);
    Task UpsertRangeAsync(IEnumerable<T> entities);
    Task UpsertEntityAsync(T entity);

    AsyncPageable<T> GetAllAsync(Expression<Func<T, bool>> filter,
        int? maxPerPage = null,
        IEnumerable<string>? select = null,
        CancellationToken cancellationToken = default);
}
