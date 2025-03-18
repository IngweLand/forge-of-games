namespace Ingweland.Fog.InnSdk.Hoh.Factories.Interfaces;

public interface IPvpRankingRequestPayloadFactory
{
    byte[] Create(int eventId);
}