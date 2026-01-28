using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh;
using Refit;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface ISharedCityStrategyService
{
    [Get(FogUrlBuilder.ApiRoutes.GET_SHARED_CITY_STRATEGIES)]
    Task<IReadOnlyCollection<CommunityCityStrategyDto>> GetAllAsync(CancellationToken ct = default);
}
