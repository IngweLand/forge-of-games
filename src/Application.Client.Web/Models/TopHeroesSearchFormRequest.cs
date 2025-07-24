using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.Models;

public record TopHeroesSearchFormRequest
{
    public AgeViewModel? Age { get; set; }
    public HeroLevelRange? LevelRange { get; set; }
}
