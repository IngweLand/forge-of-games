using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface IHohCityFactory
{
    HohCity CreateNewCapital();
    HohCity CreateNewCapital(string cityName);

    HohCity Create(string id, CityId inGameCityId, string ageId, string name,
        IReadOnlyCollection<CityMapEntity> entities, IReadOnlyCollection<HohCitySnapshot> snapshots);
}
