using System.Collections.ObjectModel;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Helpers;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Factories;

public class UnitDtoFactory(IHohGameLocalizationService localizationService, ILogger<UnitDtoFactory> logger)
    : IUnitDtoFactory
{
    public UnitDto Create(Unit unit, IEnumerable<UnitStatFormulaData> unitStatCalculationData,
        UnitBattleConstants unitBattleConstants, HeroUnitType heroUnitType)
    {
        var statFactors = new Dictionary<UnitStatType, UnitStatFormulaFactors>();
        foreach (var formulaData in unitStatCalculationData)
        {
            if (unitBattleConstants.FormulaTypeToStatTypeMap.TryGetValue(formulaData.Type, out var statType))
            {
                if (unit.RarityId == null)
                {
                    statFactors.Add(statType, new UnitStatFormulaFactors() {Normal = formulaData.BaseFactor});
                }
                else
                {
                    if (formulaData.RarityFactors.TryGetValue(unit.RarityId, out var formulaFactors))
                    {
                        statFactors.Add(statType, formulaFactors);
                    }
                    else
                    {
                        logger.LogError($"Unknown rarity: {unit.RarityId}");
                    }
                }
            }
            else
            {
                // logger.LogError($"Unknown formula type: {formulaData.Type}");
            }
        }

        var missingStats = unitBattleConstants.BaseValues.Where(us => !unit.Stats.Any(us2 => us2.Type == us.Type));
        var stats = unit.Stats.Concat(missingStats).ToList();
        stats = stats.Concat(heroUnitType.BaseValues.Where(us => stats.All(us2 => us2.Type != us.Type))).ToList();
        return new UnitDto
        {
            Name = localizationService.GetUnitName(StringParser.GetConcreteId(unit.Id)),
            AssetId = unit.Name,
            Color = unit.Color,
            Id = unit.Id,
            Stats = stats,
            Type = unit.Type,
            StatCalculationFactors = new ReadOnlyDictionary<UnitStatType, UnitStatFormulaFactors>(statFactors),
            TypeName = localizationService.GetUnitType(unit.Type),
        };
    }
}
