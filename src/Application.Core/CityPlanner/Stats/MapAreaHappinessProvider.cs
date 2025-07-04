using System.Drawing;

namespace Ingweland.Fog.Application.Core.CityPlanner.Stats;

public class MapAreaHappinessProvider
{
    public required Rectangle Bounds { get; init; }
    public required int Value { get; init; }
}