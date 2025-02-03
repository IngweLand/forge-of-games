using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;

public interface IInGameStartupDataProcessingService
{
    Task<InGameStartupData> ParseStartupData(string inputData);
}
