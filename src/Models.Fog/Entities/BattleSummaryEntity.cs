using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class BattleSummaryEntity
{
    private BattleKey? _key;

    public required string BattleDefinitionId { get; set; }

    public required BattleType BattleType { get; set; } = BattleType.Undefined;

    // Not for all battle locations
    public Difficulty Difficulty { get; set; }

    [NotMapped]
    public BattleSquadsEntity EnemySquads => Squads.First(s => s.Side == BattleSquadSide.Enemy);

    [NotMapped]
    public IEnumerable<BattleUnitEntity> EnemyUnits => Units.Where(s => s.Side == BattleSquadSide.Enemy);

    public int Id { get; set; }

    public required byte[] InGameBattleId { get; set; }

    [JsonIgnore]
    public BattleKey Key
    {
        get { return _key ??= new BattleKey(WorldId, InGameBattleId); }
    }

    public required DateOnly PerformedAt { get; init; }

    [NotMapped]
    public BattleSquadsEntity PlayerSquads => Squads.First(s => s.Side == BattleSquadSide.Player);

    [NotMapped]
    public IEnumerable<BattleUnitEntity> PlayerUnits => Units.Where(s => s.Side == BattleSquadSide.Player);

    public BattleResultStatus ResultStatus { get; set; }
    public required ICollection<BattleSquadsEntity> Squads { get; set; } = new List<BattleSquadsEntity>();

    public Guid? SubmissionId { get; set; }

    public ICollection<BattleUnitEntity> Units { get; set; } = new List<BattleUnitEntity>();
    public required string WorldId { get; set; }
}
