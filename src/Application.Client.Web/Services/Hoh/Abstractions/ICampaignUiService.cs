using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;

public interface ICampaignUiService
{
    Task<RegionViewModel?> GetRegionAsync(string id);
    Task<IReadOnlyCollection<ContinentBasicViewModel>> GetCampaignContinentsBasicDataAsync();
    Task<IReadOnlyCollection<RegionBasicViewModel>> GetHistoricBattlesBasicDataAsync();
    Task<IReadOnlyCollection<RegionBasicViewModel>> GetTeslaStormRegionsBasicDataAsync();
}
