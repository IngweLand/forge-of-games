using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Enums;

[ProtoContract]
public enum UnitStatType
{
    Undefined = 0,
    Accuracy,
    AssetRadius,
    Attack,
    AttackRadius,
    AttackRange,
    AttackSpeed,
    BaseDamage,
    BurnDamageAmp,
    CritChance,
    CritDamage,
    Defense,
    Evasion,
    ExpectedSquadSize,
    Focus,
    FocusRegen,
    HitPoints,
    HitTime,
    MaxFocus,
    MaxHitPoints,
    MoveSpeed,
    ProjectileSpeed,
    Shield,
    SplashDamageDivisor,
    SquadRows,
    SquadSize,
    SquadSpacingX,
    SquadSpacingY,
}
