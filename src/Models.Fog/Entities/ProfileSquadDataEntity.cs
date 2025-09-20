using System.Text.Json;
using System.Text.Json.Serialization;
using Ingweland.Fog.Models.Hoh.Entities.Battle;

namespace Ingweland.Fog.Models.Fog.Entities;

public class ProfileSquadDataEntity
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters = {new JsonStringEnumConverter()},
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
    };

    public int ProfileSquadId { get; set; }
    public ProfileSquadEntity ProfileSquad { get; set; }
    public int Id { get; set; }
    
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
}
