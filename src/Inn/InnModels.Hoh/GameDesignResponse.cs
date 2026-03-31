using Ingweland.Fog.Inn.Models.Hoh.Extensions;

namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class GameDesignResponse
{
    public IList<AgeDefinitionDTO> AgeDefinitions => Items.FindAndUnpackToList<AgeDefinitionDTO>();

    public IList<BattleAbilityDefinitionDTO> BattleAbilityDefinitions =>
        Items.FindAndUnpackToList<BattleAbilityDefinitionDTO>();

    public IList<BuildingCustomizationDefinitionDTO> BuildingCustomizationDefinitions =>
        Items.FindAndUnpackToList<BuildingCustomizationDefinitionDTO>();

    public IList<BuildingDefinitionDTO> BuildingDefinitions => Items.FindAndUnpackToList<BuildingDefinitionDTO>();
    public IList<CityDefinitionDTO> CityDefinitions => Items.FindAndUnpackToList<CityDefinitionDTO>();
    public IList<ConstantsDefinitionDTO> ConstantsDefinitions => Items.FindAndUnpackToList<ConstantsDefinitionDTO>();
    public IList<ContinentDefinitionDTO> ContinentDefinitions => Items.FindAndUnpackToList<ContinentDefinitionDTO>();
    public IList<DifficultyDefinitionDTO> DifficultyDefinitions => Items.FindAndUnpackToList<DifficultyDefinitionDTO>();

    public IList<DynamicFloatValueDefinitionDTO> DynamicFloatValueDefinitions =>
        Items.FindAndUnpackToList<DynamicFloatValueDefinitionDTO>();

    public IList<EncounterDefinitionDTO> EncounterDefinitions => Items.FindAndUnpackToList<EncounterDefinitionDTO>();

    public IList<EquipmentSetDefinitionDTO> EquipmentSetDefinitions =>
        Items.FindAndUnpackToList<EquipmentSetDefinitionDTO>();

    public IList<ExpansionCostsDTO> ExpansionCosts => Items.FindAndUnpackToList<ExpansionCostsDTO>();
    public IList<ExpansionDefinitionDTO> ExpansionDefinitions => Items.FindAndUnpackToList<ExpansionDefinitionDTO>();
    public IList<FactionDefinitionDTO> FactionDefinitions => Items.FindAndUnpackToList<FactionDefinitionDTO>();

    public IList<HeroAbilityTrainingComponentDTO> HeroAbilityTrainingComponents =>
        Items.FindAndUnpackToList<HeroAbilityTrainingComponentDTO>();

    public IList<HeroAwakeningComponentDTO> HeroAwakeningComponents =>
        Items.FindAndUnpackToList<HeroAwakeningComponentDTO>();

    public IList<HeroBattleAbilityComponentDTO> HeroBattleAbilityComponents =>
        Items.FindAndUnpackToList<HeroBattleAbilityComponentDTO>();

    public HeroBattleConstantsDefinitionDTO HeroBattleConstantsDefinition =>
        Items.FindAndUnpackToList<HeroBattleConstantsDefinitionDTO>().Single();

    public IList<HeroBattleDefinitionDTO> HeroBattleDefinitions => Items.FindAndUnpackToList<HeroBattleDefinitionDTO>();

    public IList<HeroBattleWaveDefinitionDTO> HeroBattleWaveDefinitions =>
        Items.FindAndUnpackToList<HeroBattleWaveDefinitionDTO>();

    public IList<HeroBuildingBoostComponentDTO> HeroBuildingBoostComponents =>
        Items.FindAndUnpackToList<HeroBuildingBoostComponentDTO>();

    public IList<HeroClassDefinitionDTO> HeroClassDefinitions => Items.FindAndUnpackToList<HeroClassDefinitionDTO>();
    public IList<HeroDeckDefinitionDTO> HeroDeckDefinitions => Items.FindAndUnpackToList<HeroDeckDefinitionDTO>();
    public IList<HeroDefinitionDTO> HeroDefinitions => Items.FindAndUnpackToList<HeroDefinitionDTO>();

    public IList<HeroProgressionAscensionCostDefinitionDTO> HeroProgressionAscensionCostDefinitions =>
        Items.FindAndUnpackToList<HeroProgressionAscensionCostDefinitionDTO>();

    public IList<HeroProgressionCostDefinitionDTO> HeroProgressionCostDefinitions =>
        Items.FindAndUnpackToList<HeroProgressionCostDefinitionDTO>();

    public IList<HeroProgressionDefinitionDTO> HeroProgressionDefinitions =>
        Items.FindAndUnpackToList<HeroProgressionDefinitionDTO>();

    public IList<HeroStarUpDefinitionDTO> HeroStarUpDefinitions => Items.FindAndUnpackToList<HeroStarUpDefinitionDTO>();

    public IList<HeroUnitColorDefinitionDTO> HeroUnitColorDefinitions =>
        Items.FindAndUnpackToList<HeroUnitColorDefinitionDTO>();

    public IList<HeroUnitDefinitionDTO> HeroUnitDefinitions => Items.FindAndUnpackToList<HeroUnitDefinitionDTO>();

    public IList<HeroUnitRarityDefinitionDTO> HeroUnitRarityDefinitions =>
        Items.FindAndUnpackToList<HeroUnitRarityDefinitionDTO>();

    public IList<HeroUnitStatDefinitionDTO> HeroUnitStatDefinitions =>
        Items.FindAndUnpackToList<HeroUnitStatDefinitionDTO>();

    public IList<HeroUnitStatFormulaDefinitionDTO> HeroUnitStatFormulaDefinitions =>
        Items.FindAndUnpackToList<HeroUnitStatFormulaDefinitionDTO>();

    public IList<HeroUnitTypeDefinitionDTO> HeroUnitTypeDefinitions =>
        Items.FindAndUnpackToList<HeroUnitTypeDefinitionDTO>();

    public IList<RegionDefinitionDTO> RegionDefinitions => Items.FindAndUnpackToList<RegionDefinitionDTO>();

    public RelicBoostAgeModifierDefinitionDTO RelicBoostAgeModifiers =>
        Items.FindAndUnpack<RelicBoostAgeModifierDefinitionDTO>();

    public IList<RelicDefinitionDTO> RelicDefinitions => Items.FindAndUnpackToList<RelicDefinitionDTO>();
    public IList<ResourceDefinitionDTO> ResourceDefinitions => Items.FindAndUnpackToList<ResourceDefinitionDTO>();
    public IList<RewardDefinitionDTO> RewardDefinitions => Items.FindAndUnpackToList<RewardDefinitionDTO>();

    public IList<ReworkedWonderDefinitionDTO> ReworkedWonderDefinitions =>
        Items.FindAndUnpackToList<ReworkedWonderDefinitionDTO>();

    public IList<TechnologyDefinitionDTO> TechnologyDefinitions => Items.FindAndUnpackToList<TechnologyDefinitionDTO>();
    public IList<WorldDefinitionDTO> WorldDefinitions => Items.FindAndUnpackToList<WorldDefinitionDTO>();

    public IList<WorldTypeDefinitionDTO> WorldTypeDefinitions => Items.FindAndUnpackToList<WorldTypeDefinitionDTO>();
}
