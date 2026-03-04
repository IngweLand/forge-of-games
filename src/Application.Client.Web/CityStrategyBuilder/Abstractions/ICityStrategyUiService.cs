using Ingweland.Fog.Application.Client.Web.ViewModels;
using Ingweland.Fog.Application.Core.CityPlanner;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CityStrategyBuilder.Abstractions;

public interface ICityStrategyUiService
{
    CityStrategy CreateCityStrategy(NewCityRequest newCityRequest);
    Task<CityStrategy?> FetchPlayerStrategy(int strategyId, CancellationToken ct = default);
    Task<CityStrategy?> GetCommunityStrategyAsync(string strategyId);
    Task<IReadOnlyCollection<CommunityCityStrategyViewModel>> GetCommunityStrategiesAsync();
    Task<IReadOnlyCollection<CommunityCityGuideViewModel>> GetCommunityGuidesAsync();
    Task<CommunityCityGuideViewModel?> GetCommunityGuideAsync(int guideId);
}
