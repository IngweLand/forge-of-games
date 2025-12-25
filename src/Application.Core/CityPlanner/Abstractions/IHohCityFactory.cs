using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.CityPlanner.Abstractions;

public interface IHohCityFactory
{
    HohCity CreateNewCapital(int cityPlannerVersion);
    HohCity Create(NewCityRequest newCityRequest, int cityPlannerVersion);

    HohCity Create(string id, CityId inGameCityId, string ageId, string name,
        IEnumerable<CityMapEntity> entities, IEnumerable<CityMapEntity> inventoryBuildings,
        IReadOnlyCollection<HohCitySnapshot> snapshots, IEnumerable<string> expansions, int cityPlannerVersion,
        WonderId cityWonderId = WonderId.Undefined, int cityWonderLevel = 0);

    HohCity Create(string id, CityId inGameCityId, string ageId, string name,
        IReadOnlyCollection<HohCityMapEntity> entities, HashSet<string> expansions, int cityPlannerVersion,
        WonderId cityWonderId = WonderId.Undefined, int cityWonderLevel = 0);

    HohCity Create(City inGameCity, IReadOnlyDictionary<string, Building> buildings, WonderId wonderId, int wonderLevel,
        string? name = null);

    HohCity Create(OtherCity inGameCity, IReadOnlyDictionary<string, Building> buildings, string name);
}
