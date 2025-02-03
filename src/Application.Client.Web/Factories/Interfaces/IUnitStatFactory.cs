using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Factories.Interfaces;

public interface IUnitStatFactory
{
    IReadOnlyDictionary<UnitStatType, float> CreateMainHeroStats(HeroDto hero,
        int level, int ascensionLevel, int awakeningLevel, BuildingDto? barracks);

    IReadOnlyDictionary<UnitStatType, float> CreateMainSupportUnitStats(IUnit unit, int level,
        IReadOnlyDictionary<UnitStatType, UnitStatFormulaFactors> statCalculationFactors);
}
