using Ingweland.Fog.Inn.Models.Hoh.Extensions;

namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class GameDesignResponseDTO
{
    public IList<CityDefinitionDTO> CityDefinitions => Items.FindAndUnpackToList<CityDefinitionDTO>();
    public IList<BuildingCustomizationDefinitionDTO> BuildingCustomizationDefinitions => Items.FindAndUnpackToList<BuildingCustomizationDefinitionDTO>();
    public IList<ConstantsDefinitionDTO> ConstantsDefinitions => Items.FindAndUnpackToList<ConstantsDefinitionDTO>();
    public IList<ContinentDefinitionDTO> ContinentDefinitions => Items.FindAndUnpackToList<ContinentDefinitionDTO>();
    public IList<DifficultyDefinitionDTO> DifficultyDefinitions => Items.FindAndUnpackToList<DifficultyDefinitionDTO>();
    public IList<EncounterDefinitionDTO> EncounterDefinitions => Items.FindAndUnpackToList<EncounterDefinitionDTO>();
    public IList<FactionDefinitionDTO> FactionDefinitions => Items.FindAndUnpackToList<FactionDefinitionDTO>();
    public IList<HeroBattleDefinitionDTO> HeroBattleDefinitions => Items.FindAndUnpackToList<HeroBattleDefinitionDTO>();
    public IList<AgeDefinitionDTO> AgeDefinitions => Items.FindAndUnpackToList<AgeDefinitionDTO>();

    public IList<HeroBattleWaveDefinitionDTO> HeroBattleWaveDefinitions =>
        Items.FindAndUnpackToList<HeroBattleWaveDefinitionDTO>();

    public IList<HeroClassDefinitionDTO> HeroClassDefinitions => Items.FindAndUnpackToList<HeroClassDefinitionDTO>();
    public IList<HeroDeckDefinitionDTO> HeroDeckDefinitions => Items.FindAndUnpackToList<HeroDeckDefinitionDTO>();

    public IList<HeroProgressionAscensionCostDefinitionDTO> HeroProgressionAscensionCostDefinitions =>
        Items.FindAndUnpackToList<HeroProgressionAscensionCostDefinitionDTO>();

    public IList<HeroProgressionCostDefinitionDTO> HeroProgressionCostDefinitions =>
        Items.FindAndUnpackToList<HeroProgressionCostDefinitionDTO>();

    public IList<HeroProgressionDefinitionDTO> HeroProgressionDefinitions =>
        Items.FindAndUnpackToList<HeroProgressionDefinitionDTO>();

    public IList<HeroUnitColorDefinitionDTO> HeroUnitColorDefinitions =>
        Items.FindAndUnpackToList<HeroUnitColorDefinitionDTO>();

    public IList<HeroUnitRarityDefinitionDTO> HeroUnitRarityDefinitions =>
        Items.FindAndUnpackToList<HeroUnitRarityDefinitionDTO>();

    public IList<HeroUnitStatDefinitionDTO> HeroUnitStatDefinitions =>
        Items.FindAndUnpackToList<HeroUnitStatDefinitionDTO>();

    public IList<HeroUnitTypeDefinitionDTO> HeroUnitTypeDefinitions =>
        Items.FindAndUnpackToList<HeroUnitTypeDefinitionDTO>();

    public IList<RegionDefinitionDTO> RegionDefinitions => Items.FindAndUnpackToList<RegionDefinitionDTO>();
    public IList<ResourceDefinitionDTO> ResourceDefinitions => Items.FindAndUnpackToList<ResourceDefinitionDTO>();
    public IList<RewardDefinitionDTO> RewardDefinitions => Items.FindAndUnpackToList<RewardDefinitionDTO>();
    public IList<TechnologyDefinitionDTO> TechnologyDefinitions => Items.FindAndUnpackToList<TechnologyDefinitionDTO>();
    public IList<WorldDefinitionDTO> WorldDefinitions => Items.FindAndUnpackToList<WorldDefinitionDTO>();

    public IList<WorldTypeDefinitionDTO> WorldTypeDefinitions => Items.FindAndUnpackToList<WorldTypeDefinitionDTO>();
    public IList<BuildingDefinitionDTO> BuildingDefinitions => Items.FindAndUnpackToList<BuildingDefinitionDTO>();
    public IList<HeroBuildingBoostComponentDTO> HeroBuildingBoostComponents => Items.FindAndUnpackToList<HeroBuildingBoostComponentDTO>();
    public IList<HeroAbilityTrainingComponentDTO> HeroAbilityTrainingComponents => Items.FindAndUnpackToList<HeroAbilityTrainingComponentDTO>();
    public IList<HeroUnitDefinitionDTO> HeroUnitDefinitions => Items.FindAndUnpackToList<HeroUnitDefinitionDTO>();
    public IList<HeroDefinitionDTO> HeroDefinitions => Items.FindAndUnpackToList<HeroDefinitionDTO>();
    
    public HeroBattleConstantsDefinitionDTO HeroBattleConstantsDefinition => Items.FindAndUnpackToList<HeroBattleConstantsDefinitionDTO>().Single();
    public IList<HeroUnitStatFormulaDefinitionDTO> HeroUnitStatFormulaDefinitions => Items.FindAndUnpackToList<HeroUnitStatFormulaDefinitionDTO>();
    public IList<BattleAbilityDefinitionDTO> BattleAbilityDefinitions => Items.FindAndUnpackToList<BattleAbilityDefinitionDTO>();
    public IList<HeroBattleAbilityComponentDTO> HeroBattleAbilityComponents => Items.FindAndUnpackToList<HeroBattleAbilityComponentDTO>();
    public IList<ReworkedWonderDefinitionDTO> ReworkedWonderDefinitions => Items.FindAndUnpackToList<ReworkedWonderDefinitionDTO>();
    public IList<HeroAwakeningComponentDTO> HeroAwakeningComponents => Items.FindAndUnpackToList<HeroAwakeningComponentDTO>();
    public IList<ExpansionDefinitionDTO> ExpansionDefinitions => Items.FindAndUnpackToList<ExpansionDefinitionDTO>();
    public IList<DynamicFloatValueDefinitionDTO> DynamicFloatValueDefinitions => Items.FindAndUnpackToList<DynamicFloatValueDefinitionDTO>();
}
