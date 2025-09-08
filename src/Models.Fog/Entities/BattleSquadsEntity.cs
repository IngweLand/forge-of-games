using Ingweland.Fog.Models.Fog.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class BattleSquadsEntity
{
    public int BattleId { get; set; }
    public int Id { get; set; }
    public required BattleSquadSide Side { get; set; }
    public required string Squads { get; set; }
}
