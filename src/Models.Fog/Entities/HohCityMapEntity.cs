using System.Text.Json.Serialization;

namespace Ingweland.Fog.Models.Fog.Entities;

public class HohCityMapEntity
{
    public required string CityEntityId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? CustomizationId { get; set; }

    public int Id { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsLocked { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsRotated { get; set; }

    public required int Level { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? SelectedProductId { get; set; }

    public int X { get; set; }
    public int Y { get; set; }

    public HohCityMapEntity Clone()
    {
        return (HohCityMapEntity) MemberwiseClone();
    }
}
