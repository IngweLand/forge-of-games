using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;

namespace Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;

public interface IStaticDataService
{
    Task<string> GetGameDesignJsonAsync(GameWorldConfig world);
    Task<byte[]> GetGameDesignProtobufAsync(GameWorldConfig world);
    Task<string> GetLocalizationJsonAsync(GameWorldConfig world, string locale);
    Task<byte[]> GetLocalizationProtobufAsync(GameWorldConfig world, string locale);
    Task<StartupDto> GetStartupDataAsync(GameWorldConfig world);
    Task<byte[]> GetStartupRawDataAsync(GameWorldConfig world);
    Task<string> GetStartupJsonAsync(GameWorldConfig world);
}
