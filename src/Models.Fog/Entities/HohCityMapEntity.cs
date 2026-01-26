using System.Text.Json.Serialization;
using ProtoBuf;

namespace Ingweland.Fog.Models.Fog.Entities;

[ProtoContract]
public class HohCityMapEntity : IEquatable<HohCityMapEntity>
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

    /// <summary>
    ///     Indicates whether this entity is identical in both layouts.
    /// </summary>
    /// <remarks>
    ///     True if the entity exists in the previous layout and all comparison-relevant properties are equal;
    ///     otherwise false.
    /// </remarks>
    [ProtoIgnore]
    [JsonIgnore]
    public bool IsUnchanged { get; set; }

    [ProtoMember(10)]
    public bool IsUpgrading { get; set; }

    [ProtoMember(6)]
    public required int Level { get; set; }

    [ProtoMember(7)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? SelectedProductId { get; set; }

    [ProtoMember(8)]
    public int X { get; set; }

    [ProtoMember(9)]
    public int Y { get; set; }

    public bool Equals(HohCityMapEntity? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        // Id is intentionally ignored
        return CityEntityId == other.CityEntityId
            && CustomizationId == other.CustomizationId
            && IsLocked == other.IsLocked
            && IsRotated == other.IsRotated
            && Level == other.Level
            && SelectedProductId == other.SelectedProductId
            && X == other.X
            && Y == other.Y
            && IsUpgrading == other.IsUpgrading;
    }

    public override bool Equals(object? obj)
    {
        return obj is HohCityMapEntity other && Equals(other);
    }

    public override int GetHashCode()
    {
        // Id is intentionally ignored
        var hash = new HashCode();
        hash.Add(CityEntityId);
        hash.Add(CustomizationId);
        hash.Add(IsLocked);
        hash.Add(IsRotated);
        hash.Add(Level);
        hash.Add(SelectedProductId);
        hash.Add(X);
        hash.Add(Y);
        hash.Add(IsUpgrading);
        return hash.ToHashCode();
    }

    public static bool operator ==(HohCityMapEntity? left, HohCityMapEntity? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(HohCityMapEntity? left, HohCityMapEntity? right)
    {
        return !Equals(left, right);
    }

    public HohCityMapEntity Clone()
    {
        return (HohCityMapEntity) MemberwiseClone();
    }
}
