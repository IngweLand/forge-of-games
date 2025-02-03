using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.CommandCenter;
using Ingweland.Fog.Models.Fog.Entities;
using Refit;

namespace Ingweland.Fog.Application.Client.Web.Services.Abstractions;

public interface IInGameStartupDataService
{
    [Post("/inGameData")]
    Task<ResourceCreatedResponse> ImportInGameDataAsync([Body] ImportInGameStartupDataRequestDto importHeroesRequestDto);
    
    [Get("/inGameData/{inGameStartupDataId}")]
    Task<InGameStartupData?> GetImportedInGameDataAsync([AliasAs("inGameStartupDataId")] string inGameStartupDataId);
}
