using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface IBuildingRenderer
{
    void RenderBuildings(SKCanvas canvas, IEnumerable<CityMapEntity> entities);
    void RenderBuilding(SKCanvas canvas, CityMapEntity entity);
    Task InitializeAsync();
}