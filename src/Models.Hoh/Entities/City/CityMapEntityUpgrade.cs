namespace Ingweland.Fog.Models.Hoh.Entities.City;

public class CityMapEntityUpgrade
{
    public DateTime CompleteAt { get; set; }
    public required string DefinitionId { get; init; }
    public bool IsStarted { get; set; }
    public DateTime StartedAt { get; set; }
}
