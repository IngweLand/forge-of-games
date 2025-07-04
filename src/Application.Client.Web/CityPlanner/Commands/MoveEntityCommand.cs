using System.Drawing;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Core.CityPlanner;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Commands;

public class MoveEntityCommand(ICityPlanner cityPlanner, CityMapEntity entity, Point oldLocation) : IUndoableCommand
{
    private readonly Point _newLocation = entity.Location;
    private readonly int _entityId = entity.Id;

    public void Execute()
    {
        cityPlanner.MoveEntity(_entityId, _newLocation);
    }

    public void Undo()
    {
        cityPlanner.MoveEntity(_entityId, oldLocation);
    }
}
