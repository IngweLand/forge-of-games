using System.Diagnostics.CodeAnalysis;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public record BattleSquadViewModel : HeroProfileViewModel
{
    [SetsRequiredMembers]
    public BattleSquadViewModel(HeroProfileViewModel heroProfile, string? finalHitPointsPercent, bool isDead) :
        base(heroProfile)
    {
        FinalHitPointsPercent = finalHitPointsPercent;
        IsDead = isDead;
    }

    public BattleSquadViewModel()
    {
    }

    public string? FinalHitPointsPercent { get; init; }
    public bool IsDead { get; init; }
}
