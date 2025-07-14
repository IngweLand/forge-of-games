using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Units;

namespace Ingweland.Fog.Application.Core.Factories.Interfaces;

public interface IHeroBasicDtoFactory
{
    HeroBasicDto Create(Hero hero, Unit unit);
}
