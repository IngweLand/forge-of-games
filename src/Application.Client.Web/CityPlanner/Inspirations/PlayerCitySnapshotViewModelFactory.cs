using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Dtos.Hoh.PlayerCity;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Inspirations;

public class PlayerCitySnapshotViewModelFactory(IAssetUrlProvider assetUrlProvider)
    : IPlayerCitySnapshotViewModelFactory
{
    public PlayerCitySnapshotBasicViewModel Create(PlayerCitySnapshotBasicDto snapshot, AgeViewModel age)
    {
        return new PlayerCitySnapshotBasicViewModel
        {
            Id = snapshot.Id,
            CityId = snapshot.CityId,
            Age = age,
            PlayerName = snapshot.PlayerName,
            Coins = new IconLabelItemViewModel
            {
                Label = snapshot.Coins.ToString("N0"),
                IconUrl = assetUrlProvider.GetHohIconUrl("icon_coins"),
            },
            Food = new IconLabelItemViewModel
            {
                Label = snapshot.Food.ToString("N0"),
                IconUrl = assetUrlProvider.GetHohIconUrl("icon_food"),
            },
            Goods = new IconLabelItemViewModel
            {
                Label = snapshot.Goods.ToString("N0"),
                IconUrl = assetUrlProvider.GetHohIconUrl("icon_good_storage"),
            },
            Premium = snapshot.HasPremiumBuildings
                ? new IconLabelItemViewModel
                {
                    Label = string.Empty,
                    IconUrl = assetUrlProvider.GetHohIconUrl("icon_premium"),
                }
                : null,
        };
    }
}
