using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

public class TopHeroesSearchFormViewModel
{
    public required IReadOnlyCollection<AgeViewModel> Ages { get; init; }
    public required IReadOnlyCollection<HeroLevelRange> LevelRanges { get; init; }
    public required IReadOnlyCollection<HeroInsightsModeViewModel> Modes { get; init; }
}
