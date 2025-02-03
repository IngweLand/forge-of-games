using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface IHappinessStatsViewModelFactory
{
    HappinessStatsViewModel Create(CityStats stats);
}
