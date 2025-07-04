using Ingweland.Fog.Application.Core.CityPlanner;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface IHohCityFactory
{
    HohCity CreateNewCapital();
    HohCity Create(NewCityRequest newCityRequest);

    HohCity Create(string id, CityId inGameCityId, string ageId, string name,
        IEnumerable<CityMapEntity> entities, IReadOnlyCollection<HohCitySnapshot> snapshots,
        IEnumerable<string> expansions, WonderId cityWonderId = WonderId.Undefined, int cityWonderLevel = 0);
}