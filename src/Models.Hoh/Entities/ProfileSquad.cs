using Ingweland.Fog.Models.Hoh.Entities.Battle;

namespace Ingweland.Fog.Models.Hoh.Entities;

public class ProfileSquad
{
    public int Place { get; set; }

    public required BattleUnitProperties Hero { get; set; }

    public required BattleUnitProperties SupportUnit { get; set; }
}
