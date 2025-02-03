using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.Providers.Interfaces;

public interface IHohHeroLevelSpecsProvider
{
    IReadOnlyCollection<HeroLevelSpecs> Get(int maxLevel);
}
