using Google.Protobuf;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Factories.Interfaces;

namespace Ingweland.Fog.InnSdk.Hoh.Factories;

public class GameDesignRequestPayloadFactory : IGameDesignRequestPayloadFactory
{
    public byte[] Create()
    {
        var payload = new GamedesignRequest() {Checksum = "invalid"};
        return payload.ToByteArray();
    }
}
