using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Commands;

public class AddEntityCommand(ICityPlanner cityPlanner, BuildingGroup buildingGroup) :IUndoableCommand
{
    private CityMapEntity? _addedEntity;

    public void Execute()
    {
        _addedEntity = cityPlanner.AddEntity(buildingGroup);
    }

    public void Undo()
    {
        if (_addedEntity != null)
        {
            cityPlanner.DeleteEntity(_addedEntity.Id);
        }
    }
}
