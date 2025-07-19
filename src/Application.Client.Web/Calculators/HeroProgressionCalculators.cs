using Ingweland.Fog.Application.Client.Web.Calculators.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Client.Web.Calculators;

public class HeroProgressionCalculators(ILogger<HeroProgressionCalculators> logger) : IHeroProgressionCalculators
{
    private static readonly Dictionary<string, int> AscensionMaterialsOrder = new List<string>
        {
            "resource.hero_xp",
            "food",

            "purple_crest_common",
            "red_crest_common",
            "green_crest_common",
            "yellow_crest_common",
            "blue_crest_common",

            "purple_crest_enhanced",
            "red_crest_enhanced",
            "green_crest_enhanced",
            "yellow_crest_enhanced",
            "blue_crest_enhanced",

            "purple_crest_superior",
            "red_crest_superior",
            "green_crest_superior",
            "yellow_crest_superior",
            "blue_crest_superior",

            "purple_crest_epic",
            "red_crest_epic",
            "green_crest_epic",
            "yellow_crest_epic",
            "blue_crest_epic",

            "shadow_dial",
            "war_horn",
            "fragrant_potpourri",

            "charta_terrestre",
            "crested_guidon",
            "herbal_poultice",

            "travelers_compass",
            "gilded_pennant",
            "hortus_sanitatis",

            "astrolabe_majesticum",
            "imperial_war_drum",
            "philosophers_stone",

            "folio_of_enlightenment",
            "tesla_core",
            "stardust_quartz",
            "prism_of_fate",
            "lantern_of_vigilance",
        }.Select((resource, index) => new {resource, index})
        .ToDictionary(x => x.resource, x => x.index);

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
                    new ResourceAmount
                    {
                        ResourceId = "resource.hero_xp",
                        Amount = src.Amount,
                    },
                    new ResourceAmount
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
            .Select(g => { return new ResourceAmount {ResourceId = g.Key, Amount = g.Sum(ra => ra.Amount)}; })
            .OrderBy(x => AscensionMaterialsOrder.GetValueOrDefault(x.ResourceId, int.MaxValue))
            .ToList();
    }

    public int CalculateAbilityCost(HeroAbilityDto heroAbility, int from, int to)
    {
        if (from < 1)
        {
            logger.LogWarning("Invalid from level {from}. Expected minimum 1.", from);
            return 0;
        }

        if (to <= from)
        {
            logger.LogWarning("To level {to} must be greater than from level {from}.", from, to);
        }

        return heroAbility.Levels.Skip(from - 1).Take(to - from).Sum(x => x.Cost);
    }
}
