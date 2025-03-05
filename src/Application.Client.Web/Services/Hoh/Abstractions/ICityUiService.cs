using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;

public interface ICityUiService
{
    Task<IReadOnlyCollection<BuildingTypeViewModel>> GetBuildingCategoriesAsync(CityId cityId);
    Task<BuildingGroupViewModel?> GetBuildingGroupAsync(CityId cityId, BuildingGroup group);
    Task<IReadOnlyCollection<CityBuildingGroupsViewModel>> GetCityBuildingGroupsAsync();
    Task<IReadOnlyCollection<WonderGroupViewModel>> GetWonderGroupsAsync();
    Task<WonderViewModel?> GetWonderAsync(WonderId id);
    Task<CityBuildingGroupsViewModel> GetCityBuildingGroupsAsync(CityId cityId);
}
