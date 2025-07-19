using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Hoh.Entities.Research;

public class ResearchStateTechnology
{
    public required string TechnologyId { get; init; }
    public TechnologyState State { get; init; }
}
