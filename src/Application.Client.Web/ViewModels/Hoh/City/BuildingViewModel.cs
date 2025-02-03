using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Dtos.Hoh.City;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;

public class BuildingViewModel
{
    public string? AgeColor { get; init; }
    public string? AgeName { get; init; }
    public ConstructionComponentViewModel? ConstructionComponent { get; init; }
    public required BuildingDto Data { get; init; }
    public required string Id { get; init; }
    public required string Size { get; init; }
    public int Level { get; init; }
    public required string Name { get; init; }
    public UpgradeComponentViewModel? UpgradeComponent { get; init; }
}
