using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Dtos.Hoh.Battle;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;

public interface IBattleStatsViewModelFactory
{
    BattleStatsViewModel Create(BattleStatsDto statsDto);
}
