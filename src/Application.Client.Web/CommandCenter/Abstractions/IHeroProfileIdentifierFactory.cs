using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;

public interface IHeroProfileIdentifierFactory
{
    HeroProfileIdentifier Create(string heroId);
    HeroProfileIdentifier Create(string heroId, int barracksLevel);
    HeroProfileIdentifier Create(string heroId, IBattleUnitProperties battleUnit, int barracksLevel);
    HeroProfileIdentifier Create(HeroProfileIdentifier identifier, HeroLevelSpecs levelSpecs);
}
