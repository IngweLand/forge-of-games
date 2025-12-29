using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;
using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.ViewModels;
using Ingweland.Fog.Application.Client.Web.Factories;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator;

public class EquipmentItemViewModel2Factory(
    IEquipmentItemAttributeViewModelFactory attributeViewModelFactory,
    IAssetUrlProvider assetUrlProvider) : IEquipmentItemViewModel2Factory
{
    public EquipmentItemViewModel2 CreateItem(EquipmentItem equipmentItem, IEnumerable<HeroBasicViewModel> heroes,
        IReadOnlyDictionary<string, string> setNames, IReadOnlyDictionary<StatAttribute, string> statAttributesNames)
    {
        var setName = setNames.TryGetValue(equipmentItem.EquipmentSet.ToString(), out var set)
            ? set
            : equipmentItem.EquipmentSet.ToString();
        var concreteSetName = setNames
            .TryGetValue($"{equipmentItem.EquipmentSet}_{equipmentItem.EquipmentSlotType}", out var concreteSet)
            ? concreteSet
            : $"{equipmentItem.EquipmentSet} {equipmentItem.EquipmentSlotType}";
        var vm = new EquipmentItemViewModel2
        {
            Id = equipmentItem.Id,
            Label = concreteSetName,
            IconUrl = assetUrlProvider.GetHohEquipmentIconUrl(equipmentItem.EquipmentSet,
                equipmentItem.EquipmentSlotType),
            EquipmentSet = new IconLabelItemViewModel
            {
                IconUrl = assetUrlProvider.GetHohEquipmentSetIconUrl(equipmentItem.EquipmentSet),
                Label = setName,
            },
            MainAttribute = attributeViewModelFactory.CreateAttribute(equipmentItem.MainAttribute, statAttributesNames),
            SubAttributes = equipmentItem.SubAttributes.OrderBy(x => x.UnlockedAtLevel)
                .Select(x => attributeViewModelFactory.CreateSubAttribute(x, statAttributesNames)).ToList(),
            EquipmentItem = equipmentItem,
            EquippedOnHeroes = new HashSet<HeroBasicViewModel>(heroes),
        };
        
        return vm;
    }
}
