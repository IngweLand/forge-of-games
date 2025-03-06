using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.Interfaces.Hoh;

public interface IInGameRawDataTableRepository
{
    Task<IReadOnlyCollection<InGameRawData>> GetAllAsync(string partitionKey);

    Task SaveAsync(InGameRawData data, string partitionKey);
}