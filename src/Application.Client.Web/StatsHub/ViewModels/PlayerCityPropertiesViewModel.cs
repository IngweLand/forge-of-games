using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

public class PlayerCityPropertiesViewModel
{
    public IconLabelItemViewModel? Coins { get; init; }
    public IconLabelItemViewModel? Food { get; init; }
    public IconLabelItemViewModel? Goods { get; init; }
    public IconLabelItemViewModel? TotalPremiumExpansionCost { get; init; }
    public IconLabelItemViewModel? PremiumExpansionCount { get; init; }
}
