using AutoMapper;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace HohProtoParser.Converters;

public class ComponentDtoConverter : ITypeConverter<RepeatedField<Any>, IList<ComponentBase>>
{
    public IList<ComponentBase> Convert(RepeatedField<Any> source,
        IList<ComponentBase> destination, ResolutionContext context)
    {
        var heroBuildingBoostComponents =
            (IDictionary<string, HeroBuildingBoostComponentDTO>) context.Items[
                ContextKeys.HERO_BUILDING_BOOST_COMPONENTS];
        var heroAbilityTrainingComponent =
            (IDictionary<string, HeroAbilityTrainingComponentDTO>) context.Items[
                ContextKeys.HERO_ABILITY_TRAINING_COMPONENTS];
        var list = new List<ComponentBase>();
        var mapper = context.Mapper;
        foreach (var any in source)
        {
            var componentsToSkip = new List<string>()
            {
                "InitComponentDTO", "MoveComponentDTO", "SellComponentDTO", "PinnedAgeComponentDTO",
                "SubscriptionSlotComponentDTO", "RebuildConstructionComponentDTO",
                "OriginComponentDTO",
            };
            if (componentsToSkip.Any(s => any.TypeUrl.EndsWith(s)))
            {
                continue;
            }

            if (any.Is(UpgradeComponentDTO.Descriptor))
            {
                list.Add(mapper.Map<UpgradeComponent>(any.Unpack<UpgradeComponentDTO>()));
            }
            else if (any.Is(ProductionComponentDTO.Descriptor))
            {
                list.Add(mapper.Map<ProductionComponent>(any.Unpack<ProductionComponentDTO>()));
            }
            else if (any.Is(ConstructionComponentDTO.Descriptor))
            {
                list.Add(mapper.Map<ConstructionComponent>(any.Unpack<ConstructionComponentDTO>()));
            }
            else if (any.Is(GrantWorkerComponentDTO.Descriptor))
            {
                list.Add(mapper.Map<GrantWorkerComponent>(any.Unpack<GrantWorkerComponentDTO>()));
            }
            else if (any.Is(CultureComponentDTO.Descriptor))
            {
                list.Add(mapper.Map<CultureComponent>(any.Unpack<CultureComponentDTO>()));
            }
            else if (any.Is(BuildingUnitProviderComponentDTO.Descriptor))
            {
                list.Add(mapper.Map<BuildingUnitProviderComponent>(any.Unpack<BuildingUnitProviderComponentDTO>()));
            }
            else if (any.Is(BoostResourceComponentDTO.Descriptor))
            {
                list.Add(mapper.Map<BoostResourceComponent>(any.Unpack<BoostResourceComponentDTO>()));
            }
            else if (any.Is(CultureBoostComponentDTO.Descriptor))
            {
                list.Add(mapper.Map<CultureBoostComponent>(any.Unpack<CultureBoostComponentDTO>()));
            }
            else if (any.Is(LevelUpComponentDTO.Descriptor))
            {
                list.Add(mapper.Map<LevelUpComponent>(any.Unpack<LevelUpComponentDTO>()));
            }
            else if (any.Is(GameDesignReference.Descriptor))
            {
                var gdr = any.Unpack<GameDesignReference>();
                if (Any.GetTypeName(gdr.Type) == "HeroBuildingBoostComponentDTO")
                {
                    list.Add(mapper.Map<HeroBuildingBoostComponent>(heroBuildingBoostComponents[gdr.Id]));
                }
                else if (Any.GetTypeName(gdr.Type) == "HeroAbilityTrainingComponentDTO")
                {
                    list.Add(mapper.Map<HeroAbilityTrainingComponent>(heroAbilityTrainingComponent[gdr.Id]));
                }
                else
                {
                    throw new Exception($"Unknown GameDesignReference type: {gdr.Type}");
                }
            }
            else
            {
                throw new Exception($"Unknown component type: {any.TypeUrl}");
            }
        }

        return list;
    }
}
