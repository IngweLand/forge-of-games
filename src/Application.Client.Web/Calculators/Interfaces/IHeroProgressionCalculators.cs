using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Units;

namespace Ingweland.Fog.Application.Client.Web.Calculators.Interfaces;

public interface IHeroProgressionCalculators
{
    int CalculateDependentCost(HeroProgressionCostResource src);

    IReadOnlyCollection<ResourceAmount> CalculateProgressionCost(HeroDto hero, HeroLevelSpecs from,
        HeroLevelSpecs to);
}
