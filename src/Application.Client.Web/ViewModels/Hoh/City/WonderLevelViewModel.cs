namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;

public class WonderLevelViewModel
{
    public static readonly WonderLevelViewModel Blank = new()
    {
        Cost = [],
        CumulativeCost = [],
        Level = 0,
    };

    public required IReadOnlyCollection<IconLabelItemViewModel> Cost { get; init; }
    public required IReadOnlyCollection<IconLabelItemViewModel> CumulativeCost { get; init; }
    public required int Level { get; init; }
}
