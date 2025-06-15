using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Commands;

public class RotateEntityCommand(ICityPlanner cityPlanner, int cityMapEntityId) :IUndoableCommand
{
    public void Execute()
    {
        cityPlanner.RotateEntity(cityMapEntityId);
    }

    public void Undo()
    {
        cityPlanner.RotateEntity(cityMapEntityId);
    }
}
