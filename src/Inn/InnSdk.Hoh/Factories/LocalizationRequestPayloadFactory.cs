using Google.Protobuf;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Factories.Interfaces;

namespace Ingweland.Fog.InnSdk.Hoh.Factories;

public class LocalizationRequestPayloadFactory : ILocalizationRequestPayloadFactory
{
    public byte[] Create(string locale)
    {
        var payload = new LocalizationRequest()
        {
            Locale = locale,
        };
        return payload.ToByteArray();
    }
}
