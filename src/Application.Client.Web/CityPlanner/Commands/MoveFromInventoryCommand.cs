using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Commands;

public class MoveFromInventoryCommand(ICityPlanner cityPlanner, BuildingGroup buildingGroup) : IUndoableCommand
{
    public void Execute()
    {
        cityPlanner.MoveFromInventory(buildingGroup);
    }

    public void Undo()
    {
        throw new NotImplementedException();
    }
}
