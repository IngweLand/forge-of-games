namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

public class PvpUnit
{
    public required PvpUnitDetails Hero { get; init; }
    public required PvpUnitDetails SupportUnit { get; init; }
}