using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class BuildingLevelRangesFactory : IBuildingLevelRangesFactory
{
    public IReadOnlyDictionary<BuildingGroup, BuildingLevelRange> Create(IReadOnlyCollection<BuildingDto> buildings)
    {
        var groups = buildings.Where(b => b.Type != BuildingType.Evolving).GroupBy(b => b.Group);
        var result = new Dictionary<BuildingGroup, BuildingLevelRange>();
        foreach (var group in groups)
        {
            var ordered = group.OrderBy(b => b.Level).ToList();
            var levelRange = new BuildingLevelRange()
            {
                StartLevel = ordered.First().Level,
                EndLevel = ordered.Last().Level,
            };
            result.Add(group.Key, levelRange);
        }

        var evolvingBuildings = buildings.Where(b => b.Type == BuildingType.Evolving);
        foreach (var evolvingBuilding in evolvingBuildings)
        {
            var levelUpComponent = evolvingBuilding.Components.OfType<LevelUpComponent>().First();
            var levelRange = new BuildingLevelRange()
            {
                StartLevel = 1,
                EndLevel = levelUpComponent.StarLevels.Last(),
            };
            result.Add(evolvingBuilding.Group, levelRange);
        }

        return result;
    }
}
