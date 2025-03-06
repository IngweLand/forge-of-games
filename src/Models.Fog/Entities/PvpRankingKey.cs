namespace Ingweland.Fog.Models.Fog.Entities;

public record PvpRankingKey(string WorldId, int InGamePlayerId, DateTime CollectedAt)
{
    public object[] ToCompositeKeyArray()
    {
        return [WorldId, InGamePlayerId, CollectedAt];
    }
};