namespace Ingweland.Fog.Models.Fog.Entities;

public record PlayerRankingKey(string WorldId, int InGamePlayerId, DateOnly CollectedAt)
{
    public object[] ToCompositeKeyArray()
    {
        return [WorldId, InGamePlayerId, CollectedAt];
    }
};
