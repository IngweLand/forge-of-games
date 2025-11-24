using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public class BattleWaveSquadViewModel
{
    public required string Amount { get; init; }
    public required UnitColor Color { get; init; }
    public UnitColorAffinity? ColorAffinity { get; set; }
    public BattleUnitProperties? Hero { get; set; }
    public required string ImageUrl { get; init; }
    public required string Level { get; init; }
    public required string Name { get; init; }
    public required double Power { get; init; }
    public required string TypeIconUrl { get; init; }
}
