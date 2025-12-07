using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Dtos.Hoh.PlayerCity;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.Factories;

public class PlayerProductionCapacityViewModelFactory(IAssetUrlProvider assetUrlProvider)
    : IPlayerProductionCapacityViewModelFactory
{
    public PlayerProductionCapacityViewModel Create(PlayerProductionCapacityDto src)
    {
        return new PlayerProductionCapacityViewModel
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
        };
    }
}
