namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;

public class BuildingLevelSpecs
{
    public static readonly BuildingLevelSpecs ZeroLevel = new()
    {
        Level = 0,
        CanBeConstructed = false,
        CanBeUpgradedTo = false,
    };

    public string? AgeColor { get; init; }
    public string? AgeName { get; init; }
    public required bool CanBeConstructed { get; init; }
    public required bool CanBeUpgradedTo { get; init; }
    public required int Level { get; init; }
}
