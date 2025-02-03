using Ingweland.Fog.Application.Client.Web.Factories;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.Providers;

public class HohHeroLevelSpecsProvider : IHohHeroLevelSpecsProvider
{
    private const int ASCENSION_PERIOD = 10;

    private IDictionary<int, IReadOnlyCollection<HeroLevelSpecs>> _levelSpecs =
        new Dictionary<int, IReadOnlyCollection<HeroLevelSpecs>>();

    public IReadOnlyCollection<HeroLevelSpecs> Get(int maxLevel)
    {
        if (_levelSpecs.TryGetValue(maxLevel, out var levelSpecs))
        {
            return levelSpecs;
        }

        List<HeroLevelSpecs> levels = new();
        var ascensionLevel = 0;
        for (var i = 1; i < maxLevel + 1; i++)
        {
            if (i % ASCENSION_PERIOD == 0 && i < maxLevel)
            {
                levels.Add(new HeroLevelSpecs()
                {
                    Title = $"{i} > {i + 10}",
                    Level = i,
                    AscensionLevel = ascensionLevel,
                    IsAscension = true,
                });
                ascensionLevel = i / ASCENSION_PERIOD;
            }

            levels.Add(new HeroLevelSpecs()
            {
                Title = i.ToString(),
                Level = i,
                AscensionLevel = ascensionLevel,
            });
        }

        _levelSpecs.Add(maxLevel, levels);
        return levels;
    }
}
