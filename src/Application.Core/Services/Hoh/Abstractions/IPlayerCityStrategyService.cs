using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Models.Fog.Entities;
using Refit;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface IPlayerCityStrategyService
{
    [Get(FogUrlBuilder.ApiRoutes.PLAYER_CITY_STRATEGY_REFIT)]
    Task<CityStrategy> GetPlayerCityStrategyAsync(int strategyId, CancellationToken ct = default);
}
