using AutoMapper;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using HohProtoParser.Extensions;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Entities.Research;
using Ingweland.Fog.Models.Hoh.Entities.Rewards;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Helpers;

namespace HohProtoParser.Converters;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ResourceDTO, ResourceAmount>()
            .ForMember(dest => dest.ResourceId, opt => opt.MapFrom(r => r.Id))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(r => Math.Abs(r.Amount)));
        CreateMap<BuildingBuffDetailsDto, BuildingBuffDetails>()
            .ForMember(dest => dest.Resources, opt => opt.PreCondition(bbd => bbd.Resources != null));
        CreateMap<BuildingBuffResourceDto, BuildingBuffResource>()
            .ForMember(dest => dest.ResourceId, opt => opt.MapFrom(bbr => bbr.ResourceType));
        CreateMap<HeroUnitStatValueDefinitionDTO, UnitStat>()
            .ForMember(dest => dest.Type,
                opt => opt.MapFrom(husvd => HohStringParser.ParseEnumFromString<UnitStatType>(husvd.StatId)));
        CreateMap<HeroUnitStatBaseValueDto, UnitStat>()
            .ForMember(dest => dest.Type,
                opt => opt.MapFrom(src => HohStringParser.ParseEnumFromString<UnitStatType>(src.StatId)));
        CreateMap<UnitStatDto, UnitStat>()
            .ForMember(dest => dest.Type,
                opt => opt.MapFrom(src => HohStringParser.ParseEnumFromString<UnitStatType>(src.Type)));
        CreateMap<HeroUnitDefinitionDTO, Unit>()
            .ForMember(dest => dest.Type,
                opt => opt.MapFrom(hud => HohStringParser.ParseEnumFromString<UnitType>(hud.Type)))
            .ForMember(dest => dest.Color,
                opt => opt.MapFrom(hud => HohStringParser.GetConcreteId(hud.Color).ToUnitColor()));
        CreateMap<BuildingUnitDto, BuildingUnit>()
            .ForMember(dest => dest.Unit, opt => opt.ConvertUsing(new UnitValueConverter(), bu => bu.Id));
        CreateMap<HeroBattleWaveHeroDetailsDto, BattleWaveHeroSquad>()
            .ForMember(dest => dest.UnitId, opt => opt.MapFrom(d => d.Id))
            .ForMember(dest => dest.UnitLevel, opt => opt.MapFrom(d => d.Level));
        CreateMap<(HeroBattleWaveUnitSquadDetailsDto details, Unit unit), BattleWaveUnitSquad>()
            .ForMember(dest => dest.UnitId, opt => opt.MapFrom(src => src.details.Id))
            .ForMember(dest => dest.UnitLevel, opt => opt.MapFrom(src => src.details.Level))
            .ForMember(dest => dest.Size, opt => opt.MapFrom(src =>
                src.details.Stats != null && src.details.Stats.Value > 0
                    ? src.details.Stats.Value
                    : src.unit.Stats.Single(stat => stat.Type == UnitStatType.SquadSize).Value));
        CreateMap<HeroProgressionCostResourceDto, HeroProgressionCostResource>();
        CreateMap<HeroProgressionCostDefinitionDTO, HeroProgressionCost>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => HohStringParser.ParseEnumFromString<HeroProgressionCostId>(src.Id)))
            .ForMember(dest => dest.LevelCosts,
                opt => opt.MapFrom(src => src.Cost.ToDictionary(hpc => hpc.Level,
                    hpc => hpc.Resource)));
        CreateMap<HeroProgressionAscensionCostDefinitionDTO, HeroAscensionCost>()
            .ForMember(dest => dest.LevelCosts,
                opt => opt.MapFrom(src => src.Cost.ToDictionary(hpc => hpc.Level,
                    hpc => hpc.Resources.Resources)));
        CreateMap<HeroUnitStatFormulaDefinitionFactorsDto, UnitStatFormulaFactors>();
        CreateMap<BattleAbilityDefinitionDescriptionItemDto, HeroAbilityDescriptionItem>();
        CreateMap<BattleAbilityDefinitionDescriptionItemValueDto, HeroAbilityDescriptionItemValue>()
            .ForMember(dest => dest.Type,
                opt => opt.MapFrom(src => HohStringParser.ParseEnumFromString<NumericValueType>(src.Type)));
        CreateMap<HeroBattleAbilityComponentLevelDto, HeroBattleAbilityComponentLevel>();
        CreateMap<WonderCrateDto, WonderCrate>();
        CreateMap<AwakeningLevelDto, AwakeningLevel>()
            .ForMember(dest => dest.IsPercentage, opt => opt.MapFrom(src => src.LevelValue.IsPercentage))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.LevelValue.Value))
            .ForMember(dest => dest.StatType,
                opt => opt.MapFrom(src =>
                    HohStringParser.ParseEnumFromString<UnitStatType>(src.LevelValue.UnitStatId)));
        CreateMap<ExpansionDefinitionDTO, Expansion>()
            .ForMember(dest => dest.CityId, opt => opt.ConvertUsing(new CityIdValueConverter(), src => src.CityId))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => HohStringParser.GetConcreteId(src.Id)))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.SubType, opt => opt.MapFrom(src => src.Subtype))
            .ForMember(dest => dest.Y, opt => opt.MapFrom(new ExpansionYValueResolver()))
            .ForMember(dest => dest.LinkedExpansionId,
                opt => opt.MapFrom(src =>
                    src.LinkedExpansionComponent != null ? src.LinkedExpansionComponent.LinkedExpansionId : null));
        CreateMap<ExpansionDefinitionDTO, ExpansionBasicData>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => HohStringParser.GetConcreteId(src.Id)))
            .ForMember(dest => dest.CityId, opt => opt.ConvertUsing(new CityIdValueConverter(), src => src.CityId))
            .ForMember(dest => dest.Y, opt => opt.MapFrom(new ExpansionYValueResolver()));
        CreateMap<ExpansionDefinitionDTO, Expansion>()
            .IncludeBase<ExpansionDefinitionDTO, ExpansionBasicData>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.SubType, opt => opt.MapFrom(src => src.Subtype))
            .ForMember(dest => dest.LinkedExpansionId,
                opt => opt.MapFrom(src =>
                    src.LinkedExpansionComponent != null
                        ? HohStringParser.GetConcreteId(src.LinkedExpansionComponent.LinkedExpansionId)
                        : null));
        CreateMap<string, UnitType>()
            .ConvertUsing(src => HohStringParser.ParseEnumFromString<UnitType>(src));
        CreateMap<string, HeroClassId>()
            .ConvertUsing(src => HohStringParser.ParseEnumFromString<HeroClassId>(src));
        CreateMap<MapField<string, RegionRewardDto>, IReadOnlyDictionary<Difficulty, RegionReward>>()
            .ConvertUsing((src, _, context) =>
                src.ToDictionary(kvp => HohStringParser.ParseEnumFromString<Difficulty>(kvp.Key),
                    kvp => context.Mapper.Map<RegionReward>(kvp.Value)));
        CreateMap<MapField<string, EncounterDetailsDto>, IReadOnlyDictionary<Difficulty, EncounterDetails>>()
            .ConvertUsing((src, _, context) =>
                src.ToDictionary(kvp => HohStringParser.ParseEnumFromString<Difficulty>(kvp.Key),
                    kvp =>
                    {
                        context.Items[ContextKeys.DIFFICULTY] =
                            HohStringParser.ParseEnumFromString<Difficulty>(kvp.Key);
                        return context.Mapper.Map<EncounterDetails>(kvp.Value);
                    }));
        CreateMap<EncounterDetailsDto, EncounterDetails>().ConvertUsing(new EncounterDetailsDtoConverter());
        CreateMap<BuildingLevelDynamicChangeDTO, Dictionary<int, float>>()
            .ConvertUsing(new BuildingLevelDynamicChangeDtoConverter());

        // components
        CreateMap<UpgradeComponentDTO, UpgradeComponent>()
            .ForMember(dest => dest.UpgradeTime, opt => opt.MapFrom(uc => uc.UpgradeTime.Seconds))
            .ForMember(dest => dest.WorkerCount, opt => opt.MapFrom(uc => uc.WorkerCount.Count))
            .ForMember(dest => dest.Cost, opt => opt.MapFrom(uc => uc.Requirements.Cost))
            .ForMember(dest => dest.NextBuildingId,
                opt => opt.MapFrom(uc => HohStringParser.GetConcreteId(uc.NextBuildingId)));
        CreateMap<ProductionComponentDTO, ProductionComponent>()
            .ForMember(dest => dest.ProductionTime, opt => opt.MapFrom(uc => uc.ProductionTime.Seconds))
            .ForMember(dest => dest.WorkerCount, opt =>
            {
                opt.PreCondition(uc => uc.WorkerBehaviour != null);
                opt.MapFrom(uc => uc.WorkerBehaviour!.WorkerCount);
            })
            .ForMember(dest => dest.Cost, opt => opt.MapFrom(uc => uc.Cost.Resources))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(uc => uc.Product.PackedRewards));
        CreateMap<ConstructionComponentDTO, ConstructionComponent>()
            .ForMember(dest => dest.BuildTime, opt => opt.MapFrom(uc => uc.BuildTime.Seconds))
            .ForMember(dest => dest.WorkerCount, opt => opt.MapFrom(uc => uc.WorkerCount.Count))
            .ForMember(dest => dest.Cost, opt => opt.MapFrom(uc => uc.Requirements.Cost));
        CreateMap<GrantWorkerComponentDTO, GrantWorkerComponent>()
            .ConvertUsing(new GrantWorkerComponentTypeConverter());
        CreateMap<CultureComponentDTO, CultureComponent>().ConvertUsing(new CultureComponentTypeConverter());
        CreateMap<BuildingUnitProviderComponentDTO, BuildingUnitProviderComponent>()
            .ForMember(dest => dest.BuildingUnit, opt => opt.MapFrom(bupc => bupc.Unit));
        CreateMap<HeroAbilityTrainingComponentDTO, HeroAbilityTrainingComponent>();
        CreateMap<HeroBuildingBoostComponentDTO, HeroBuildingBoostComponent>()
            .ForMember(dest => dest.UnitType,
                opt => opt.MapFrom(src => HohStringParser.ParseEnumFromString<UnitType>(src.UnitType)));
        CreateMap<BoostResourceComponentDTO, BoostResourceComponent>()
            .ConvertUsing(new BoostResourceComponentTypeConverter());
        CreateMap<RepeatedField<Any>, IList<ComponentBase>>().ConvertUsing<ComponentDtoConverter>();
        CreateMap<HeroProgressionComponentDTO, HeroProgressionComponent>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(hpc => HohStringParser.ParseEnumFromString<HeroStarClass>(hpc.Id)))
            .ForMember(dest => dest.CostId,
                opt => opt.MapFrom(hpc => HohStringParser.ParseEnumFromString<HeroProgressionCostId>(hpc.CostId)));
        CreateMap<HeroBattleAbilityComponentDTO, HeroBattleAbilityComponent>();
        CreateMap<WonderLevelUpComponentDTO, WonderLevelUpComponent>().ConvertUsing(new WonderLevelCostsConverter());
        CreateMap<HeroAbilityTrainingComponentDTO, HeroAbilityTrainingComponent>()
            .ForMember(dest => dest.UnitType,
                opt => opt.MapFrom(src => HohStringParser.ParseEnumFromString<UnitType>(src.UnitType)));
        CreateMap<HeroAwakeningComponentDTO, HeroAwakeningComponent>();
        CreateMap<ResearchComponentDTO, ResearchComponent>()
            .ForMember(dest => dest.ParentTechnologies,
                opt => opt.MapFrom(src =>
                    src.Details.ResearchRequirements.Select<ResearchRequirementDTO, string>(rr =>
                        HohStringParser.GetConcreteId(rr.TechnologyId))))
            .ForMember(dest => dest.Rewards, opt => opt.MapFrom(src => src.Rewards.PackedRewards))
            .ForMember(dest => dest.Costs, opt => opt.MapFrom(src => src.Details.Costs));
        CreateMap<InitialGridComponentDTO, InitialGridComponent>();
        CreateMap<CultureBoostComponentDTO, CultureBoostComponent>();
        CreateMap<LevelUpComponentDTO, LevelUpComponent>()
            .ForMember(dest => dest.StarLevels, opt => opt.MapFrom(src => src.StarLevels));
        CreateMap<CityCultureAreaComponentDTO, CityCultureAreaComponent>()
            .ForMember(dest => dest.Y, opt => opt.MapFrom(new CityCultureAreaYValueResolver()));

        // rewards
        CreateMap<DynamicActionChangeRewardDTO, DynamicActionChangeReward>();
        CreateMap<HeroRewardDto, HeroReward>();
        CreateMap<IncreaseExpansionRightRewardDTO, IncreaseExpansionRightReward>()
            .ForMember(dest => dest.CityId, opt => opt.ConvertUsing(new CityIdValueConverter(), rd => rd.City));
        CreateMap<LootContainerRewardDTO, LootContainerReward>()
            .ForMember(dest => dest.Rewards, opt => opt.MapFrom(r => r.PackedRewards));
        CreateMap<MysteryChestRewardDTO, MysteryChestReward>()
            .ForMember(dest => dest.Rewards, opt => opt.MapFrom(r => r.PackedRewards))
            .ForMember(dest => dest.Probabilities, opt => opt.MapFrom(r => r.RewardProbabilities));
        CreateMap<RegionRewardDto, RegionReward>().ConvertUsing<RegionRewardDtoConverter>();
        CreateMap<RepeatedField<Any>, IReadOnlyCollection<RewardBase>>().ConvertUsing<RewardDefinitionConverter>();
        CreateMap<ResourceRewardDTO, ResourceReward>();
        CreateMap<RepeatedField<Any>, IReadOnlyCollection<ResearchRewardBase>>()
            .ConvertUsing<ResearchRewardDtoConverter>();
        CreateMap<UnlockBuildingUpgradeRewardDTO, UnlockBuildingUpgradeReward>();
        CreateMap<IncreaseBuildingLimitRewardDTO, IncreaseBuildingLimitReward>();
        CreateMap<InstantUpgradeRewardDTO, InstantUpgradeReward>();
        CreateMap<UnlockAgeRewardDTO, UnlockAgeReward>();
        CreateMap<StorageCapRewardDTO, StorageCapReward>();
        CreateMap<HeroTreasureHuntUnlockDifficultyRewardDTO, HeroTreasureHuntUnlockDifficultyReward>();
        CreateMap<UnlockGoodRewardDTO, UnlockGoodReward>();
        CreateMap<UnlockFeatureRewardDTO, UnlockFeatureReward>();
        CreateMap<UnlockBuildingRewardDTO, UnlockBuildingReward>();

        // definitions
        CreateMap<AgeDefinitionDTO, Age>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(a => HohStringParser.GetConcreteId(a.Name)));
        CreateMap<ContinentDefinitionDTO, Continent>().ConvertUsing<ContinentDefinitionConverter>();
        CreateMap<EncounterDefinitionDTO, Encounter>().ConvertUsing<EncounterDefinitionDtoConverter>();
        CreateMap<RegionDefinitionDTO, Region>().ConvertUsing<RegionDefinitionConverter>();
        CreateMap<ResourceDefinitionDTO, Resource>()
            .ForMember(dest => dest.CityIds, opt => opt.ConvertUsing(new CityIdListValueConverter(), rd => rd.Cities))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(rd => rd.Type.ToResourceType()))
            .ForMember(
                dest => dest.Age, opt =>
                {
                    opt.PreCondition(rd => !string.IsNullOrWhiteSpace(rd.Age));
                    opt.ConvertUsing(new AgeValueConverter(), r => r.Age);
                });
        CreateMap<RewardDefinitionDTO, Reward>()
            .ForMember(dest => dest.Rewards, opt => opt.MapFrom(r => r.PackedRewards));
        CreateMap<TechnologyDefinitionDTO, Technology>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => HohStringParser.GetConcreteId(src.Id)))
            .ForMember(dest => dest.Age, opt => opt.ConvertUsing(new AgeValueConverter(), td => td.Age))
            .ForMember(dest => dest.CityId, opt => opt.ConvertUsing(new CityIdValueConverter(), td => td.City))
            .ForMember(dest => dest.ResearchComponent, opt => opt.MapFrom(src => src.ResearchComponent));
        CreateMap<WorldDefinitionDTO, World>().ConvertUsing<WorldDefinitionConverter>();
        CreateMap<BuildingDefinitionDTO, Building>()
            .ForMember(
                dest => dest.Age, opt =>
                {
                    opt.PreCondition(bd => !string.IsNullOrWhiteSpace(bd.Age));
                    opt.ConvertUsing(new AgeValueConverter(), bd => bd.Age);
                })
            .ForMember(dest => dest.CityIds, opt => opt.ConvertUsing(new CityIdListValueConverter(), bd => bd.Cities))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(bd => bd.Type.ToBuildingType()))
            .ForMember(dest => dest.Group, opt => opt.MapFrom(bd => bd.Subtype.ToBuildingSubtype()))
            .ForMember(dest => dest.Components, opt => opt.MapFrom(bd => bd.PackedComponents))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(bd => HohStringParser.GetConcreteId(bd.Id)));
        CreateMap<HeroBattleWaveDefinitionDTO, BattleWave>().ConvertUsing<HeroBattleWaveDefinitionDtoConverter>();
        CreateMap<HeroBattleDefinitionDTO, BattleDetails>().ConvertUsing<HeroBattleDefinitionDtoConverter>();
        CreateMap<HeroDefinitionDTO, Hero>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(hd => HohStringParser.GetConcreteId(hd.Id)))
            .ForMember(dest => dest.SupportUnitType,
                opt => opt.MapFrom(hd => HohStringParser.ParseEnumFromString<UnitType>(hd.SupportUnitType)))
            .ForMember(dest => dest.ClassId,
                opt => opt.MapFrom(hd => HohStringParser.ParseEnumFromString<HeroClassId>(hd.ClassId)));
        CreateMap<HeroBattleConstantsDefinitionDTO, UnitBattleConstants>()
            .ForMember(dest => dest.FormulaTypeToStatTypeMap, opt => opt.MapFrom(src =>
                src.StatToFormula.ToDictionary(
                    kvp => HohStringParser.ParseEnumFromString<UnitStatFormulaType>(kvp.Value),
                    kvp => HohStringParser.ParseEnumFromString<UnitStatType>(kvp.Key)
                )));
        CreateMap<HeroUnitStatFormulaDefinitionDTO, UnitStatFormulaData>()
            .ForMember(dest => dest.Type,
                opt => opt.MapFrom(src => HohStringParser.ParseEnumFromString<UnitStatFormulaType>(src.Id)))
            .ForMember(dest => dest.BaseFactor, opt => opt.MapFrom(src => src.Unit.Normal))
            .ForMember(dest => dest.RarityFactors,
                opt => opt.MapFrom(src =>
                    src.RarityUnits.ToDictionary(dto => dto.RarityId, dto => dto.Factors)));
        CreateMap<BattleAbilityDefinitionDTO, HeroAbility>();
        CreateMap<ReworkedWonderDefinitionDTO, Wonder>()
            .ForMember(dest => dest.Id, opt => opt.ConvertUsing(new WonderIdValueConverter(), src => src.Id))
            .ForMember(dest => dest.CityId, opt => opt.ConvertUsing(new CityIdValueConverter(), src => src.CityId))
            .ForMember(dest => dest.Components, opt => opt.MapFrom(src => src.PackedLevels));
        CreateMap<CityDefinitionDTO, CityDefinition>()
            .ForMember(dest => dest.Id, opt => opt.ConvertUsing(new CityIdValueConverter(), src => src.Id))
            .ForMember(dest => dest.BuildMenuTypes,
                opt => opt.ConvertUsing(new BuildMenuTypesConverter(), src => src.BuildingMenuTypes))
            .ForMember(dest => dest.InitConfigs, opt => opt.MapFrom(src => src.InitDefinition))
            .ForMember(dest => dest.Components, opt => opt.MapFrom(bd => bd.PackedComponents));
        CreateMap<CityInitDefinitionDTO, CityInitConfigs>()
            .ForMember(dest => dest.Grid, opt => opt.MapFrom(src => src.InitialGridComponent));
        CreateMap<BuildingCustomizationDefinitionDTO, BuildingCustomization>()
            .ForMember(dest => dest.CityId, opt => opt.ConvertUsing(new CityIdValueConverter(), src => src.CityId))
            .ForMember(dest => dest.BuildingGroup, opt => opt.MapFrom(src => src.Subtype.ToBuildingSubtype()))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration.Seconds))
            .ForMember(dest => dest.Components, opt => opt.MapFrom(bd => bd.PackedComponents));
        CreateMap<HeroUnitTypeDefinitionDTO, HeroUnitType>()
            .ForMember(dest => dest.UnitType,
                opt => opt.MapFrom(src => HohStringParser.ParseEnumFromString<UnitType>(src.UnitType)))
            .ForMember(dest => dest.BaseValues, opt => opt.MapFrom(src => src.UnitStats));

        // localization
        CreateMap<LocaResponse, LocalizationData>()
            .ForMember(dest => dest.Entries, opt => opt.MapFrom(src =>
                src.Entries.ToDictionary(entry => entry.Key, entry => entry.Values)));
    }
}
