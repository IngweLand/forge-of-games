using System.Diagnostics.CodeAnalysis;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public record BattleSquadViewModel : HeroProfileViewModel
{
    [SetsRequiredMembers]
    public BattleSquadViewModel(HeroProfileViewModel heroProfile, string? finalHitPointsPercent, bool isDead,
        int battlefieldSlot) :
        base(heroProfile)
    {
        FinalHitPointsPercent = finalHitPointsPercent;
        IsDead = isDead;
        BattlefieldSlot = battlefieldSlot;
    }

    // we need this empty ctor as well as init in properties for JSON deserialization.
    public BattleSquadViewModel()
    {
    }

    public int BattlefieldSlot { get; init; }

    public string? FinalHitPointsPercent { get; init; }
    public bool IsDead { get; init; }
}
