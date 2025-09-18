using FluentResults;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.InnSdk.Hoh.Constants;
using Ingweland.Fog.InnSdk.Hoh.Factories.Interfaces;
using Ingweland.Fog.InnSdk.Hoh.Net.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;

namespace Ingweland.Fog.InnSdk.Hoh.Services;

public class StaticDataService(
    IGameApiClient apiClient,
    IGameDesignRequestPayloadFactory gameDesignRequestPayloadFactory,
    ILocalizationRequestPayloadFactory localizationRequestPayloadFactory,
    IDataParsingService dataParsingService) : IStaticDataService
{
    public Task<string> GetGameDesignJsonAsync(GameWorldConfig world)
    {
        return apiClient.SendForJsonAsync(world, GameEndpoints.GameDesignPath,
            gameDesignRequestPayloadFactory.Create());
    }

    public Task<byte[]> GetGameDesignProtobufAsync(GameWorldConfig world)
    {
        return apiClient.SendForProtobufAsync(world, GameEndpoints.GameDesignPath,
            gameDesignRequestPayloadFactory.Create());
    }

    public Task<string> GetLocalizationJsonAsync(GameWorldConfig world, string locale)
    {
        var payload = localizationRequestPayloadFactory.Create(locale);
        return apiClient.SendForJsonAsync(world, GameEndpoints.LocaPath, payload);
    }

    public Task<byte[]> GetLocalizationProtobufAsync(GameWorldConfig world, string locale)
    {
        var payload = localizationRequestPayloadFactory.Create(locale);
        return apiClient.SendForProtobufAsync(world, GameEndpoints.LocaPath, payload);
    }
    
    public Task<byte[]> GetStartupRawDataAsync(GameWorldConfig world)
    {
        return apiClient.SendForProtobufAsync(world, GameEndpoints.StartupPath, []);
    }

    public async Task<Result<CommunicationDto>> GetStartupDataAsync(GameWorldConfig world)
    {
        var data = await GetStartupRawDataAsync(world);
        return dataParsingService.ParseCommunicationDto(data);
    }
    
    public Task<string> GetStartupJsonAsync(GameWorldConfig world)
    {
        return apiClient.SendForJsonAsync(world, GameEndpoints.StartupPath,[]);
    }
}
