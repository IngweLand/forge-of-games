using Ingweland.Fog.Inn.Models.Hoh;

namespace Ingweland.Fog.InnSdk.Hoh.Factories.Interfaces;

public interface IAllianceRankingRequestPayloadFactory
{
    byte[] Create(AllianceRankingType rankingType);
}