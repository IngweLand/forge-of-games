namespace Ingweland.Fog.Models.Fog.Entities;

public record PlayerKeyExtended(int Id, string WorldId, int InGamePlayerId) : PlayerKey(WorldId, InGamePlayerId);
