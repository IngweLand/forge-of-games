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
    private static int _nextBuildingId = -1;

    private readonly HashSet<BuildingType> _notMovableEntities =
        [BuildingType.RitualSite, BuildingType.ExtractionPoint, BuildingType.PresetIrrigation];

    public CityMapEntity Create(BuildingDto building, HohCityMapEntity hohCityMapEntity)
    {
        var overflowRange = FindOverflowRange(building, hohCityMapEntity.Level);

        var cme = new CityMapEntity(hohCityMapEntity.Id, new Point(hohCityMapEntity.X, hohCityMapEntity.Y),
            new Size(building.Width, building.Length), building.Name, building.Id,
            hohCityMapEntity.Level,
            building.Type, building.Group, building.ExpansionSubType, overflowRange,
            !_notMovableEntities.Contains(building.Type), _notMovableEntities.Contains(building.Type),
            hohCityMapEntity.IsLocked, hohCityMapEntity.IsUpgrading)
        {
            IsRotated = hohCityMapEntity.IsRotated,
            SelectedProductId = hohCityMapEntity.SelectedProductId,
            CustomizationId = hohCityMapEntity.CustomizationId,
            Stats = mapEntityStatsFactory.Create(building),
            IsUnchanged = hohCityMapEntity.IsUnchanged,
        };
        return cme;
    }

    public CityMapEntity Create(BuildingDto building, Point location, BuildingLevelRange levelRange, int level = -1)
    {
        level = Math.Max(level, levelRange.StartLevel);
        var overflowRange = FindOverflowRange(building, level);

        var cme = new CityMapEntity(_nextBuildingId, location, new Size(building.Width, building.Length),
            building.Name, building.Id, level, building.Type, building.Group, building.ExpansionSubType, overflowRange)
        {
            Stats = mapEntityStatsFactory.Create(building),
        };
        _nextBuildingId--;
        return cme;
    }

    public CityMapEntity Duplicate(CityMapEntity sourceEntity, BuildingDto building)
    {
        if (sourceEntity.CityEntityId != building.Id)
        {
            throw new InvalidOperationException($"Source entity ID {sourceEntity.CityEntityId
            } does not match building ID {building.Id}");
        }

        var cme = new CityMapEntity(_nextBuildingId, sourceEntity.Location, new Size(building.Width, building.Length),
            building.Name, building.Id, sourceEntity.Level, building.Type, building.Group, building.ExpansionSubType,
            sourceEntity.OverflowRange)
        {
            Stats = mapEntityStatsFactory.Create(building),
            IsRotated = sourceEntity.IsRotated,
            CustomizationId = sourceEntity.CustomizationId,
            SelectedProductId = sourceEntity.SelectedProductId,
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
