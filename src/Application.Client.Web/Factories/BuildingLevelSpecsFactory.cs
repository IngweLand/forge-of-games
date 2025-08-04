using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class BuildingLevelSpecsFactory : IBuildingLevelSpecsFactory
{
    public BuildingLevelSpecs Create(BuildingDto building)
    {
        return new BuildingLevelSpecs
        {
            AgeName = building.CityIds.Contains(CityId.Capital) ? building.Age?.Name : null,
            AgeColor = building.Age.ToCssColor(),
            Level = building.Level,
            CanBeConstructed = building.Components.OfType<ConstructionComponent>().Any(),
            CanBeUpgradedTo = building.Components.OfType<UpgradeComponent>().Any(),
        };
    }


    public IReadOnlyCollection<BuildingLevelSpecs> Create(BuildingLevelRange levelRange)
    {
        var levels = new List<BuildingLevelSpecs>();
        for (var i = levelRange.StartLevel; i < levelRange.EndLevel + 1; i++)
        {
            levels.Add(new BuildingLevelSpecs
            {
                Level = i,
                CanBeConstructed = i == levelRange.StartLevel,
                CanBeUpgradedTo = i > levelRange.StartLevel,
            });
        }

        return levels;
    }
}
