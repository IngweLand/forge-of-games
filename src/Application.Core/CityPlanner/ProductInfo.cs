namespace Ingweland.Fog.Application.Core.CityPlanner;

public record ProductInfo(string Id, int ProductionTime, IReadOnlyDictionary<string, string> Resources);
