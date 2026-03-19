using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Models.Fog.Entities;
using Refit;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;

public interface IHohDataService
{
    [Get(FogUrlBuilder.ApiRoutes.HOH_DATA)]
    Task<VersionedResponse<byte[]?>> GetHohDataAsync([Query] string? version);

    [Get(FogUrlBuilder.ApiRoutes.HOH_LOCALIZATION_DATA)]
    Task<VersionedResponse<byte[]?>> GetHohLocalizationDataAsync([Query] string? version);
}
