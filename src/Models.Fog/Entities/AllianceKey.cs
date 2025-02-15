namespace Ingweland.Fog.Models.Fog.Entities;

public record AllianceKey(string WorldId, int InGameAllianceId)
{
    public object[] ToCompositeKeyArray()
    {
        return [WorldId, InGameAllianceId];
    }
};