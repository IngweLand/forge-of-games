using System.Text.Json.Serialization;
using ProtoBuf;

namespace Ingweland.Fog.Models.Fog.Entities;

[ProtoContract]
public class HohCityMapEntity
{
    [ProtoMember(1)]
    public required string CityEntityId { get; set; }

    [ProtoMember(2)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? CustomizationId { get; set; }

    [ProtoMember(3)]
    public int Id { get; set; }

    [ProtoMember(4)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsLocked { get; set; }

    [ProtoMember(5)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsRotated { get; set; }

    [ProtoMember(6)]
    public required int Level { get; set; }

    [ProtoMember(7)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? SelectedProductId { get; set; }

    [ProtoMember(8)]
    public int X { get; set; }

    [ProtoMember(9)]
    public int Y { get; set; }

    public HohCityMapEntity Clone()
    {
        return (HohCityMapEntity) MemberwiseClone();
    }
}
