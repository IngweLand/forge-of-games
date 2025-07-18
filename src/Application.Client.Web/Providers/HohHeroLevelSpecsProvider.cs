using System.Collections.Concurrent;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.Providers;

public class HohHeroLevelSpecsProvider : IHohHeroLevelSpecsProvider
{
    private const int ASCENSION_PERIOD = 10;

    private readonly ConcurrentDictionary<int, IReadOnlyCollection<HeroLevelSpecs>> _levelSpecs = new();

    public IReadOnlyCollection<HeroLevelSpecs> Get(int maxLevel)
    {
        return _levelSpecs.GetOrAdd(maxLevel, SpecsFactory);
    }

    private static IReadOnlyCollection<HeroLevelSpecs> SpecsFactory(int maxLevel)
    {
        List<HeroLevelSpecs> levels = [];
        var ascensionLevel = 0;
        for (var i = 1; i < maxLevel + 1; i++)
        {
            if (i % ASCENSION_PERIOD == 0 && i < maxLevel)
            {
                levels.Add(new HeroLevelSpecs
                {
                    Title = $"{i} > {i + 10}",
                    Level = i,
                    AscensionLevel = ascensionLevel,
                    IsAscension = true,
                });
                ascensionLevel = i / ASCENSION_PERIOD;
            }

            levels.Add(new HeroLevelSpecs
            {
                Title = i.ToString(),
                Level = i,
                AscensionLevel = ascensionLevel,
            });
        }

        return levels;
    }
}
