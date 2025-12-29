using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class SquadEquipmentItemViewModelFactory(
    IAssetUrlProvider assetUrlProvider,
    IEquipmentSlotTypeIconUrlProvider equipmentSlotTypeIconUrlProvider,
    IIconLabelItemViewModelFactory iconLabelItemViewModelFactory) : ISquadEquipmentItemViewModelFactory
{
    public SquadEquipmentItemViewModel Create(SquadEquipmentItem src,
        IReadOnlyDictionary<StatAttribute, string> statAttributes,
        IReadOnlyDictionary<string, string> sets)
    {
        var setName = sets.TryGetValue(src.EquipmentSet.ToString(), out var set) ? set : src.EquipmentSet.ToString();
        return new SquadEquipmentItemViewModel
        {
            EquipmentSlotType = src.EquipmentSlotType,
            EquipmentSlotTypeIconUrl = equipmentSlotTypeIconUrlProvider.GetIconUrl(src.EquipmentSlotType),
            EquipmentSet = new IconLabelItemViewModel
            {
                IconUrl = assetUrlProvider.GetHohEquipmentSetIconUrl(src.EquipmentSet),
                Label = setName,
            },
            Attributes =
                new List<IconLabelItemViewModel>
                {
                    iconLabelItemViewModelFactory.CreateEquipmentAttribute(src.MainAttribute.StatAttribute,
                        statAttributes),
                }.Concat(src.SubAttributes.Where(x => x.Unlocked).OrderBy(x => x.UnlockedAtLevel).Select(x =>
                    iconLabelItemViewModelFactory.CreateEquipmentAttribute(x.StatAttribute, statAttributes))).ToList(),
        };
    }
}
