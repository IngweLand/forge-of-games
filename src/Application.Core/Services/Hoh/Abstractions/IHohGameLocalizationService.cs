using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface IHohGameLocalizationService
{
    string GetAgeName(string ageId);
    string GetBuildingCustomizationName(string customizationId);
    string GetBuildingGroup(BuildingGroup buildingGroup, bool plural = false);
    string GetBuildingName(string name);
    string GetBuildingType(BuildingType buildingType, bool plural = false);
    string GetCityName(CityId id);
    string GetContinentName(ContinentId id);
    string GetDifficultyName(Difficulty difficulty);
    string GetHeroAbilityDescription(string abilityDescriptionId);
    string GetHeroAbilityName(string abilityId);
    string GetHeroClassName(string name);
    string GetHeroName(string id);
    string GetRegionName(RegionId id);
    string GetTechnologyName(string technologyId);
    string GetTreasureHuntDifficulty(int difficulty);
    string GetTreasureHuntStageName(int stage);
    string GetUnitName(string name);
    string GetUnitType(UnitType unitType);
    string GetWonderName(string id);
}
