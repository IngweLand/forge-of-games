using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class EquipmentInsightsViewModelFactory(IAssetUrlProvider assetUrlProvider) : IEquipmentInsightsViewModelFactory
{
    public EquipmentInsightsViewModel Create(EquipmentInsightsDto dto,
        IReadOnlyDictionary<StatAttribute, string> statAttributes,
        IReadOnlyDictionary<EquipmentSet, string> sets)
    {
        string levelRange;
        if (dto.FromLevel == 0)
        {
            levelRange = $"<{dto.ToLevel}";
        }
        else if (dto.ToLevel == int.MaxValue)
        {
            levelRange = $"{dto.FromLevel}+";
        }
        else
        {
            levelRange = $"{dto.FromLevel}-{dto.ToLevel}";
        }

        return new EquipmentInsightsViewModel
        {
            LevelRange = levelRange,
            EquipmentSlotType = dto.EquipmentSlotType,
            EquipmentSets = dto.EquipmentSets.Select(x =>
                {
                    var name = sets.TryGetValue(x, out var set) ? set : x.ToString();
                    return new IconLabelItemViewModel()
                    {
                        IconUrl = assetUrlProvider.GetHohEquipmentSetIconUrl(x),
                        Label = name,
                    };
                })
                .ToList(),
            MainAttributes = dto.MainAttributes.Select(x => CreateAttribute(x, statAttributes)).ToList(),
            SubAttributesLevel4 = dto.SubAttributesLevel4.Select(x => CreateAttribute(x, statAttributes)).ToList(),
            SubAttributesLevel8 = dto.SubAttributesLevel8.Select(x => CreateAttribute(x, statAttributes)).ToList(),
            SubAttributesLevel12 = dto.SubAttributesLevel12.Select(x => CreateAttribute(x, statAttributes)).ToList(),
            SubAttributesLevel16 = dto.SubAttributesLevel16.Select(x => CreateAttribute(x, statAttributes)).ToList(),
        };
    }

    private IconLabelItemViewModel CreateAttribute(StatAttribute statAttribute,
        IReadOnlyDictionary<StatAttribute, string> statAttributes)
    {
        var name = statAttributes.TryGetValue(statAttribute, out var set) ? set : statAttribute.ToString();
        return new IconLabelItemViewModel()
        {
            IconUrl = assetUrlProvider.GetHohStatAttributeIconUrl(statAttribute),
            Label = name,
        };
    }
}
