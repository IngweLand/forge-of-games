using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Commands;

public class ChangeEntityUpgradeStateCommand(ICityPlanner cityPlanner, int cityMapEntityId, bool isUpgrading) : IUndoableCommand
{
    public void Execute()
    {
        cityPlanner.ChangeEntityUpgradeState(cityMapEntityId, isUpgrading);
    }

    public void Undo()
    {
        throw new NotImplementedException();
    }
}
