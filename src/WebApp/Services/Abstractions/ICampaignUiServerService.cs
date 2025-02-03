using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

namespace Ingweland.Fog.WebApp.Services.Abstractions;

public interface ICampaignUiServerService
{
    Task<RegionViewModel?> GetRegionAsync(string id);
    Task<IReadOnlyCollection<ContinentBasicViewModel>> GetCampaignContinentsBasicDataAsync();
}
