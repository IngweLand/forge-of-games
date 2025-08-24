using System.Diagnostics.CodeAnalysis;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public record BattleSquadBasicViewModel : HeroProfileBasicViewModel
{
    [SetsRequiredMembers]
    public BattleSquadBasicViewModel(HeroProfileBasicViewModel heroProfile, string? finalHitPointsPercent, bool isDead,
        int battlefieldSlot) : base(heroProfile)
    {
        FinalHitPointsPercent = finalHitPointsPercent;
        IsDead = isDead;
        BattlefieldSlot = battlefieldSlot;
    }
    
    // we need this empty ctor as well as init in properties for JSON deserialization.
    public BattleSquadBasicViewModel()
    {
    }

    public int BattlefieldSlot { get; init; }
    public string? FinalHitPointsPercent { get; init; }

    public bool IsDead { get; init; }
}
