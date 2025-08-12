using Ingweland.Fog.Application.Server.Interfaces;

namespace Ingweland.Fog.Application.Server.Factories.Interfaces;

public interface ICacheKeyFactory
{
    string HeroDto(string heroId);
    string RelicDtos();
    string HeroesBasicData();
    string HohAges();
    string HohResources();
    string Alliance(int allianceId);
    string Player(int playerId);
    string CreateKey<TRequest>(TRequest request) where TRequest : ICacheableRequest;
}
