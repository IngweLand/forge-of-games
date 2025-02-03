using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Factories;

public class BarracksProfileFactory : IBarracksProfileFactory
{
    public BarracksProfile Create(IReadOnlyCollection<HohCityMapEntity> cityMapEntities,
        IEnumerable<Building> buildings)
    {
        var barracks = buildings.Where(b => b.Type == BuildingType.Barracks).ToDictionary(b => b.Id);
        var barracksEntities = cityMapEntities.Where(cme => barracks.Any(kvp => kvp.Key == cme.CityEntityId));
        var profile = new BarracksProfile();
        foreach (var entity in barracksEntities)
        {
            var concreteBarracks = barracks[entity.CityEntityId];
            profile.Levels[concreteBarracks.Group] = entity.Level;
        }

        return profile;
    }
}
