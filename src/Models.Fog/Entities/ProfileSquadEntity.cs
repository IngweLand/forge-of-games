using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ingweland.Fog.Models.Fog.Entities;

public class ProfileSquadEntity
{
    private ProfileSquadKey? _key;
    public required int AbilityLevel { get; set; }

    public required string Age { get; set; }
    public required int AscensionLevel { get; set; }
    public required int AwakeningLevel { get; set; }

    public DateOnly CollectedAt { get; set; }

    public required ProfileSquadDataEntity Data { get; set; }

    public int Id { get; set; }

    [JsonIgnore]
    public ProfileSquadKey Key
    {
        get { return _key ??= new ProfileSquadKey(PlayerId, UnitId, CollectedAt); }
    }

    public required int Level { get; set; }
    public Player Player { get; set; } = null!;
    public int PlayerId { get; set; }

    public required string UnitId { get; set; }
}
