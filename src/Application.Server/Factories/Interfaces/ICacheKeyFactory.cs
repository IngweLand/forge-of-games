using Ingweland.Fog.Application.Server.Interfaces;

namespace Ingweland.Fog.Application.Server.Factories.Interfaces;

public interface ICacheKeyFactory
{
    string HohData { get; }
    string Alliance(int allianceId);
    string Player(int playerId);
    string CreateKey<TRequest>(TRequest request) where TRequest : ICacheableRequest;
    string HohLocalizationData(string cultureCode);
    string HeroAbilityFeatures(string cultureCode);
}
