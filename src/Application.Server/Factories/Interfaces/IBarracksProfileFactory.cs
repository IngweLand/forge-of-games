using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Server.Factories.Interfaces;

public interface IBarracksProfileFactory
{
    BarracksProfile Create(IReadOnlyCollection<HohCityMapEntity> cityMapEntities, IEnumerable<Building> buildings);
}
