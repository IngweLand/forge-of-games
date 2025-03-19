using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Enums;

[ProtoContract]
public enum UnitStatType
{
    Undefined = 0,
    Accuracy,
    AoeDamageAmp,
    AssetRadius,
    Attack,
    AttackRadius,
    AttackRange,
    AttackSpeed,
    BaseDamage,
    BasicAttackDamageAmp,
    BurnDamageAmp,
    BleedDamageAmp,
    CritChance,
    CritDamage,
    Defense,
    DotDamageAmp,
    Evasion,
    ExpectedSquadSize,
    Focus,
    FocusRegen,
    HealGivenAmp,
    HealTakenAmp,
    HitPoints,
    HitTime,
    InitialFocusInSecondsBonus,
    MaxFocus,
    MaxHitPoints,
    MoveSpeed,
    ProjectileSpeed,
    Shield,
    ShieldTakenAmp,
    SingleTargetDamageAmp,
    SplashDamageDivisor,
    SquadRows,
    SquadSize,
    SquadSpacingX,
    SquadSpacingY,
    StatusEffectDurationAmp,
}