using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Commands;

public class PurgeInventoryCommand(ICityPlanner cityPlanner) : IUndoableCommand
{
    public void Execute()
    {
        cityPlanner.PurgeInventory();
    }

    public void Undo()
    {
        throw new NotImplementedException();
    }
}
