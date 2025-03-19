using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public class BattleWaveSquadViewModel
{
    public required string Amount { get; init; }
    public required UnitColor Color { get; init; }
    public required string ImageUrl { get; init; }
    public bool IsHero { get; init; }
    public required string Level { get; init; }
    public required string Name { get; init; }
    public required string TypeIconUrl { get; init; }
    public required double Power { get; init; }
    public UnitColorAffinity? ColorAffinity { get; set; }
}
