using Ingweland.Fog.Application.Client.Web.Calculators.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Units;

namespace Ingweland.Fog.Application.Client.Web.Calculators;

public class HeroProgressionCalculators : IHeroProgressionCalculators
{
    public int CalculateDependentCost(HeroProgressionCostResource src)
    {
        return (int) Math.Round(src.Amount * src.ResourceFactor);
    }

    public IReadOnlyCollection<ResourceAmount> CalculateProgressionCost(HeroDto hero, HeroLevelSpecs from,
        HeroLevelSpecs to)
    {
        var progressionCost = new List<ResourceAmount>();
        if (to.Level > from.Level)
        {
            var i = from.Level - 1;
            progressionCost = hero.ProgressionCosts
                .Skip(i)
                .Take(to.Level - i - 1)
                .SelectMany(src => new[]
                {
                    new ResourceAmount()
                    {
                        ResourceId = "resource.hero_xp",
                        Amount = src.Amount,
                    },
                    new ResourceAmount()
                    {
                        ResourceId = src.ResourceId,
                        Amount = CalculateDependentCost(src),
                    },
                })
                .ToList();
        }

        const int ascensionPeriod = 10;
        IEnumerable<ResourceAmount> ascensionCost = [];
        var firstAscension = from.Level / ascensionPeriod;
        if (from.IsAscension)
        {
            firstAscension--;
        }

        var lastAscension = to.Level / ascensionPeriod;
        if (to.IsAscension)
        {
            lastAscension--;
        }

        for (var i = firstAscension; i < lastAscension; i++)
        {
            if (hero.AscensionCosts.TryGetValue(i, out var ac))
            {
                ascensionCost = ascensionCost.Concat(ac);
            }
        }

        var total = progressionCost.Concat(ascensionCost);

        return total
            .GroupBy(ra => ra.ResourceId)
            .Select(g => new ResourceAmount() {ResourceId = g.Key, Amount = g.Sum(ra => ra.Amount)})
            .ToList();
    }
}
