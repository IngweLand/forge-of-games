using Google.Protobuf;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Factories.Interfaces;

namespace Ingweland.Fog.InnSdk.Hoh.Factories;

public class PlayerRankingRequestPayloadFactory : IPlayerRankingRequestPayloadFactory
{
    public byte[] Create(PlayerRankingType rankingType)
    {
        var payload = new PlayerRankingRequestDto() {Type = rankingType};
        return payload.ToByteArray();
    }
}
