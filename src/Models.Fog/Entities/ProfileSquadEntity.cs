using System.Text.Json;
using System.Text.Json.Serialization;
using Ingweland.Fog.Models.Hoh.Entities.Battle;

namespace Ingweland.Fog.Models.Fog.Entities;

public class ProfileSquadEntity
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters = {new JsonStringEnumConverter()},
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
    };

    private ProfileSquadKey? _key;
    public required int AbilityLevel { get; set; }

    public required string Age { get; set; }
    public required int AscensionLevel { get; set; }

    public DateOnly CollectedAt { get; set; }

    public BattleUnitProperties Hero
    {
        get
        {
            var result =
                JsonSerializer.Deserialize<BattleUnitProperties>(SerializedHero, JsonSerializerOptions);
            return result ?? throw new InvalidOperationException($"Deserialized {nameof(Hero)} is null");
        }
        set => SerializedHero = JsonSerializer.Serialize(value, JsonSerializerOptions);
    }

    public int Id { get; set; }

    [JsonIgnore]
    public ProfileSquadKey Key
    {
        get { return _key ??= new ProfileSquadKey(PlayerId, UnitId, CollectedAt); }
    }

    public required int Level { get; set; }
    public Player Player { get; set; } = null!;
    public int PlayerId { get; set; }

    [JsonIgnore]
    public string SerializedHero { get; set; }

    [JsonIgnore]
    public string SerializedSupportUnit { get; set; }

    public BattleUnitProperties SupportUnit
    {
        get
        {
            var result =
                JsonSerializer.Deserialize<BattleUnitProperties>(SerializedSupportUnit,
                    JsonSerializerOptions);
            return result ??
                throw new InvalidOperationException($"Deserialized {nameof(SupportUnit)} is null");
        }
        set => SerializedSupportUnit = JsonSerializer.Serialize(value, JsonSerializerOptions);
    }

    public required string UnitId { get; set; }
}
