using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.Interfaces.Hoh;

public interface IInGameRawDataTableRepository
{
    Task<IReadOnlyCollection<InGameRawData>> GetAllAsync(string partitionKey);
    Task<InGameRawData?> GetAsync(string partitionKey, string rowKey);

    Task<string> SaveAsync(InGameRawData data, string partitionKey);
    Task SaveAsync(InGameRawData data, string partitionKey, string rowKey);
    Task DeleteAllAsync(DateTime cutOffDate);
    Task<IReadOnlyCollection<InGameRawData>> GetAsync(string partitionKey, int skip, int take);
}
