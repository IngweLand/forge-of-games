using AutoMapper;
using Ingweland.Fog.Application.Server.Helpers;
using Ingweland.Fog.Application.Server.Mapping.Hoh.Converters;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh;

public class EquipmentMappingProfile : Profile
{
    public EquipmentMappingProfile()
    {
        CreateMap<EquipmentItemDto, EquipmentItem>()
            .ForMember(dest => dest.EquippedOnHero, opt =>
            {
                opt.PreCondition(src => !string.IsNullOrWhiteSpace(src.EquippedOnHeroDefinitionId));
                opt.MapFrom(src => StringParser.GetConcreteId(src.EquippedOnHeroDefinitionId));
            })
            .ForMember(dest => dest.EquipmentSlotType,
                opt => opt.MapFrom(src =>
                    StringParser.ParseEnumFromString<EquipmentSlotType>(src.EquipmentSlotTypeDefinitionId)))
            .ForMember(dest => dest.EquipmentRarity,
                opt => opt.ConvertUsing<EquipmentRarityValueConverter, string>(src => src.EquipmentRarityDefinitionId))
            .ForMember(dest => dest.EquipmentSet,
                opt => opt.MapFrom(src =>
                    StringParser.ParseEnumFromString<EquipmentSet>(src.EquipmentSetDefinitionId)));
        
        CreateMap<EquipmentStatBoostDto, EquipmentStatBoost>()
            .ForMember(dest => dest.UnitStatType,
                opt => opt.MapFrom(src => StringParser.ParseEnumFromString<UnitStatType>(src.UnitStatDefinitionId)))
            .ForMember(dest => dest.StatAttribute,
                opt => opt.MapFrom(src => StringParser.ParseEnumFromString<StatAttribute>(src.UnitStatAttributeDefinitionId)))
            .ForMember(dest => dest.Calculation, opt => opt.MapFrom(src => src.Calculation));
        
        CreateMap<EquipmentAttributeDto, EquipmentAttribute>()
            .ForMember(dest => dest.StatAttribute,
                opt => opt.MapFrom(src =>
                    StringParser.ParseEnumFromString<StatAttribute>(src.UnitStatAttributeDefinitionId)));
    }
}