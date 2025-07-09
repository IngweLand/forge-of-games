using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Commands;

public class MoveToInventoryCommand(ICityPlanner cityPlanner, IReadOnlySet<int> cityMapEntityIds) : IUndoableCommand
{
    public void Execute()
    {
        cityPlanner.MoveToInventory(cityMapEntityIds);
    }

    public void Undo()
    {
        throw new NotImplementedException();
    }
}
