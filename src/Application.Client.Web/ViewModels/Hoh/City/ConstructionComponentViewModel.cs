namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;

public class ConstructionComponentViewModel
{
    public required string BuildTime { get; init; }

    public required IReadOnlyCollection<IconLabelItemViewModel> Cost { get; init; }

    public int WorkerCount { get; init; }
}
