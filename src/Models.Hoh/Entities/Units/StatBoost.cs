using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

public class StatBoost
{
    public Calculation Calculation { get; set; }
    public int Order { get; set; }
    public StatAttribute? StatAttribute { get; set; }
    public UnitStatType UnitStatType { get; set; }
    public float Value { get; set; }
}