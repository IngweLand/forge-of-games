using System.Globalization;
using Ingweland.Fog.Application.Server.Factories.Interfaces;

namespace Ingweland.Fog.Application.Server.Factories;

public class CacheKeyFactory : ICacheKeyFactory
{
    public string HeroDto(string heroId)
    {
        return $"hero_dto-{heroId}-{CultureInfo.CurrentCulture.Name}";
    }

    public string HeroesBasicData()
    {
        return $"heroes-basic-data-{CultureInfo.CurrentCulture.Name}";
    }

    public string HohAges()
    {
        return $"hoh-ages-{CultureInfo.CurrentCulture.Name}";
    }

    public string HohResources()
    {
        return $"hoh-resources-{CultureInfo.CurrentCulture.Name}";
    }
}
