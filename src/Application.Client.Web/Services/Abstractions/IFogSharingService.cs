using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh;
using Refit;

namespace Ingweland.Fog.Application.Client.Web.Services.Abstractions;

public interface IFogSharingService
{
    [Post(FogUrlBuilder.ApiRoutes.CREATE_SHARE)]
    Task<CreatedShareDto> ShareAsync(SharedDataDto data);

    [Get(FogUrlBuilder.ApiRoutes.GET_SHARED_RESOURCE_TEMPLATE)]
    Task<SharedDataDto?> GetAsync(string shareId);
}
