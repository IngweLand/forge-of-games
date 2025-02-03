using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Core.Calculators.Interfaces;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class HohHeroSupportUnitProfileFactory(
    IUnitStatFactory unitStatFactory,
    IUnitPowerCalculator unitPowerCalculator)
    : IHohHeroSupportUnitProfileFactory
{
    public HeroSupportUnitProfile Create(UnitDto baseSupportUnit, BuildingDto? barracks)
    {
        var component = barracks?.Components.OfType<BuildingUnitProviderComponent>().FirstOrDefault();
        IUnit unit;
        var unitLevel = 1;
        var assetId = baseSupportUnit.AssetId;
        if (component != null)
        {
            unit = component.BuildingUnit.Unit;
            unitLevel = component.BuildingUnit.Level;
            assetId = component.BuildingUnit.Unit.Name;
        }
        else
        {
            unit = baseSupportUnit;
        }

        var stats = unitStatFactory.CreateMainSupportUnitStats(unit, unitLevel, baseSupportUnit.StatCalculationFactors);
        var allStats = new Dictionary<UnitStatType, float>(stats);
        var missingStats = unit.Stats.Concat(baseSupportUnit.Stats.Where(us => !unit.Stats.Any(us2 => us2.Type == us.Type)));
        foreach (var us in missingStats.Where(us => !stats.ContainsKey(us.Type)))
        {
            allStats.Add(us.Type, us.Value);
        }

        var power = unitPowerCalculator.CalculateUnitPower(allStats);
        return new HeroSupportUnitProfile()
        {
            Stats = allStats,
            Power = (int) Math.Ceiling(power),
            Unit = unit,
            AssetIt = assetId,
        };
    }
}
