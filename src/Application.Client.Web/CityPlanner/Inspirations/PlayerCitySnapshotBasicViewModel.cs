using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Inspirations;

public class PlayerCitySnapshotBasicViewModel
{
    public required AgeViewModel Age { get; init; }
    public required CityId CityId { get; init; }
    public IconLabelItemViewModel? Coins { get; init; }
    public IconLabelItemViewModel? Food { get; init; }
    public IconLabelItemViewModel? Goods { get; init; }
    public required IconLabelItemViewModel HappinessUsageRatio { get; init; }
    public int Id { get; init; }
    public required string PlayerName { get; init; }
    public IconLabelItemViewModel? Premium { get; init; }
    public required IconLabelItemViewModel TotalArea { get; init; }
}
