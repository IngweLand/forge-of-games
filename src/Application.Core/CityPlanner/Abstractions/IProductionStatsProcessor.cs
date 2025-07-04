namespace Ingweland.Fog.Application.Core.CityPlanner.Abstractions;

public interface IProductionStatsProcessor
{
    void UpdateProduction(CityMapEntity cityMapEntity);
    void UpdateProduction(CityMapEntity cityMapEntity, IReadOnlyDictionary<string, double> modifiers);
}
