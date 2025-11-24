using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;

public interface ICampaignUiService
{
    Task<RegionViewModel?> GetRegionAsync(string id);
    Task<IReadOnlyCollection<ContinentBasicViewModel>> GetCampaignContinentsBasicDataAsync();
    Task<IReadOnlyCollection<RegionBasicViewModel>> GetHistoricBattlesBasicDataAsync();
    Task<IReadOnlyCollection<RegionBasicViewModel>> GetTeslaStormRegionsBasicDataAsync();
    Task<HeroProfileViewModel> CreateHeroProfileAsync(IBattleUnitProperties hero);
}
