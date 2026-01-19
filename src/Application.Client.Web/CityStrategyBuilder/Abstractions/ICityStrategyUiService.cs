using Ingweland.Fog.Application.Core.CityPlanner;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CityStrategyBuilder.Abstractions;

public interface ICityStrategyUiService
{
    CityStrategy CreateCityStrategy(NewCityRequest newCityRequest);
    Task<CityStrategy?> FetchStrategy(int strategyId, CancellationToken ct = default);
}
