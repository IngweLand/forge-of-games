using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Commands;

public class RotateEntityCommand(ICityPlanner cityPlanner, CityMapEntity entity) :IUndoableCommand
{
    public void Execute()
    {
        cityPlanner.RotateEntity(entity);
    }

    public void Undo()
    {
        cityPlanner.RotateEntity(entity);
    }
}
