using System.Globalization;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Enums.Hoh;
using Ingweland.Fog.Application.Server.Extensions;
using Ingweland.Fog.Application.Server.Helpers;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class HohGameLocalizationService(IHohGameLocalizationDataRepository localizationDataRepository)
    : IHohGameLocalizationService
{
    public string GetContinentName(ContinentId id)
    {
        var key = HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.Continents, HohLocalizationProperty.Name,
            $"continent.{id}");
        return GetValue(key) ?? id.ToString();
    }

    public string GetTreasureHuntDifficulty(int difficulty)
    {
        var key = HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.TreasureHuntDifficulties,
            HohLocalizationProperty.Name,
            $"Difficulty_{difficulty}");
        return GetValue(key) ?? difficulty.ToString();
    }

    public string GetTreasureHuntStageName(int stage)
    {
        var key = HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.TreasureHuntDifficultiesPanel, "Stage");
        var value = GetValue(key);
        return value != null ? $"{value} {stage + 1}" : (stage + 1).ToString();
    }

    public string GetCityName(CityId id)
    {
        var key = HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.Cities, HohLocalizationProperty.Name,
            $"City_{id}");
        return GetValue(key) ?? id.ToString();
    }

    public string GetWonderName(string id)
    {
        var key = HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.Wonders, HohLocalizationProperty.Name,
            $"Wonder_{id}");
        return GetValue(key) ?? id.ToString();
    }

    public string GetHeroName(string id)
    {
        var key = HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.Heroes, HohLocalizationProperty.Name,
            $"hero.{id}");
        return GetValue(key) ?? id;
    }

    public string GetRegionName(RegionId id)
    {
        var key = HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.Regions, HohLocalizationProperty.Name,
            $"region.{id}");
        return GetValue(key) ?? id.ToString();
    }

    public string GetUnitName(string name)
    {
        var key = HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.Units, HohLocalizationProperty.Name, name);
        return GetValue(key) ?? name;
    }

    public string GetHeroClassName(string name)
    {
        var key = HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.HeroClass, name);
        return GetValue(key) ?? name;
    }

    public string GetHeroAbilityDescription(string abilityDescriptionId)
    {
        var keyName = HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.AbilityDescriptions,
            HohLocalizationProperty.Name, abilityDescriptionId);
        var keyDesc = HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.AbilityDescriptions,
            HohLocalizationProperty.Desc, abilityDescriptionId);
        return GetValue(keyName) ?? GetValue(keyDesc) ?? abilityDescriptionId;
    }

    public string GetHeroAbilityName(string abilityId)
    {
        var key = HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.Abilities,
            HohLocalizationProperty.Name, abilityId);
        return GetValue(key) ?? abilityId;
    }

    public string GetUnitType(UnitType unitType)
    {
        var key = HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.UnitTypes, HohLocalizationProperty.Name,
            unitType.ToString());
        return GetValue(key) ?? unitType.ToString();
    }

    public string GetBuildingName(string name)
    {
        var key = HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.Buildings, HohLocalizationProperty.Name,
            name);
        return GetValue(key) ?? name;
    }

    public string GetAgeName(string ageId)
    {
        var key = HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.Ages, HohLocalizationProperty.Name,
            ageId);
        return GetValue(key) ?? ageId;
    }

    public string GetBuildingType(BuildingType buildingType, bool plural = false)
    {
        var key = HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.BuildingTypes,
            HohLocalizationProperty.Name,
            buildingType.ToStringRepresentation());
        return GetValue(key, plural) ?? buildingType.ToString();
    }

    public string GetBuildingGroup(BuildingGroup buildingGroup, bool plural = false)
    {
        var key = HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.BuildingGroups,
            HohLocalizationProperty.Name,
            buildingGroup.ToStringRepresentation());
        return GetValue(key, plural) ?? buildingGroup.ToString();
    }

    public string GetTechnologyName(string technologyId)
    {
        var key = HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.Technologies, HohLocalizationProperty.Name,
            technologyId);
        return GetValue(key) ?? technologyId;
    }

    public string GetBuildingCustomizationName(string customizationId)
    {
        var key = HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.BuildingCustomizations, HohLocalizationProperty.Name,
            customizationId);
        return GetValue(key) ?? customizationId;
    }

    private string? GetValue(string key, bool plural = false)
    {
        if (!localizationDataRepository.GetEntries(CultureInfo.CurrentCulture.Name).TryGetValue(key, out var values))
        {
            return null;
        }

        if (!plural)
        {
            return values.First();
        }

        return values.Count > 1 ? values.Skip(1).First() : values.First();
    }
}
