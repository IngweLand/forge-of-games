using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.CityPlanner;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;
using Refit;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface ICityService
{
    Task<IReadOnlyCollection<BuildingDto>> GetBarracks([AliasAs("unitType")] UnitType unitType);

    Task<IReadOnlyCollection<BuildingTypeDto>> GetBuildingCategoriesAsync([AliasAs("cityId")] CityId cityId);

    Task<BuildingGroupDto?> GetBuildingGroupAsync([AliasAs("cityId")] CityId cityId,
        [AliasAs("group")] BuildingGroup group, CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<CityId, IReadOnlyCollection<WonderBasicDto>>> GetWonderBasicDataAsync();

    Task<WonderDto?> GetWonderAsync([AliasAs("id")] WonderId id);

    Task<IReadOnlyCollection<Expansion>> GetExpansionsAsync([AliasAs("cityId")] CityId cityId);

    Task<IReadOnlyCollection<BuildingDto>> GetBuildingsAsync([AliasAs("cityId")] CityId cityId);

    Task<CityPlannerDataDto?> GetCityPlannerDataAsync([AliasAs("cityId")] CityId cityId);

    Task<IReadOnlyCollection<BuildingDto>> GetAllBarracks();
}
