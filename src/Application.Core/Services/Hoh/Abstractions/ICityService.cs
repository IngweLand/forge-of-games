using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.CityPlanner;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;
using Refit;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface ICityService
{
    [Get("/city/barracks/{unitType}")]
    Task<IReadOnlyCollection<BuildingDto>> GetBarracks([AliasAs("unitType")] UnitType unitType);

    [Get("/city/buildingCategories/{cityId}")]
    Task<IReadOnlyCollection<BuildingTypeDto>> GetBuildingCategoriesAsync([AliasAs("cityId")] CityId cityId);

    [Get("/city/buildingGroup/{cityId}/{group}")]
    Task<BuildingGroupDto?> GetBuildingGroupAsync([AliasAs("cityId")] CityId cityId,
        [AliasAs("group")] BuildingGroup group, CancellationToken cancellationToken = default);

    [Get("/city/wonders/basic")]
    Task<IReadOnlyDictionary<CityId, IReadOnlyCollection<WonderBasicDto>>> GetWonderBasicDataAsync();

    [Get("/city/wonders/{id}")]
    Task<WonderDto?> GetWonderAsync([AliasAs("id")] WonderId id);

    [Get("/city/{cityId}/expansions")]
    Task<IReadOnlyCollection<Expansion>> GetExpansionsAsync([AliasAs("cityId")] CityId cityId);

    [Get("/city/{cityId}/buildings")]
    Task<IReadOnlyCollection<BuildingDto>> GetBuildingsAsync([AliasAs("cityId")] CityId cityId);

    [Get("/cityPlanner/data/{cityId}")]
    Task<CityPlannerDataDto?> GetCityPlannerDataAsync([AliasAs("cityId")] CityId cityId);

    [Get("/city/barracks")]
    Task<IReadOnlyCollection<BuildingDto>> GetAllBarracks();

    [Get(FogUrlBuilder.ApiRoutes.COMMON_CITIES)]
    Task<IReadOnlyCollection<CityDto>> GetCitiesAsync();
}
