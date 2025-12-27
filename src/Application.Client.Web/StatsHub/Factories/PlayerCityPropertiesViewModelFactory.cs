using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Dtos.Hoh.PlayerCity;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.Factories;

public class PlayerCityPropertiesViewModelFactory(IAssetUrlProvider assetUrlProvider)
    : IPlayerCityPropertiesViewModelFactory
{
    public PlayerCityPropertiesViewModel Create(PlayerCityPropertiesDto src)
    {
        IconLabelItemViewModel? premiumExpansions = null;
        IconLabelItemViewModel? totalPremiumExpansionCost = null;
        if (src.PremiumExpansionCount > 0)
        {
            premiumExpansions = new IconLabelItemViewModel
            {
                Label = src.PremiumExpansionCount.ToString("N0"),
                IconUrl = assetUrlProvider.GetHohIconUrl("icon_flat_expansion"),
            };

            totalPremiumExpansionCost = new IconLabelItemViewModel
            {
                Label = src.TotalPremiumExpansionCost.ToString("N0"),
                IconUrl = assetUrlProvider.GetHohIconUrl("icon_premium"),
            };
        }

        return new PlayerCityPropertiesViewModel
        {
            Coins = new IconLabelItemViewModel
            {
                Label = src.Coins.ToString("N0"),
                IconUrl = assetUrlProvider.GetHohIconUrl("icon_coins"),
            },
            Food = new IconLabelItemViewModel
            {
                Label = src.Food.ToString("N0"),
                IconUrl = assetUrlProvider.GetHohIconUrl("icon_food"),
            },
            Goods = new IconLabelItemViewModel
            {
                Label = src.Goods.ToString("N0"),
                IconUrl = assetUrlProvider.GetHohIconUrl("icon_good_storage"),
            },
            PremiumExpansionCount = premiumExpansions,
            TotalPremiumExpansionCost = totalPremiumExpansionCost,
        };
    }
}
