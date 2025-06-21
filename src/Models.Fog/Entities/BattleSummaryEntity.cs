using System.Text.Json.Serialization;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class BattleSummaryEntity
{
    private BattleKey? _key;

    public required string BattleDefinitionId { get; set; }

    public required string EnemySquads { get; set; }
    public int Id { get; set; }

    public required byte[] InGameBattleId { get; set; }

    [JsonIgnore]
    public BattleKey Key
    {
        get { return _key ??= new BattleKey(WorldId, InGameBattleId); }
    }

    public required string PlayerSquads { get; set; }
    public BattleResultStatus ResultStatus { get; set; }

    // Not for all battle locations
    public Difficulty Difficulty { get; set; }
    public required string WorldId { get; set; }

    public ICollection<BattleUnitEntity> Units { get; set; } = new List<BattleUnitEntity>();
}
