using Ingweland.Fog.Application.Server.Interfaces;

namespace Ingweland.Fog.Application.Server.Factories.Interfaces;

public interface ICacheKeyFactory
{
    string HeroDto(string heroId, Guid version);
    string RelicDtos(Guid version);
    string HeroesBasicData(Guid version);
    string HohAges(Guid version);
    string HohResources(Guid version);
    string Alliance(int allianceId);
    string Player(int playerId);
    string CreateKey<TRequest>(TRequest request) where TRequest : ICacheableRequest;
}
