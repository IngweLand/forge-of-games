namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Research;

public class ResearchCostViewModel
{
    public required IReadOnlyCollection<IconLabelItemViewModel> Cost { get; init; }

    public  IReadOnlyCollection<ResearchCalculatorTechnologyViewModel> FromTechnologies { get; init; } =
        new List<ResearchCalculatorTechnologyViewModel>();
    public  IReadOnlyCollection<ResearchCalculatorTechnologyViewModel> ToTechnologies { get; init; } =
        new List<ResearchCalculatorTechnologyViewModel>();
}
