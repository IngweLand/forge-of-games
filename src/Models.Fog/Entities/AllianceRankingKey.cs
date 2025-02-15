namespace Ingweland.Fog.Models.Fog.Entities;

public record AllianceRankingKey(string WorldId, int InGameAllianceId, DateOnly CollectedAt)
{
    public object[] ToCompositeKeyArray()
    {
        return [WorldId, InGameAllianceId, CollectedAt];
    }
};