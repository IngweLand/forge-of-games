using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Battle;

namespace Ingweland.Fog.Application.Core.Factories.Interfaces;

public interface IRegionDtoFactory
{
    RegionDto Create(Region region, IReadOnlyCollection<UnitDto> units, IReadOnlyCollection<HeroDto> heroes);
}
