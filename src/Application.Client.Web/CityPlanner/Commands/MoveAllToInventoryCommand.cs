using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Commands;

public class MoveAllToInventoryCommand(ICityPlanner cityPlanner) : IUndoableCommand
{
    public void Execute()
    {
        cityPlanner.MoveAllToInventory();
    }

    public void Undo()
    {
        throw new NotImplementedException();
    }
}
