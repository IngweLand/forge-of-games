namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public class RegionViewModel
{
    public required IReadOnlyCollection<EncounterViewModel> Encounters { get; init; }
    public required string Name { get; init; }
    public required IReadOnlyCollection<IconLabelItemViewModel> Rewards { get; init; }
}
