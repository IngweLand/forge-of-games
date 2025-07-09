using Ingweland.Fog.Application.Core.CityPlanner;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface IHohCityFactory
{
    HohCity CreateNewCapital(int cityPlannerVersion);
    HohCity Create(NewCityRequest newCityRequest, int cityPlannerVersion);

    HohCity Create(string id, CityId inGameCityId, string ageId, string name,
        IEnumerable<CityMapEntity> entities, IEnumerable<CityMapEntity> inventoryBuildings,
        IReadOnlyCollection<HohCitySnapshot> snapshots, IEnumerable<string> expansions, int cityPlannerVersion,
        WonderId cityWonderId = WonderId.Undefined, int cityWonderLevel = 0);
}
