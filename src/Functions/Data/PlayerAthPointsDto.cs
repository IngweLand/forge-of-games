namespace Ingweland.Fog.Functions.Data;

public class PlayerAthPointsDto
{
    public required int EventId { get; init; }
    public required int PlayerId { get; init; }
    public required string PlayerName { get; init; }
    public required int Points { get; init; }
}
