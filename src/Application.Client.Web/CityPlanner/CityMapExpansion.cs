using System.Drawing;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class CityMapExpansion
{
    public required Rectangle Bounds { get; init; }
    public required string Id { get; init; }
    public bool IsLocked { get; set; }
}