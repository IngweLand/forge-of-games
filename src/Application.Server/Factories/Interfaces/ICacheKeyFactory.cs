namespace Ingweland.Fog.Application.Server.Factories.Interfaces;

public interface ICacheKeyFactory
{
    string HeroDto(string heroId);
    string HeroesBasicData();
}
