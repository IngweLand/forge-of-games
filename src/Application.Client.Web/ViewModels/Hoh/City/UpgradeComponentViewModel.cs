namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;

public class UpgradeComponentViewModel
{
    public required IReadOnlyCollection<IconLabelItemViewModel> Cost { get; init; }
    public required string UpgradeTime { get; init; }

    public int WorkerCount { get; init; }
}
