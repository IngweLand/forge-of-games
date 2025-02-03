using Ingweland.Fog.Dtos.Hoh.CommandCenter;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.Interfaces.Hoh;

public interface IInGameStartupDataRepository
{
    Task<InGameStartupData?> GetAsync(string inGameStartupDataId);
    Task<string> SaveAsync(InGameStartupData data);
}
