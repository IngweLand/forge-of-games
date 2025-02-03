namespace Ingweland.Fog.Inn.Models.Hoh;

public  sealed partial class LocaResponseContainer
{
    public LocaResponse Data => PackedContent.Unpack<LocaResponse>();
}
