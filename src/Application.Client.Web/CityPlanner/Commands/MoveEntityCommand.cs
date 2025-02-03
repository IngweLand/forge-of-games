using System.Drawing;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Commands;

public class MoveEntityCommand(ICityPlanner cityPlanner, CityMapEntity entity, Point oldLocation) : IUndoableCommand
{
    private readonly Point _newLocation = entity.Location;

    public void Execute()
    {
        cityPlanner.MoveEntity(entity, _newLocation);
    }

    public void Undo()
    {
        cityPlanner.MoveEntity(entity, oldLocation);
    }
}
