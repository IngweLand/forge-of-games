using Google.Protobuf;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Factories.Interfaces;

namespace Ingweland.Fog.InnSdk.Hoh.Factories;

public class PvpRankingRequestPayloadFactory : IPvpRankingRequestPayloadFactory
{
    public byte[] Create(int eventId)
    {
        var payload = new PvpRankingRequestDto() {EventId = eventId, EventType = "pvp_event.PvP"};
        return payload.ToByteArray();
    }
}