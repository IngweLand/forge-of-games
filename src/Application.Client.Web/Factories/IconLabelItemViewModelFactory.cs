using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class IconLabelItemViewModelFactory(IAssetUrlProvider assetUrlProvider) : IIconLabelItemViewModelFactory
{
    public IconLabelItemViewModel CreateEquipmentAttribute(StatAttribute statAttribute,
        IReadOnlyDictionary<StatAttribute, string> statAttributes)
    {
        var name = statAttributes.TryGetValue(statAttribute, out var set) ? set : statAttribute.ToString();
        return new IconLabelItemViewModel
        {
            IconUrl = assetUrlProvider.GetHohStatAttributeIconUrl(statAttribute),
            Label = name,
        };
    }
}
