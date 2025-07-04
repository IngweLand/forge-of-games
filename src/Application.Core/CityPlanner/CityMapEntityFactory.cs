using System.Drawing;
using Ingweland.Fog.Application.Core.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Core.CityPlanner.Stats.BuildingTypedStats;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.CityPlanner;

public class CityMapEntityFactory(ICityMapEntityStatsFactory mapEntityStatsFactory) : ICityMapEntityFactory
{
    private readonly HashSet<BuildingType> _notMovableEntities =
        [BuildingType.RitualSite, BuildingType.ExtractionPoint];

    private int _nextBuildingId = -1;

    public CityMapEntity Create(BuildingDto building, HohCityMapEntity hohCityMapEntity)
    {
        var overflowRange = FindOverflowRange(building, hohCityMapEntity.Level);

        var cme = new CityMapEntity(hohCityMapEntity.Id, new Point(hohCityMapEntity.X, hohCityMapEntity.Y),
            new Size(building.Width, building.Length), name: building.Name, cityEntityId: building.Id,
            hohCityMapEntity.Level,
            building.Type, building.Group, building.ExpansionSubType, overflowRange,
            !_notMovableEntities.Contains(building.Type))
        {
            IsRotated = hohCityMapEntity.IsRotated,
            SelectedProductId = hohCityMapEntity.SelectedProductId,
            CustomizationId = hohCityMapEntity.CustomizationId,
            Stats = mapEntityStatsFactory.Create(building),
        };
        return cme;
    }

    public CityMapEntity Create(BuildingDto building, Point location, BuildingLevelRange levelRange, int level = -1)
    {
        level = Math.Max(level, levelRange.StartLevel);
        var overflowRange = FindOverflowRange(building, level);

        var cme = new CityMapEntity(_nextBuildingId, location, new Size(building.Width, building.Length),
            name: building.Name,
            cityEntityId: building.Id, level, building.Type, building.Group, building.ExpansionSubType, overflowRange)
        {
            Stats = mapEntityStatsFactory.Create(building),
        };
        _nextBuildingId--;
        return cme;
    }

    private static int FindOverflowRange(BuildingDto building, int level)
    {
        var cultureComponent = building.Components.OfType<CultureComponent>().FirstOrDefault();
        var overflowRange = -1;
        if (cultureComponent != null)
        {
            overflowRange = cultureComponent.GetRange(level);
        }

        return overflowRange;
    }
}
