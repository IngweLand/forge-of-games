using System.Drawing;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Commands;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class CityMapEntityInteractionComponent(
    IMapGrid grid,
    ICityPlanner cityPlanner,
    ICommandManager commandManager,
    ICityPlannerCommandFactory commandFactory) : ICityMapEntityInteractionComponent
{
    private PointF _initOffset;
    private Point _initialGridLocation;

    public bool Start(PointF screenCoordinates)
    {
        if (cityPlanner.CityMapState.SelectedCityMapEntity == null)
        {
            return false;
        }

        if(cityPlanner.CityMapState.SelectedCityMapEntity.CanBePlaced)
        {
            _initialGridLocation = cityPlanner.CityMapState.SelectedCityMapEntity.Location;
        }
        
        var initScreenCoordinates = grid.GridToScreen(cityPlanner.CityMapState.SelectedCityMapEntity.Location);
        _initOffset = new PointF(initScreenCoordinates.X - screenCoordinates.X,
            initScreenCoordinates.Y - screenCoordinates.Y);
        return cityPlanner.CityMapState.SelectedCityMapEntity.Bounds.Contains(grid.ScreenToGrid(screenCoordinates));
    }

    public bool Drag(PointF screenDelta)
    {
        if (cityPlanner.CityMapState.SelectedCityMapEntity == null)
        {
            return false;
        }

        var withOffset = new PointF(screenDelta.X + _initOffset.X, screenDelta.Y + _initOffset.Y);
        var newLocation = grid.ScreenToGrid(withOffset);
        if (cityPlanner.CityMapState.SelectedCityMapEntity.Location == newLocation)
        {
            return false;
        }

        cityPlanner.MoveEntity(cityPlanner.CityMapState.SelectedCityMapEntity, newLocation);
        return true;
    }

    public void End()
    {
        if (cityPlanner.CityMapState.SelectedCityMapEntity == null || !cityPlanner.CanBePlaced(cityPlanner.CityMapState.SelectedCityMapEntity))
        {
            return;
        }

        var cmd = commandFactory.CreateMoveEntityCommand(cityPlanner.CityMapState.SelectedCityMapEntity, _initialGridLocation);
        commandManager.ExecuteCommand(cmd);
    }
}
