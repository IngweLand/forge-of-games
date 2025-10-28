namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Relic;

public class RelicInsightsViewModel
{
    public required string LevelRange { get; init; }
    public required IReadOnlyCollection<IconLabelItemViewModel> Relics { get; init; }
}
