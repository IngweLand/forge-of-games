using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface IHohGameLocalizationService
{
    string GetContinentName(ContinentId id);
    string GetTreasureHuntDifficulty(int difficulty);
    string GetTreasureHuntStageName(int stage);
    string GetCityName(CityId id);
    string GetWonderName(string id);
    string GetHeroName(string id);
    string GetRegionName(RegionId id);
    string GetUnitName(string name);
    string GetHeroClassName(string name);
    string GetHeroAbilityDescription(string abilityDescriptionId);
    string GetHeroAbilityName(string abilityId);
    string GetUnitType(UnitType unitType);
    string GetBuildingName(string name);
    string GetAgeName(string ageId);
    string GetBuildingType(BuildingType buildingType, bool plural = false);
    string GetBuildingGroup(BuildingGroup buildingGroup, bool plural = false);
    string GetTechnologyName(string technologyId);
    string GetBuildingCustomizationName(string customizationId);
}
