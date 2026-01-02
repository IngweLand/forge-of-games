using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Application.Client.Web.Factories;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;

public class EquipmentItemToViewModelConverter(
    IEquipmentItemAttributeViewModelFactory attributeViewModelFactory,
    IAssetUrlProvider assetUrlProvider,
    IEquipmentSlotTypeIconUrlProvider equipmentSlotTypeIconUrlProvider)
    : ITypeConverter<EquipmentItem, EquipmentItemViewModel>
{
    

    public EquipmentItemViewModel Convert(EquipmentItem source, EquipmentItemViewModel destination,
        ResolutionContext context)
    {
        var heroAssetId = source.EquippedOnHero != null ? $"Unit_{source.EquippedOnHero}" : null;

        var subAttributes = attributeViewModelFactory.StatToNumericValueTypeMap.Keys.ToDictionary(statAttribute => statAttribute,
            statAttribute => attributeViewModelFactory.CreateSubAttribute(source.SubAttributes, statAttribute));
        return new EquipmentItemViewModel
        {
            Id = source.Id,
            EquipmentSlotType = source.EquipmentSlotType,
            EquipmentSet = source.EquipmentSet,
            Level = source.Level,
            StarCount = source.EquipmentRarity.ToStarCount(),
            EquippedOnHero = source.EquippedOnHero,
            EquippedOnHeroPortraitUrl =
                heroAssetId != null ? assetUrlProvider.GetHohUnitPortraitUrl(heroAssetId) : null,
            EquipmentSetIconUrl = assetUrlProvider.GetHohEquipmentSetIconUrl(source.EquipmentSet),
            EquipmentSlotTypeIconUrl = equipmentSlotTypeIconUrlProvider.GetIconUrl(source.EquipmentSlotType),
            MainAttribute = attributeViewModelFactory.CreateAttribute(source.MainAttribute),
            SubAttributes = subAttributes,
        };
    }

    
}
