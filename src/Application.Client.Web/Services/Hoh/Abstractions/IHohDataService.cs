using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Fog.Entities;
using Refit;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;

public interface IHohDataService
{
    [Get(FogUrlBuilder.ApiRoutes.HOH_CORE_DATA)]
    Task<VersionedResponse<byte[]?>> GetHohCoreDataAsync();

    [Get(FogUrlBuilder.ApiRoutes.HOH_LOCALIZATION_DATA)]
    Task<VersionedResponse<byte[]?>> GetHohLocalizationDataAsync([Query] string localeCode);

    [Get(FogUrlBuilder.ApiRoutes.HOH_CORE_DATE_VERSION)]
    Task<VersionDto> GetHohCoreDataVersionAsync();
}
