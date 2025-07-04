using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;
using Ingweland.Fog.Application.Core.CityPlanner.Stats;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface IHappinessStatsViewModelFactory
{
    HappinessStatsViewModel Create(CityStats stats);
}
