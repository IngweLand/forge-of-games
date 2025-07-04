using System.Globalization;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Core.CityPlanner.Stats;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

public class HappinessStatsViewModelFactory(IAssetUrlProvider assetUrlProvider) : IHappinessStatsViewModelFactory
{
    public HappinessStatsViewModel Create(CityStats stats)
    {
        return new HappinessStatsViewModel()
        {
            ExcessHappiness = new IconLabelItemViewModel()
            {
                Label = stats.ExcessHappiness.ToString("N0"),
                IconUrl = assetUrlProvider.GetHohIconUrl("icon_flat_culture_boost"),
            },
            TotalAvailableHappiness = new IconLabelItemViewModel()
            {
                Label = stats.TotalAvailableHappiness.ToString("N0"),
                IconUrl = assetUrlProvider.GetHohIconUrl("icon_flat_culture_boost"),
            },
            TotalHappinessConsumption = new IconLabelItemViewModel()
            {
                Label = stats.TotalHappinessConsumption.ToString("N0"),
                IconUrl = assetUrlProvider.GetHohIconUrl("icon_flat_culture_boost"),
            },
            UnmetHappinessNeed = new IconLabelItemViewModel()
            {
                Label = stats.UnmetHappinessNeed.ToString("N0"),
                IconUrl = assetUrlProvider.GetHohIconUrl("icon_flat_culture_boost"),
            },
            UsageRatio = new IconLabelItemViewModel()
            {
                Label = stats.HappinessUsageRatio.ToString(CultureInfo.CurrentCulture),
                IconUrl = assetUrlProvider.GetHohIconUrl("icon_flat_culture_boost"),
            },
        };
    }
}
