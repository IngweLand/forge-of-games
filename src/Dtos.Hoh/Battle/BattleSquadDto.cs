namespace Ingweland.Fog.Dtos.Hoh.Battle;

public class BattleSquadDto
{
    public int BattlefieldSlot { get; set; }
    public BattleUnitDto? Hero { get; init; }

    public BattleUnitDto? Unit { get; init; }
}
