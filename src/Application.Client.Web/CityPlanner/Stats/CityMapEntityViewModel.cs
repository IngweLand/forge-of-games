using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

public class CityMapEntityViewModel
{
    public AgeViewModel? Age { get; init; }
    public required int Id { get; init; }

    public IReadOnlyCollection<IconLabelItemViewModel> InfoItems { get; init; } =
        new List<IconLabelItemViewModel>();

    public int Level { get; init; }

    public required IReadOnlyCollection<BuildingLevelSpecs> Levels { get; init; }
    public required string Name { get; init; }

    public ProductionComponentViewModel? ProductionComponent { get; init; }
    public CustomizationComponentViewModel? CustomizationComponent { get; init; }
    public required string Size { get; init; }
}
