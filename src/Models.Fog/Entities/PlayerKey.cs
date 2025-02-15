namespace Ingweland.Fog.Models.Fog.Entities;

public record PlayerKey(string WorldId, int InGamePlayerId)
{
    public object[] ToCompositeKeyArray()
    {
        return [WorldId, InGamePlayerId];
    }
};
