using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.Interfaces.Hoh;

public interface IInGameBinDataTableRepository
{
    Task<IReadOnlyCollection<InGameBinData>> GetAllAsync(string dataType, string gameWorldId, int playerId);

    Task<InGameBinData?> GetAsync(string dataType, string gameWorldId, int playerId, DateOnly collectedAt);
    Task<InGameBinData?> GetLatestAsync(string dataType, string gameWorldId, int playerId);
    Task SaveAsync(InGameBinData data);
}
