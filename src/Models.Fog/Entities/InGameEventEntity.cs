using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class InGameEventEntity
{
    public required EventDefinitionId DefinitionId { get; set; }
    public required DateTime EndAt { get; set; }
    public required int EventId { get; set; }
    public int Id { get; set; }
    public required string InGameDefinitionId { get; set; }
    public required DateTime StartAt { get; set; }
    public required string WorldId { get; set; }
}
