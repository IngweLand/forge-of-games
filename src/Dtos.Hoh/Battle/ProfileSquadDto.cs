using Ingweland.Fog.Models.Hoh.Entities.Battle;

namespace Ingweland.Fog.Dtos.Hoh.Battle;

public class ProfileSquadDto
{
    public required BattleUnitProperties Hero { get; init; }

    public required BattleUnitProperties SupportUnit { get; init; }
}
