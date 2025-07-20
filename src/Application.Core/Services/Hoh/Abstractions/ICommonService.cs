using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh;
using Refit;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface ICommonService
{
    [Get(FogUrlBuilder.ApiRoutes.COMMON_AGES)]
    Task<IReadOnlyCollection<AgeDto>> GetAgesAsync();

    [Get(FogUrlBuilder.ApiRoutes.COMMON_RESOURCES)]
    Task<IReadOnlyCollection<ResourceDto>> GetResourcesAsync();
}
