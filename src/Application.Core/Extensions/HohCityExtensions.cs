using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.Extensions;

public static class HohCityExtensions
{
    public static BuildingGroup ToBuildingGroup(this UnitType unitType)
    {
        return unitType switch
        {
            UnitType.Cavalry => BuildingGroup.CavalryBarracks,
            UnitType.HeavyInfantry => BuildingGroup.HeavyInfantryBarracks,
            UnitType.Infantry => BuildingGroup.InfantryBarracks,
            UnitType.Ranged => BuildingGroup.RangedBarracks,
            UnitType.Siege => BuildingGroup.SiegeBarracks,
            _ => BuildingGroup.Undefined,
        };
    }
}
