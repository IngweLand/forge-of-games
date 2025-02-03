using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;

public class HeroLevelViewModel
{
    public required IReadOnlyCollection<IconLabelItemViewModel>? Cost { get; init; }
    public required IReadOnlyCollection<IconLabelItemViewModel> Stats { get; init; }
    public required string Title { get; init; }

    public required HeroLevelSpecs LevelSpecs { get; init; }
}
