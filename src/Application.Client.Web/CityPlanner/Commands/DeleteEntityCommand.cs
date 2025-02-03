using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Commands;

public class DeleteEntityCommand(ICityPlanner cityPlanner, CityMapEntity entity) :IUndoableCommand
{
    public void Execute()
    {
        cityPlanner.DeleteEntity(entity);
    }

    public void Undo()
    {
        cityPlanner.AddEntity(entity);
    }
}
