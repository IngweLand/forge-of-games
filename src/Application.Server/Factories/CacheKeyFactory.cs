using Ingweland.Fog.Application.Server.Factories.Interfaces;

namespace Ingweland.Fog.Application.Server.Factories;

public class CacheKeyFactory : ICacheKeyFactory
{
    public string HeroDto(string heroId)
    {
        return $"hero_dto-{heroId}";
    }

    public string HeroesBasicData()
    {
        return "heroes-basic-data";
    }
}
