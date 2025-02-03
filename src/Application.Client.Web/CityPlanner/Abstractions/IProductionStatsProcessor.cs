namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface IProductionStatsProcessor
{
    void UpdateProduction(CityMapEntity cityMapEntity, CityMapState cityMapState);
}
