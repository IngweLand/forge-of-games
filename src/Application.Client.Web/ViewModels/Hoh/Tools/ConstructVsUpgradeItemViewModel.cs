namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Tools;

public class ConstructVsUpgradeItemViewModel
{
    public required string IconUrl { get; init; }
    public required string ConstructionCost { get; init; } = "-";
    public required string UpgradeCost { get; init; } = "-";
}
