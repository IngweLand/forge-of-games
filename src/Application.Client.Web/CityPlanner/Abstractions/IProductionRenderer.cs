using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface IProductionRenderer
{
    Task InitializeAsync();
    void Draw(SKCanvas canvas, SKRect bounds, IEnumerable<string> resourceIds, int? productionTime, bool isUnchanged);
    void UpdateFontSize();
}
