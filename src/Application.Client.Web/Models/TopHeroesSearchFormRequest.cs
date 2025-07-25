using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.Models;

public record TopHeroesSearchFormRequest
{
    public AgeViewModel? Age { get; init; }
    public HeroLevelRange? LevelRange { get; init; }
    public required HeroInsightsModeViewModel Mode { get; init; }
}
