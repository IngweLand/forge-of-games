namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface IProductionStatsProcessor
{
    void UpdateProduction(CityMapEntity cityMapEntity);
    void UpdateProduction(CityMapEntity cityMapEntity, IReadOnlyDictionary<string, double> modifiers);
}
