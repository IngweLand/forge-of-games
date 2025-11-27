using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Commands;

public class ToggleEntityLockStateCommand(ICityPlanner cityPlanner, int cityMapEntityId) : IUndoableCommand
{
    public void Execute()
    {
        cityPlanner.ToggleEntityLockState(cityMapEntityId);
    }

    public void Undo()
    {
        cityPlanner.ToggleEntityLockState(cityMapEntityId);
    }
}
