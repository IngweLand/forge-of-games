namespace Ingweland.Fog.InnSdk.Hoh.Factories.Interfaces;

public interface ILocalizationRequestPayloadFactory
{
    byte[] Create(string locale);
}
