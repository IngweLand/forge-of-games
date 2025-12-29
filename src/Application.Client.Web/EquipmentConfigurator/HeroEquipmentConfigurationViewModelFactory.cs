using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator;

public class HeroEquipmentConfigurationViewModelFactory(
    IAssetUrlProvider assetUrlProvider,
    IIconLabelItemViewModelFactory iconLabelItemViewModelFactory)
    : IHeroEquipmentConfigurationViewModelFactory
{
    public HeroEquipmentConfigurationViewModel Create(HeroEquipmentConfiguration srcConfiguration,
        IReadOnlyDictionary<int, EquipmentItemViewModel> equipment,
        IReadOnlyDictionary<StatAttribute, string> statAttributes, IReadOnlyDictionary<EquipmentSet, string> sets)
    {
        return new HeroEquipmentConfigurationViewModel
        {
            Id = srcConfiguration.Id,
            Hand = srcConfiguration.HandId != null
                ? CreateItem(srcConfiguration.HandId.Value, equipment, statAttributes, sets)
                : null,
            Garment = srcConfiguration.GarmentId != null
                ? CreateItem(srcConfiguration.GarmentId.Value, equipment, statAttributes, sets)
                : null,
            Hat = srcConfiguration.HatId != null
                ? CreateItem(srcConfiguration.HatId.Value, equipment, statAttributes, sets)
                : null,
            Neck = srcConfiguration.NeckId != null
                ? CreateItem(srcConfiguration.NeckId.Value, equipment, statAttributes, sets)
                : null,
            Ring = srcConfiguration.RingId != null
                ? CreateItem(srcConfiguration.RingId.Value, equipment, statAttributes, sets)
                : null,
            IsInGame = srcConfiguration.IsInGame,
        };
    }

    private HeroEquipmentItemViewModel CreateItem(int id, IReadOnlyDictionary<int, EquipmentItemViewModel> equipment,
        IReadOnlyDictionary<StatAttribute, string> statAttributes, IReadOnlyDictionary<EquipmentSet, string> sets)
    {
        var concreteEquipment = equipment[id];
        var setName = sets.TryGetValue(concreteEquipment.EquipmentSet, out var set)
            ? set
            : concreteEquipment.EquipmentSet.ToString();
        return new HeroEquipmentItemViewModel
        {
            EquipmentItemViewModel = concreteEquipment,
            EquipmentSet = new IconLabelItemViewModel
            {
                IconUrl = assetUrlProvider.GetHohEquipmentSetIconUrl(concreteEquipment.EquipmentSet),
                Label = setName,
            },
            Attributes =
                new List<IconLabelItemViewModel>
                    {
                        iconLabelItemViewModelFactory.CreateEquipmentAttribute(
                            concreteEquipment.MainAttribute.StatAttribute,
                            statAttributes),
                    }.Concat(concreteEquipment.SubAttributes
                        .Where(x => x.Value != null)
                        .OrderBy(x => x.Value!.UnlockedAtLevel)
                        .Select(x => iconLabelItemViewModelFactory.CreateEquipmentAttribute(x.Key, statAttributes)))
                    .ToList(),
        };
    }
}
