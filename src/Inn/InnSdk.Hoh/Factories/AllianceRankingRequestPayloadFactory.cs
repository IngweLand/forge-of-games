using Google.Protobuf;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Factories.Interfaces;

namespace Ingweland.Fog.InnSdk.Hoh.Factories;

public class AllianceRankingRequestPayloadFactory : IAllianceRankingRequestPayloadFactory
{
    public byte[] Create(AllianceRankingType rankingType)
    {
        var payload = new AllianceRankingRequestDto {Type = rankingType};
        return payload.ToByteArray();
    }
}