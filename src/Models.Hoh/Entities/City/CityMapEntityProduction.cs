using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

public class CityMapEntityProduction
{
    public required string DefinitionId { get; init; }
    public bool IsStarted { get; set; }
    public ProductionSourceConstant Source { get; set; }
}