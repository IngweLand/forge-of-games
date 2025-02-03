namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Research;

public class AgeTechnologiesViewModel
{
    public required string AgeColor { get; init; }
    public required string AgeName { get; init; }
    public bool IsListOpen { get; set; }
    public required IReadOnlyCollection<ResearchCalculatorTechnologyViewModel> Technologies { get; init; }
}
