using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

public class CityMapEntityViewModel
{
    public AgeViewModel? Age { get; init; }
    public CustomizationComponentViewModel? CustomizationComponent { get; init; }
    public required int Id { get; init; }

    public IReadOnlyCollection<IconLabelItemViewModel> InfoItems { get; init; } =
        new List<IconLabelItemViewModel>();

    public bool IsLockable { get; init; }
    public bool IsLocked { get; init; }

    public int Level { get; init; }

    public required IReadOnlyCollection<BuildingLevelSpecs> Levels { get; init; }
    public required string Name { get; init; }

    public ProductionComponentViewModel? ProductionComponent { get; init; }
    public required string Size { get; init; }
}
