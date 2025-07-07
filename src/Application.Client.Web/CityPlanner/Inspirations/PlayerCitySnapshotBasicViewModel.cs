using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Inspirations;

public class PlayerCitySnapshotBasicViewModel
{
    public required CityId CityId { get; set; }
    public required AgeViewModel Age { get; set; }
    public IconLabelItemViewModel? Coins { get; set; }
    public IconLabelItemViewModel? Food { get; set; }
    public IconLabelItemViewModel? Goods { get; set; }
    public IconLabelItemViewModel? Premium { get; set; }
    public int Id { get; set; }
    public required string PlayerName { get; init; }
}
