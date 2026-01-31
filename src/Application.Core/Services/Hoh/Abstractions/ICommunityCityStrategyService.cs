using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh;
using Refit;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface ICommunityCityStrategyService
{
    [Get(FogUrlBuilder.ApiRoutes.GET_COMMUNITY_CITY_STRATEGIES)]
    Task<IReadOnlyCollection<CommunityCityStrategyDto>> GetStrategiesAsync(CancellationToken ct = default);

    [Get(FogUrlBuilder.ApiRoutes.GET_COMMUNITY_CITY_GUIDES)]
    Task<IReadOnlyCollection<CommunityCityGuideInfoDto>> GetGuidesAsync(CancellationToken ct = default);

    [Get(FogUrlBuilder.ApiRoutes.GET_COMMUNITY_CITY_GUIDE_TEMPLATE_REFIT)]
    Task<CommunityCityGuideDto?> GetGuideAsync(int id, CancellationToken ct = default);
}
