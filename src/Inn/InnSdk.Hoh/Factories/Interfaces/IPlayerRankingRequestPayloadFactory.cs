using Ingweland.Fog.Inn.Models.Hoh;

namespace Ingweland.Fog.InnSdk.Hoh.Factories.Interfaces;

public interface IPlayerRankingRequestPayloadFactory
{
    byte[] Create(PlayerRankingType rankingType);
}
