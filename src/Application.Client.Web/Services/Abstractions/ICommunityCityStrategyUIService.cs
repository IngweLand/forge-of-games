using Ingweland.Fog.Application.Client.Web.ViewModels;

namespace Ingweland.Fog.Application.Client.Web.Services.Abstractions;

public interface ICommunityCityStrategyUIService
{
    Task<IReadOnlyCollection<CommunityCityStrategyViewModel>> GetStrategiesAsync();
    Task<IReadOnlyCollection<CommunityCityGuideViewModel>> GetGuidesAsync();
    Task<CommunityCityGuideViewModel?> GetGuideAsync(int guideId);
}
